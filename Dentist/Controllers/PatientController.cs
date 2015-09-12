using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;
using Dentist.Models.Patient;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    [Authorize]
    public class PatientController : BaseController
    {
        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = ReadContext.Paitients.Where(x => x.IsDeleted != true)
           .Select(x => new
           {
               x.Id,
               Text = x.FirstName + " " + x.LastName,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Phone = x.Phone,
           })
           .OrderBy(x => x.Text);


            if (!string.IsNullOrEmpty(text))
            {
                var patient = query.Where(p => p.Text.Contains(text));
                return Json(patient, JsonRequestBehavior.AllowGet);
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        // can be inherited
        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = ReadContext
                .Paitients
                .Where(x => x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Patient);
            var projectedQuery = query.ProjectTo<PatientListViewModel>();

            var result = projectedQuery.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new PatientViewModel()
            {
                Address = new AddressViewModel(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var patient = new Paitient();
                Mapper.Map(viewModel, patient);
                patient.Practice = WriteContext.Practices.Find(viewModel.PatientViewPracticeId);
                WriteContext.Paitients.Add(patient);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? RedirectToAction("Index") : RedirectToAction("Edit", new { @id = patient.Id });
                }
            }

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patient = ReadContext.Paitients
                            .Include(x => x.Address)
                            .Include(x => x.Practice)
                            .First(x => x.Id == id);

            if (patient.PersonRole != PersonRole.Patient)
            {
                throw new Exception(string.Format("Person with id {0} is not a patient", id));
            }

            var view = Mapper.Map<PatientViewModel>(patient);

            return View("Create", view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var patient = WriteContext.Paitients.Find(viewModel.Id);
                Mapper.Map(viewModel, patient);
                patient.Practice = WriteContext.Practices.Find(viewModel.PatientViewPracticeId);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? RedirectToAction("Index") : RedirectToAction("Edit", new { @id = patient.Id });
                }
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var errorMessage = "";
            var patient = WriteContext.Paitients.Find(id);
            patient.IsDeleted = true;
            var changesSaved = WriteContext.TrySaveChanges(out errorMessage);
            return Json(new { Success = changesSaved, ErrorMessage = errorMessage });
        }

        //private IPatientService _patientService;
        //public IPatientService PatientService
        //{
        //    get
        //    {
        //        if (_patientService == null)
        //        {
        //            _patientService = DependencyInjectionConfig.Container.Resolve<IPatientService>();
        //        }
        //        return _patientService;
        //    }
        //}

        #region  -------------------------------------------------------------------------------------
        public ActionResult GetVitalSignBrowserItems([DataSourceRequest] DataSourceRequest request, int patientId)
        {
            if (patientId == 0)
            {
                throw new ArgumentException("Patient id cannot be 0", "patientId");
            }

            var query = ReadContext.Set<VitalSign>().Where(x => x.PatientId == patientId)
                        .ProjectTo<VitalSignViewModel>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateVitalSign([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<VitalSignViewModel> viewModels, int patientId)
        {
            var results = new List<VitalSignViewModel>();
            if (viewModels != null && ModelState.IsValid)
            {
                foreach (var viewModel in viewModels)
                {
                    var model = Mapper.Map<VitalSign>(viewModel);
                    model.PatientId = patientId;
                    WriteContext.Set<VitalSign>().Add(model);
                    WriteContext.TrySaveChanges(ModelState);

                    viewModel.Id = model.Id;
                    results.Add(viewModel);
                }

            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateVitalSign([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<VitalSignViewModel> viewModels)
        {
             viewModels = viewModels as IList<VitalSignViewModel> ?? viewModels.ToList();

            if (ModelState.IsValid)
            {
                foreach (var viewModel in viewModels)
                {
                    var model = WriteContext.Set<VitalSign>().Find(viewModel.Id);
                    Mapper.Map(viewModel, model);
                }
                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(viewModels.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteVitalSign([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<VitalSignViewModel> viewModels)
        {
            viewModels = viewModels as IList<VitalSignViewModel> ?? viewModels.ToList();

            foreach (var viewModel in viewModels)
            {
                var model = new VitalSign() { Id = viewModel.Id };
                WriteContext.Set<VitalSign>().Attach(model);
                WriteContext.Set<VitalSign>().Remove(model);
            }
            WriteContext.TrySaveChanges(ModelState);

            return Json(viewModels.ToDataSourceResult(request, ModelState));
        }
        #endregion
    }
}
