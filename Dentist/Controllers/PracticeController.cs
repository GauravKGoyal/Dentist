using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class PracticeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = Context.Practices.Where(x => x.IsDeleted != true);
            var projectedQuery = query.ProjectTo<PracticeViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllIdTexts()
        {
            var query = Context.Practices.Where(x => x.IsDeleted != true)
                                .Select(x => new
                                {
                                    x.Id,
                                    Text = x.Name,
                                })
                                .OrderBy(x => x.Text);

            var result = query.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPracticesIdTextsByDoctor(int? doctorId)
        {
            if (doctorId == null)
            {
                return null;
            }
            var query = Context.Practices
                .Where(x => x.IsDeleted != true)
                .Where(x => x.Doctors.Any(doctor => doctor.Id == doctorId))
                .Select(x => new
                {
                    x.Id,
                    Text = x.Name,
                    Color = x.Color
                })
                .OrderBy(x => x.Text);


            var result = query.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var view = new PracticeViewModel()
            {
                Address = new AddressView()
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PracticeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var practice = Mapper.Map<Practice>(viewModel);
                Context.Practices.Add(practice);
                Context.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = practice.Id });
            }

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Practice practice = Context.Practices
                            .Include(x => x.Address)
                            .First(x => x.Id == id);
            var practiceView = Mapper.Map<PracticeViewModel>(practice);

            return View("Create", practiceView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PracticeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var practice = Mapper.Map<Practice>(viewModel);
                Context.Entry(practice).State = EntityState.Modified;
                Context.Entry(practice.Address).State = EntityState.Modified;

                Context.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = practice.Id });
            }
            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Practice practice = Context.Practices.Find(id);
            practice.IsDeleted = true;
            Context.SaveChanges();
            return Json(new { Success = true });
        }
    }
 
 
}
