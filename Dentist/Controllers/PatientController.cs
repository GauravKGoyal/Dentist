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
    public class PatientController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        // can be inherited
        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = Db
                .People
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
                var patient = Mapper.Map<Person>(view);
                patient.PersonRole = PersonRole.Patient;
                Db.People.Add(patient);

                // add practice
                var practiceToAdd = Db.Practices.Find(view.PatientViewPracticeId);
                patient.Practices = new List<Practice>();
                patient.Practices.Add(practiceToAdd);

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
            
            Person patient = Db.People
                            .Include(x =>x.Address)
                            .Include(x => x.Practices)
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
                var patient = Db.People
                    .Include(x => x.Practices)
                    .Include(x => x.Address)
                    .First(x => x.Id == view.Id);
                Mapper.Map(view, patient);
                
                // remove practice
                patient.Practices.Clear();

                // add practice
                var practiceToAdd = Db.Practices.Find(view.PatientViewPracticeId);
                patient.Practices.Add(practiceToAdd);

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
            var patient = Db.People.Find(id);
            patient.IsDeleted = true;
            Db.SaveChanges();
            return Json(new {Success=true});
        }

    }
}
