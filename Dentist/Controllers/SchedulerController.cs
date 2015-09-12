using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.Models.Patient;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    [Authorize]
    public class SchedulerController : BaseController
    {
        // GET: People
        public ActionResult Index()
        {
            var doctors = ReadContext.Doctors
               .Include(x=>x.Practices)
               .Where(x => x.IsDeleted != true && x.PersonRole == PersonRole.Doctor)
               .OrderBy(x => x.FirstName)
               .ToList();
            var practices = ReadContext.Practices.Where(x => x.IsDeleted != true).ToList();
            var dailyAvailabilities = ReadContext.DailyAvailabilities.ToList();
            var calenderSetting = ReadContext.CalenderSettings.First();

            ViewBag.Doctors = Mapper.Map<List<SchedulerDoctorViewModel>>(doctors);
            ViewBag.Practices = Mapper.Map<List<SchedulerPracticeViewModel>>(practices);
            ViewBag.DoctorsInTreeView = MapDoctorsToTreeViewItems(doctors);
            ViewBag.CalenderSettings = Mapper.Map<CalenderSettingViewModel>(calenderSetting);
            ViewBag.DailyAvailabilityList = Mapper.Map<List<DailyAvailabilityViewModel>>(dailyAvailabilities);
            return View();
        }

        private IEnumerable<TreeViewItemModel> MapDoctorsToTreeViewItems(IEnumerable<Doctor> doctors)
        {
            var treeItems = new List<TreeViewItemModel>();
            foreach (var doctor in doctors)
            {
                var practiceItems = doctor.Practices.Select(practice =>
                    new TreeViewItemModel()
                    {
                        Id = practice.Id.ToString(),
                        Text = practice.Name,
                        LinkHtmlAttributes = new Dictionary<string, string>
                        {
                            {"data-type", "practice"}, 
                            {"data-color",practice.Color},
                            {"class","dentistTreeItem"}
                        }
                    }).ToList();

                var doctorItem = new TreeViewItemModel()
                {
                    Id = doctor.Id.ToString(),
                    Text = doctor.FirstName + " " + doctor.LastName,
                    Items = practiceItems,
                    LinkHtmlAttributes = new Dictionary<string, string>
                    {
                        { "data-type", "doctor" }, 
                        { "data-color", doctor.Color },
                        {"class","dentistTreeItem"}
                    }
                };

                treeItems.Add(doctorItem);
            }
            return treeItems;
        }
   
        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request, string doctorsIds = null, string practiceIds = null)
        {
            var query = ReadContext.Appointments
                        .Include(x => x.Patient)
                        .Include(x => x.Practice)
                        .Where(x => x.Patient.IsDeleted != true);

            var doctorIdList = doctorsIds.ToInts();
            if (doctorIdList.Any())
            {
                query = query.Where(x => doctorIdList.Contains(x.DoctorId));
            }

            var practiceIdList = practiceIds.ToInts();
            if (practiceIdList.Any())
            {
                query = query.Where(x => practiceIdList.Contains(x.PracticeId));
            }

            var queryable = query.ProjectTo<SchedulerAppointmentViewModel>();
            var result = queryable.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentViewModel view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Mapper.Map<Appointment>(view);
                var practice = WriteContext.Practices.Find(appointment.PracticeId);
                var isNewPatientFortheAppointment = appointment.PatientId == 0;
                if (isNewPatientFortheAppointment)
                {
                    var patient = new Paitient()
                    {
                        FirstName = view.FirstName,
                        LastName = view.LastName,
                        Phone = view.Phone,
                        Practice = practice 
                    };                    
                    appointment.Patient = patient;
                }
                else
                {
                    var patient = WriteContext.Paitients.Find(appointment.PatientId);
                    patient.FirstName = view.FirstName;
                    patient.LastName = view.LastName;
                    patient.Phone = view.Phone;
                }

                WriteContext.Appointments.Add(appointment);
                WriteContext.TrySaveChanges(ModelState);
                Mapper.Map(appointment, view);
            }

            // Return the inserted item. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentViewModel view)
        {
            if (ModelState.IsValid)
            {
                var appointment = WriteContext.Appointments.First(x => x.Id == view.Id);
                Mapper.Map(view, appointment);
                // do not load patient before mapping view to the appointment 
                // because during update process view patient may have been replaced 
                // with new patient therefore mapping view to the appointment may have updated the appointment's paitient link
                WriteContext.Paitients.Find(appointment.PatientId);
                appointment.Patient.FirstName = view.FirstName;
                appointment.Patient.LastName = view.LastName;
                appointment.Patient.Phone = view.Phone;
                WriteContext.TrySaveChanges(ModelState);
                // load practice to update the practice color
                appointment.Practice = WriteContext.Practices.Find(appointment.PracticeId);
                Mapper.Map(appointment, view);
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentViewModel view)
        {
            if (ModelState.IsValid)
            {
                var appointment = WriteContext.Appointments.First(x => x.Id == view.Id);
                WriteContext.Appointments.Remove(appointment);
                WriteContext.TrySaveChanges(ModelState);
            }

            // Return the removed item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

    }
}
