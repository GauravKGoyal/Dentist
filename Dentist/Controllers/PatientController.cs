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
    public class PatientController : BaseController
    {
        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = Db.Paitients.Where(x => x.IsDeleted != true)
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
            var query = Db
                .Paitients
                .Where(x=>x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Patient);
            var projectedQuery = query.ProjectTo<PatientListView>();

            var result = projectedQuery.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new PatientView()
            {
                Address = new AddressView(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientView view)
        {
            if (ModelState.IsValid)
            {
                var patient = Mapper.Map<Paitient>(view);
                patient.PersonRole = PersonRole.Patient;
                Db.Paitients.Add(patient);

                // add practice
                var practiceToAdd = Db.Practices.Find(view.PatientViewPracticeId);
                patient.Practice = practiceToAdd;

                Db.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = patient.Id });
            }

            return View(view);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var patient = Db.Paitients
                            .Include(x =>x.Address)
                            .Include(x => x.Practice)
                            .First(x => x.Id == id);

            if (patient.PersonRole != PersonRole.Patient)
            {
                throw new Exception(string.Format("Person with id {0} is not a patient", id));
            }

            var view = Mapper.Map<PatientView>(patient);

            return View("Create", view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientView view)
        {
            if (ModelState.IsValid)
            {
                var patient = Db.Paitients
                    .Include(x => x.Practice)
                    .Include(x => x.Address)
                    .First(x => x.Id == view.Id);
                Mapper.Map(view, patient);
                               
                // add practice
                var practiceToAdd = Db.Practices.Find(view.PatientViewPracticeId);
                patient.Practice = practiceToAdd;

                Db.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = patient.Id });
            }
            return View("Create", view);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var patient = Db.Paitients.Find(id);
            patient.IsDeleted = true;
            Db.SaveChanges();
            return Json(new {Success=true});
        }

    }
}
