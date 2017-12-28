using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Controllers.Base;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Dentist.Controllers
{
    [Authorize]
    public class DoctorController : BaseController
    {


        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = ReadContext.Doctors.Where(x => x.IsDeleted != true)
           .Select(x => new
           {
               x.Id,
               Text = x.FirstName + " " + x.LastName,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Phone = x.Phone,
               Color = x.Color
           })
           .OrderBy(x => x.Text);


            if (!string.IsNullOrEmpty(text))
            {
                var doctor = query.Where(p => p.Text.Contains(text));
                return Json(doctor, JsonRequestBehavior.AllowGet);
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = ReadContext.Doctors
                .Where(x => x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Doctor);
            var projectedQuery = query.ProjectTo<DoctorListViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);

            var doctorViewModels = ((IEnumerable<DoctorListViewModel>)result.Data);
            var doctorIds = doctorViewModels.Select(x => x.Id);

            foreach (var doctorListViewModel in doctorViewModels)
            {
                doctorListViewModel.AvatarId = ReadContext.Files
                    .Where(f => f.FileType == FileType.Avatar)
                    .Where(f => f.Persons.Any(p => p.Id == doctorListViewModel.Id))
                    .OrderByDescending(f => f.Id)
                    .Select(f => f.Id)
                    .ToList()
                    .FirstOrDefault();

            }

            foreach (var doctorListViewModel in doctorViewModels)
            {
                var qualifications = ReadContext.Qualifications
                    .Where(f => f.DoctorId == doctorListViewModel.Id)
                    .Select(x => x.Name)
                    .ToList();
                doctorListViewModel.QualificationsName = string.Join(",", qualifications);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new DoctorViewModel()
            {
                Address = new AddressViewModel(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor();// or WriteContext.Doctors.Create(); 
                doctor.Context = WriteContext;
                viewModel.CopyTo(doctor);
                WriteContext.Doctors.Add(doctor);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? RedirectToAction("Index") : RedirectToAction("Edit", new { @id = doctor.Id });
                }
            }

            return View(viewModel);
        }

        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doctor = ReadContext.Doctors
                            .Include(x => x.Address)
                            .Include(x => x.Practices)
                            .Include(x => x.Practices.Select(p => p.Address))
                            .Include(x => x.Services)
                            .Include(x => x.Specializations)
                            .Include(x => x.Qualifications)
                            .First(x => x.Id == id);
            if (doctor.PersonRole != PersonRole.Doctor)
            {
                throw new Exception(string.Format("Person with id {0} is not a doctor", id));
            }
            var doctorView = new DoctorReportViewModel(doctor, ReadContext);

            return View("DoctorReportView", doctorView);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doctor = ReadContext.Doctors
                            .Include(x => x.Address)
                            .Include(x => x.Registration)
                            .Include(x => x.Practices)
                            .Include(x => x.Services)
                            .Include(x => x.Memberships)
                            .Include(x => x.Specializations)
                            .Include(x => x.Files)
                            .First(x => x.Id == id);
            if (doctor.PersonRole != PersonRole.Doctor)
            {
                throw new Exception(string.Format("Person with id {0} is not a doctor", id));
            }
            var doctorView = new DoctorViewModel();
            doctorView.CopyFrom(doctor);

            return View("Create", doctorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = WriteContext.Doctors.Find(viewModel.Id);
                doctor.Context = WriteContext;
                viewModel.CopyTo(doctor);

                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? RedirectToAction("Index") : RedirectToAction("Edit", new { @id = doctor.Id });
                }
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var errorMessage = "";
            Doctor doctor = WriteContext
                .Doctors
                .Include(x => x.Practices)
                .First(x => x.Id == id);
            doctor.Context = WriteContext;
            doctor.IsDeleted = true;
            var changesSaved = WriteContext.TrySaveChanges(out errorMessage);
            return Json(new { Success = changesSaved, ErrorMessage = errorMessage });
        }

        #region Qualification
        public ActionResult GetQualificationBrowserItems([DataSourceRequest] DataSourceRequest request, int doctorId)
        {
            if (doctorId == 0)
            {
                throw new ArgumentException("Doctor id cannot be 0", "doctorId");
            }

            var query = ReadContext.Set<Qualification>().Where(x => x.DoctorId == doctorId)
                        .ProjectTo<QualificationViewModel>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateQualification([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<QualificationViewModel> viewModels, int doctorId)
        {
            var results = new List<QualificationViewModel>();
            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var qualificationViewModel in viewModels)
                {
                    var qualification = Mapper.Map<Qualification>(qualificationViewModel);
                    qualification.Context = WriteContext;
                    qualification.DoctorId = doctorId;
                    WriteContext.Qualifications.Add(qualification);
                    WriteContext.TrySaveChanges(ModelState);

                    qualificationViewModel.Id = qualification.Id;
                    results.Add(qualificationViewModel);
                }

            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateQualification([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<QualificationViewModel> viewModels)
        {
            var qualificationViewModels = viewModels as IList<QualificationViewModel> ?? viewModels.ToList();

            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var qualificationViewModel in qualificationViewModels)
                {
                    var qualification = WriteContext.Qualifications.Find(qualificationViewModel.Id);
                    qualification.Context = WriteContext;
                    Mapper.Map(qualificationViewModel, qualification);
                }
                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(qualificationViewModels.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteQualification([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<QualificationViewModel> viewModels)
        {
            var qualificationViewModelList = viewModels as IList<QualificationViewModel> ?? viewModels.ToList();

            foreach (var qualificationViewModel in qualificationViewModelList)
            {
                var qualification = new Qualification() { Id = qualificationViewModel.Id };
                WriteContext.Qualifications.Attach(qualification);
                WriteContext.Qualifications.Remove(qualification);
            }
            WriteContext.TrySaveChanges(ModelState);

            return Json(qualificationViewModelList.ToDataSourceResult(request, ModelState));
        }
        # endregion 

        #region Experience
        public ActionResult GetExperienceBrowserItems([DataSourceRequest] DataSourceRequest request, int doctorId)
        {
            if (doctorId == 0)
            {
                throw new ArgumentException("Doctor id cannot be 0", "doctorId");
            }

            var query = ReadContext.Set<Experience>().Where(x => x.DoctorId == doctorId)
                        .ProjectTo<ExperienceViewModel>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateExperience([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ExperienceViewModel> viewModels, int doctorId)
        {
            var results = new List<ExperienceViewModel>();
            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var experienceViewModel in viewModels)
                {
                    var experience = Mapper.Map<Experience>(experienceViewModel);
                    experience.DoctorId = doctorId;
                    WriteContext.Experiences.Add(experience);
                    WriteContext.TrySaveChanges(ModelState);

                    experienceViewModel.Id = experience.Id;
                    results.Add(experienceViewModel);
                }

            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateExperience([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ExperienceViewModel> viewModels)
        {
            var experienceViewModels = viewModels as IList<ExperienceViewModel> ?? viewModels.ToList();

            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var experienceViewModel in experienceViewModels)
                {
                    var experience = WriteContext.Experiences.Find(experienceViewModel.Id);
                    Mapper.Map(experienceViewModel, experience);
                }
                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(experienceViewModels.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteExperience([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ExperienceViewModel> viewModels)
        {
            var experienceViewModels = viewModels as IList<ExperienceViewModel> ?? viewModels.ToList();

            foreach (var experienceViewModel in experienceViewModels)
            {
                var experience = new Experience() { Id = experienceViewModel.Id };
                WriteContext.Experiences.Attach(experience);
                WriteContext.Experiences.Remove(experience);
            }
            WriteContext.TrySaveChanges(ModelState);

            return Json(experienceViewModels.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Award
        public ActionResult GetAwardBrowserItems([DataSourceRequest] DataSourceRequest request, int doctorId)
        {
            if (doctorId == 0)
            {
                throw new ArgumentException("Doctor id cannot be 0", "doctorId");
            }

            var query = ReadContext.Set<Award>().Where(x => x.DoctorId == doctorId)
                        .ProjectTo<AwardViewModel>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAward([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AwardViewModel> viewModels, int doctorId)
        {
            var results = new List<AwardViewModel>();
            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var awardViewModel in viewModels)
                {
                    var award = Mapper.Map<Award>(awardViewModel);
                    award.DoctorId = doctorId;
                    WriteContext.Awards.Add(award);
                    WriteContext.TrySaveChanges(ModelState);

                    awardViewModel.Id = award.Id;
                    results.Add(awardViewModel);
                }

            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAward([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AwardViewModel> viewModels)
        {
            var awardViewModels = viewModels as IList<AwardViewModel> ?? viewModels.ToList();

            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var awardViewModel in awardViewModels)
                {
                    var award = WriteContext.Awards.Find(awardViewModel.Id);
                    Mapper.Map(awardViewModel, award);
                }
                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(awardViewModels.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAward([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AwardViewModel> viewModels)
        {
            var awardViewModels = viewModels as IList<AwardViewModel> ?? viewModels.ToList();

            foreach (var awardViewModel in awardViewModels)
            {
                var award = new Award() { Id = awardViewModel.Id };
                WriteContext.Awards.Attach(award);
                WriteContext.Awards.Remove(award);
            }
            WriteContext.TrySaveChanges(ModelState);

            return Json(awardViewModels.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region DailyAvailability
        public ActionResult GetDailyAvailabilityBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = ReadContext.DailyAvailabilities
                        .Include(x => x.Practice)
                        .Where(x => x.DoctorId == personId)
                        .ProjectTo<DailyAvailabilityViewModel>();

            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Mapper.Map<DailyAvailability>(viewModel);
                WriteContext.DailyAvailabilities.Add(dailyAvailability);
                WriteContext.TrySaveChanges(ModelState);
                // load practice to load practice name
                var practice = dailyAvailability.Practice;
            }

            // Return the inserted product. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = WriteContext.DailyAvailabilities.First(x => x.Id == viewModel.Id);
                Mapper.Map(viewModel, dailyAvailability);
                WriteContext.TrySaveChanges(ModelState);
                // load practice to load practice name
                var practice = dailyAvailability.Practice;
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = WriteContext.DailyAvailabilities.First(x => x.Id == viewModel.Id);
                WriteContext.DailyAvailabilities.Remove(dailyAvailability);
                WriteContext.TrySaveChanges(ModelState);
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion
    }
}
