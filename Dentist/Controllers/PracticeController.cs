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
            var query = Db.Practices.Where(x => x.IsDeleted != true);
            var projectedQuery = query.ProjectTo<PracticeView>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllIdTexts()
        {
            var query = Db.Practices.Where(x => x.IsDeleted != true)
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
            var query = Db.Practices
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
            var view = new PracticeView()
            {
                Address = new AddressView()
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PracticeView view)
        {
            if (ModelState.IsValid)
            {
                var practice = Mapper.Map<Practice>(view);
                Db.Practices.Add(practice);
                Db.SaveChanges();
                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = practice.Id });
            }

            return View(view);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Practice practice = Db.Practices
                            .Include(x => x.Address)
                            .First(x => x.Id == id);
            var practiceView = Mapper.Map<PracticeView>(practice);

            return View("Create", practiceView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PracticeView view)
        {
            if (ModelState.IsValid)
            {
                var practice = Mapper.Map<Practice>(view);
                Db.Entry(practice).State = EntityState.Modified;
                Db.Entry(practice.Address).State = EntityState.Modified;

                Db.SaveChanges();

                if (Request.Form["btnSubmit"] == "Save and Close")
                    return RedirectToAction("Index");
                return RedirectToAction("Edit", new { @id = practice.Id });
            }
            return View("Create", view);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Practice practice = Db.Practices.Find(id);
            practice.IsDeleted = true;
            Db.SaveChanges();
            return Json(new { Success = true });
        }
    }
 
 
}
