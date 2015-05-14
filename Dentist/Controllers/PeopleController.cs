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
    public class PeopleController : BaseController
    {
        // GET: People
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request, int? personRole = null)
        {
            var query = Db.People.Where(x=>x.IsDeleted != true);
            if (personRole.HasValue)
            {
                query = query.Where(x => x.PersonRole == (PersonRole) personRole);
            }
            var projectedQuery = query.ProjectTo<PersonListView>();

            var result = projectedQuery.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllIdTexts(string text = null, int personRole = 0)
        {
            var query = Db.People
                .Where(x => x.PersonRole == (PersonRole) personRole && x.IsDeleted != true)
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
                var people = query.Where(p => p.Text.Contains(text));
                return Json(people, JsonRequestBehavior.AllowGet);
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAppointmentBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = Db.Appointments
                        .Include(x=>x.Patient)
                        .Include(x=>x.Practice)
                        .Where(x => x.DoctorId == personId || x.PatientId == personId)
                        .ProjectTo<AppointmentView>();

            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAppointment([DataSourceRequest] DataSourceRequest request, AppointmentView view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Mapper.Map<Appointment>(view);
                Db.Appointments.Add(appointment);
                Db.SaveChanges();
                // load second person before updating the view
                Db.Appointments
                    .Include(x => x.Patient)
                    .Include(x => x.Practice)
                    .First(x => x.Id == appointment.Id);
                Mapper.Map(appointment, view);
            }

            // Return the inserted product. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateAppointment([DataSourceRequest] DataSourceRequest request, AppointmentView view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Db.Appointments.First(x => x.Id == view.Id);
                Mapper.Map(view, appointment);
                Db.SaveChanges();
                // load second person before updating the view
                Db.Appointments
                    .Include(x => x.Patient)
                    .Include(x => x.Practice)
                    .First(x => x.Id == appointment.Id);
                Mapper.Map(appointment, view);
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteAppointment([DataSourceRequest] DataSourceRequest request, AppointmentView view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Db.Appointments.First(x => x.Id == view.Id);
                Db.Appointments.Remove(appointment);
                Db.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetDailyAvailabilityBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = Db.DailyAvailabilities
                        .Where(x => x.PersonId == personId)
                        .ProjectTo<DailyAvailabilityView>();
            
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Mapper.Map<DailyAvailability>(view);
                Db.DailyAvailabilities.Add(dailyAvailability);
                Db.SaveChanges();
            }

            // Return the inserted product. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Db.DailyAvailabilities.First(x => x.Id == view.Id);
                Mapper.Map(view, dailyAvailability);
                Db.SaveChanges();
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Db.DailyAvailabilities.First(x => x.Id == view.Id);
                Db.DailyAvailabilities.Remove(dailyAvailability);
                Db.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Create()
        {
            var view = new PersonView()
            {
                Address = new AddressView(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonView view)
        {
            if (ModelState.IsValid)
            {
                var person = Mapper.Map<Person>(view);
                Db.People.Add(person);
                
                // add practices
                var practicesToAdd = Db.Practices.Where(practice => view.Practices.Contains(practice.Id));
                person.Practices = new List<Practice>();
                person.Practices.AddRange(practicesToAdd);

                Db.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = person.Id });
            }

            return View(view);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Person person = Db.People
                            .Include(x =>x.Address)
                            .Include(x => x.Practices)
                            .First(x => x.Id == id);
            var personView = Mapper.Map<PersonView>(person);

            return View("Create", personView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonView view)
        {
            if (ModelState.IsValid)
            {
                var person = Db.People
                    .Include(x => x.Practices)
                    .Include(x => x.Address)
                    .First(x => x.Id == view.Id);
                Mapper.Map(view, person);
                
                // remove practices
                person.Practices.RemoveAll(practice => !view.Practices.Contains(practice.Id));

                // add practices
                var practiceIdsToAdd = view.Practices.Where(practiceId => person.Practices.All(practice => practice.Id != practiceId));
                var practicesToAdd = Db.Practices.Where(practice => practiceIdsToAdd.Contains(practice.Id));
                person.Practices.AddRange(practicesToAdd);

                Db.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = person.Id });
            }
            return View("Create", view);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Person person = Db.People.Find(id);
            person.IsDeleted = true;
            Db.SaveChanges();
            return Json(new {Success=true});
        }

    }
}
