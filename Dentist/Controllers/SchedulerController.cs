using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    public class SchedulerController : BaseController
    {
        // GET: People
        public ActionResult Index()
        {
            var doctors = Db.People
               .Include(x => x.Practices)
               .Where(x => x.IsDeleted != true && x.PersonRole == PersonRole.Doctor)
               .OrderBy(x => x.FirstName)
               .ToList();

            UpdateAvailableDoctorAndPracticeResources("", "");
            ViewBag.DoctorsInTreeView = MapDoctorsToTreeViewItems(doctors);
            return View();
        }

        private IEnumerable<TreeViewItemModel> MapDoctorsToTreeViewItems(IEnumerable<Person> doctors)
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
   
        private void UpdateAvailableDoctorAndPracticeResources(string doctorIds, string practiceIds)
        {
            var practices = Db.Practices.Where(x => x.IsDeleted != true).ToList();
            var practiceIdList = practiceIds.ToInts();
            if (practiceIdList.Any())
            {
                practices = practices.Where(x => practiceIdList.Contains(x.Id)).ToList();
            }

            var doctors = Db.People.Where(x => x.PersonRole == PersonRole.Doctor && x.IsDeleted != true).ToList();
            var doctorIdList = doctorIds.ToInts();
            if (doctorIdList.Any())
            {
                doctors = doctors.Where(x => doctorIdList.Contains(x.Id)).ToList();
            }

            ViewBag.Doctors = Mapper.Map<List<SchedulerDoctorView>>(doctors);
            ViewBag.Practices = Mapper.Map<List<SchedulerPracticeView>>(practices);
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request, string doctorsIds = null, string practiceIds = null)
        {
            var query = Db.Appointments
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

            var queryable = query.ProjectTo<SchedulerAppointmentView>();
            var result = queryable.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentView view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Mapper.Map<Appointment>(view);
                var isNewPatientFortheAppointment = appointment.PatientId == 0;
                if (isNewPatientFortheAppointment)
                {
                    var patient = new Person()
                    {
                        FirstName = view.FirstName,
                        LastName = view.LastName,
                        Phone = view.Phone,
                        Address = new Address()
                    };
                    //if (appointment.IsBreak)
                    //{
                    //    patient.PersonRole = PersonRole.Break;
                    //}
                    appointment.Patient = patient;
                }
                else
                {
                    var patient = Db.People.Find(appointment.PatientId);
                    patient.FirstName = view.FirstName;
                    patient.LastName = view.LastName;
                    patient.Phone = view.Phone;
                }
                // load practice to update the practice color
                appointment.Practice = Db.Practices.Find(appointment.PracticeId);
                
                Db.Appointments.Add(appointment);
                Db.SaveChanges();
                Mapper.Map(appointment, view);
            }

            // Return the inserted item. The grid needs the generated id. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult UpdateAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentView view)
        {
            if (ModelState.IsValid)
            {
                var appointment = Db.Appointments.First(x => x.Id == view.Id);
                Mapper.Map(view, appointment);
                // do not load patient before mapping view to the appointment 
                // because during update process view patient may have been replaced 
                // with new patient therefore mapping view to the appointment may have updated the appointment's paitient link
                Db.People.Find(appointment.PatientId);
                appointment.Patient.FirstName = view.FirstName;
                appointment.Patient.LastName = view.LastName;
                appointment.Patient.Phone = view.Phone;
                Db.SaveChanges();
                // load practice to update the practice color
                appointment.Practice = Db.Practices.Find(appointment.PracticeId);
                Mapper.Map(appointment, view);
            }

            // Return the updated item. Also return any validation errors.
            return Json(new[] { view }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteAppointment([DataSourceRequest] DataSourceRequest request, SchedulerAppointmentView view)
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

    }
}
