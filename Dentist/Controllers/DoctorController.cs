﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Enums;
using Dentist.Helpers;
using Dentist.Models;
using Dentist.Models.Doctor;
using Dentist.Services;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    [Authorize]
    public class DoctorController : BaseController
    {
        public JsonResult GetAllIdTexts(string text = null)
        {
            var query = ReadContext.Doctors.Where(x => x.IsDeleted != true)
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
            var context = ReadContext;
            var query = context.Doctors.Where(x => x.IsDeleted != true);
            query = query.Where(x => x.PersonRole == PersonRole.Doctor);
            var projectedQuery = query.ProjectTo<DoctorListViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new DoctorViewModel()
            {
                Address = new AddressViewModel(),
                DateOfBirth = DateTime.Today.AddYears(-50)
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = new Doctor();
                doctor.Context = WriteContext;
                viewModel.CopyTo(doctor);
                WriteContext.Doctors.Add(doctor);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ?  RedirectToAction("Index") :  RedirectToAction("Edit", new { @id = doctor.Id });
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

            var doctor = ReadContext.Doctors
                            .Include(x => x.Address)
                            .Include(x=> x.Practices)
                            .Include(x=> x.Services)
                            .Include(x=> x.Memberships)
                            .Include(x=> x.Files)
                            .First(x => x.Id == id);
            if (doctor.PersonRole != PersonRole.Doctor)
            {
                throw new Exception(string.Format("Person with id {0} is not a doctor", id));
            }
            var doctorView = new DoctorViewModel();
            doctorView.CopyFrom(doctor);

            return View("Create", doctorView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var doctor = WriteContext.Doctors.Find(viewModel.Id);
                doctor.Context = WriteContext;
                viewModel.CopyTo(doctor);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? RedirectToAction("Index") : RedirectToAction("Edit", new { @id = doctor.Id });
                }
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var errorMessage = "";
            Doctor doctor = WriteContext
                .Doctors
                .Include(x => x.Practices)
                .First(x => x.Id == id);
            doctor.Context = WriteContext;
            doctor.IsDeleted = true;
            var changesSaved = WriteContext.TrySaveChanges(out errorMessage);            
            return Json(new { Success = changesSaved, ErrorMessage = errorMessage });
        }

        //private IDoctorService _doctorService;
        //public IDoctorService DoctorService
        //{
        //    get 
        //    {
        //        if (_doctorService == null)
        //        {
        //            _doctorService = DependencyInjectionConfig.Container.Resolve<IDoctorService>();
        //        }
        //        return _doctorService;
        //    } 
        //}

    }
}
