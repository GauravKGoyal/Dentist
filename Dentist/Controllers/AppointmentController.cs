using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Controllers.Base;
using Dentist.Enums;
using Dentist.Models;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    [Authorize]
    public class AppointmentController : BaseController
    {
        public ActionResult GetAppointmentBrowserItems([DataSourceRequest] DataSourceRequest request, int personId)
        {
            var query = ReadContext.Appointments
                        .Include(x => x.Patient)
                        .Include(x => x.Practice)
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
                WriteContext.TrySaveChanges(ModelState);
                
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
                var appointment = WriteContext.Appointments.First(x => x.Id == viewModel.Id);
                Mapper.Map(viewModel, appointment);
                WriteContext.TrySaveChanges(ModelState);
                // load second person before updating the viewModel
                WriteContext.Appointments
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
                var model = new Appointment() {Id = viewModel.Id};
                WriteContext.Appointments.Attach(model);
                WriteContext.Appointments.Remove(model);
                WriteContext.TrySaveChanges(ModelState);
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

    }
}
