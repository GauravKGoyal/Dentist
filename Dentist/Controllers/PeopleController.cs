using System.Data.Entity;
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
            var query = ReadContext.Appointments
                        .Include(x=>x.Patient)
                        .Include(x=>x.Practice)
                        .Where(x => x.DoctorId == personId || x.PatientId == personId)
                        .ProjectTo<AppointmentViewModel>();

            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAppointment([DataSourceRequest] DataSourceRequest request, AppointmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var appointment = Mapper.Map<Appointment>(viewModel);
                WriteContext.Appointments.Add(appointment);
                WriteContext.SaveChanges();
                // load second person before updating the viewModel
                WriteContext.Appointments
                    .Include(x => x.Patient)
                    .Include(x => x.Practice)
                    .First(x => x.Id == appointment.Id);
                Mapper.Map(appointment, viewModel);
            }

            // Return the inserted product. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateAppointment([DataSourceRequest] DataSourceRequest request, AppointmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var appointment = ReadContext.Appointments.First(x => x.Id == viewModel.Id);
                Mapper.Map(viewModel, appointment);
                ReadContext.SaveChanges();
                // load second person before updating the viewModel
                ReadContext.Appointments
                    .Include(x => x.Patient)
                    .Include(x => x.Practice)
                    .First(x => x.Id == appointment.Id);
                Mapper.Map(appointment, viewModel);
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteAppointment([DataSourceRequest] DataSourceRequest request, AppointmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var appointment = ReadContext.Appointments.First(x => x.Id == viewModel.Id);
                ReadContext.Appointments.Remove(appointment);
                ReadContext.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetDailyAvailabilityBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = ReadContext.DailyAvailabilities
                        .Include(x=>x.Practice)
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
                ReadContext.DailyAvailabilities.Add(dailyAvailability);
                ReadContext.SaveChanges();
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
                var dailyAvailability = ReadContext.DailyAvailabilities.First(x => x.Id == viewModel.Id);
                Mapper.Map(viewModel, dailyAvailability);
                ReadContext.SaveChanges();
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
                var dailyAvailability = ReadContext.DailyAvailabilities.First(x => x.Id == viewModel.Id);
                ReadContext.DailyAvailabilities.Remove(dailyAvailability);
                ReadContext.SaveChanges();
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

    }
}
