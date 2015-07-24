﻿using System.Data.Entity;
using System.Linq;
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
    public class PeopleController : BaseController
    {
        public ActionResult GetAppointmentBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = Context.Appointments
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
                Context.Appointments.Add(appointment);
                Context.SaveChanges();
                // load second person before updating the view
                Context.Appointments
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
                var appointment = Context.Appointments.First(x => x.Id == view.Id);
                Mapper.Map(view, appointment);
                Context.SaveChanges();
                // load second person before updating the view
                Context.Appointments
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
                var appointment = Context.Appointments.First(x => x.Id == view.Id);
                Context.Appointments.Remove(appointment);
                Context.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetDailyAvailabilityBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = Context.DailyAvailabilities
                        .Include(x=>x.Practice)
                        .Where(x => x.DoctorId == personId)
                        .ProjectTo<DailyAvailabilityView>();
            
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Mapper.Map<DailyAvailability>(view);
                Context.DailyAvailabilities.Add(dailyAvailability);
                Context.SaveChanges();
                // load practice to load practice name
                var practice = dailyAvailability.Practice;
            }

            // Return the inserted product. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Context.DailyAvailabilities.First(x => x.Id == view.Id);
                Mapper.Map(view, dailyAvailability);
                Context.SaveChanges();
                // load practice to load practice name
                var practice = dailyAvailability.Practice;
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteDailyAvailability([DataSourceRequest] DataSourceRequest request, DailyAvailabilityView view)
        {
            if (ModelState.IsValid)
            {
                var dailyAvailability = Context.DailyAvailabilities.First(x => x.Id == view.Id);
                Context.DailyAvailabilities.Remove(dailyAvailability);
                Context.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

    }
}
