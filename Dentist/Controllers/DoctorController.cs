using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Models;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data.Entity;
using WebGrease.Css.Extensions;

namespace Dentist.Controllers
{
    [Authorize]
    public class DoctorController : BaseController
    {
        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = Db.Doctors.Where(x => x.IsDeleted != true)
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
            var query = Db.Doctors.Where(x => x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Doctor);
            var projectedQuery = query.ProjectTo<DoctorListView>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new DoctorView()
            {
                Address = new AddressView(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorView view)
        {
            if (ModelState.IsValid)
            {
                var doctor = Mapper.Map<Doctor>(view);
                doctor.PersonRole = PersonRole.Doctor;

                Db.Doctors.Add(doctor);

                // add practices
                var practicesToAdd = Db.Practices.Where(practice => view.Practices.Contains(practice.Id));
                doctor.Practices = new List<Practice>();
                doctor.Practices.AddRange(practicesToAdd);

                // Daily availability
                // for each practice add availability
                foreach (Practice practice in practicesToAdd)
                {
                    doctor.SetupWeeklyAvailabilityForPractice(practice.Id);
                }

                Db.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = doctor.Id });
            }

            return View(view);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var doctor = Db.Doctors
                            .Include(x => x.Address)
                            .Include(x => x.Practices)
                            .First(x => x.Id == id);
            if (doctor.PersonRole != PersonRole.Doctor)
            {
                throw new Exception(string.Format("Person with id {0} is not a doctor", id));
            }
            var doctorView = Mapper.Map<DoctorView>(doctor);

            return View("Create", doctorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorView view)
        {
            if (ModelState.IsValid)
            {
                var doctor = Db.Doctors
                    .Include(x => x.Practices)
                    .Include(x => x.Address)
                    .First(x => x.Id == view.Id);
                Mapper.Map(view, doctor);

                // remove practices
                var practiceIdsToRemove = doctor.Practices
                    .Where(practice => !view.Practices.Contains(practice.Id))
                    .Select(x =>x.Id)
                    .ToList();
                doctor.Practices.RemoveAll(practice => !view.Practices.Contains(practice.Id));

                // add practices
                var practiceIdsToAdd = view.Practices
                    .Where(practiceId => doctor.Practices.All(practice => practice.Id != practiceId))
                    .ToList();
                var practicesToAdd = Db.Practices.Where(practice => practiceIdsToAdd.Contains(practice.Id));
                doctor.Practices.AddRange(practicesToAdd);

                // Daily availability
                // for each removed practice remove daily availability
                foreach (int practiceId in practiceIdsToRemove)
                {
                    var tempPracticeId = practiceId;
                    var dailyAvailabilitiesToRemove =
                        Db.DailyAvailabilities.Where(x => x.DoctorId == doctor.Id && x.PracticeId == tempPracticeId);
                    Db.DailyAvailabilities.RemoveRange(dailyAvailabilitiesToRemove);
                }

                // for each added practice add daily availability
                foreach (int practiceId in practiceIdsToAdd)
                {
                    doctor.SetupWeeklyAvailabilityForPractice(practiceId);
                }


                Db.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = doctor.Id });
            }
            return View("Create", view);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var doctor = Db.Doctors.Find(id);
            doctor.IsDeleted = true;
            Db.SaveChanges();
            return Json(new { Success = true });
        }

    }
}
