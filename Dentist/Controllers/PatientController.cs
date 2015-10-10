using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Controllers.Base;
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
            var query = ReadContext.Patients.Where(x => x.IsDeleted != true)
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
                .Patients
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
                var patient = new Patient();
                Mapper.Map(viewModel, patient);
                patient.Practice = WriteContext.Practices.Find(viewModel.PatientViewPracticeId);
                WriteContext.Patients.Add(patient);
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

            var patient = ReadContext.Patients
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
                var patient = WriteContext.Patients.Find(viewModel.Id);
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
            var patient = WriteContext.Patients.Find(id);
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
        
       

    }
}
