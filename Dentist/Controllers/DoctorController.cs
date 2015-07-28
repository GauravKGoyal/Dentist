using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Models;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    [Authorize]
    public class DoctorController : BaseController
    {
        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = Context.Doctors.Where(x => x.IsDeleted != true)
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
            var query = Context.Doctors.Where(x => x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Doctor);
            var projectedQuery = query.ProjectTo<DoctorListViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
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
                var doctor = Mapper.Map<Doctor>(viewModel);
                doctor.PersonRole = PersonRole.Doctor;

                Context.Doctors.Add(doctor);

                // add practices
                var practicesToAdd = Context.Practices.Where(practice => viewModel.Practices.Contains(practice.Id));
                doctor.Practices = new List<Practice>();
                doctor.Practices.AddRange(practicesToAdd);

                // Daily availability
                // for each practice add availability
                foreach (Practice practice in practicesToAdd)
                {
                    doctor.SetDefaultWeeklyAvailabilityForPractice(practice.Id);
                }

                Context.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = doctor.Id });
            }

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doctor = Context.Doctors
                            .Include(x => x.Address)
                            .Include(x => x.Practices)
                            .First(x => x.Id == id);
            if (doctor.PersonRole != PersonRole.Doctor)
            {
                throw new Exception(string.Format("Person with id {0} is not a doctor", id));
            }
            var doctorView = Mapper.Map<DoctorViewModel>(doctor);

            return View("Create", doctorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = Context.Doctors
                    .Include(x => x.Practices)
                    .Include(x => x.Address)
                    .First(x => x.Id == viewModel.Id);
                Mapper.Map(viewModel, doctor);

                // remove practices
                var practiceIdsToRemove = doctor.Practices
                    .Where(practice => !viewModel.Practices.Contains(practice.Id))
                    .Select(x =>x.Id)
                    .ToList();
                doctor.Practices.RemoveAll(practice => !viewModel.Practices.Contains(practice.Id));

                // add practices
                var practiceIdsToAdd = viewModel.Practices
                    .Where(practiceId => doctor.Practices.All(practice => practice.Id != practiceId))
                    .ToList();
                var practicesToAdd = Context.Practices.Where(practice => practiceIdsToAdd.Contains(practice.Id));
                doctor.Practices.AddRange(practicesToAdd);

                // Daily availability
                // for each removed practice remove daily availability
                foreach (int practiceId in practiceIdsToRemove)
                {
                    var tempPracticeId = practiceId;
                    var dailyAvailabilitiesToRemove =
                        Context.DailyAvailabilities.Where(x => x.DoctorId == doctor.Id && x.PracticeId == tempPracticeId);
                    Context.DailyAvailabilities.RemoveRange(dailyAvailabilitiesToRemove);
                }

                // for each added practice add daily availability
                foreach (int practiceId in practiceIdsToAdd)
                {
                    doctor.SetDefaultWeeklyAvailabilityForPractice(practiceId);
                }


                Context.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = doctor.Id });
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var doctor = Context.Doctors.Find(id);
            doctor.IsDeleted = true;
            Context.SaveChanges();
            return Json(new { Success = true });
        }

    }
}
