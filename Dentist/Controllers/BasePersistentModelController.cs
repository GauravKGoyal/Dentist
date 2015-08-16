using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class BasePersistentModelController<T> : BaseController where T : ModelWithName
    {
        public string ControllerName { get; set; }

        public BasePersistentModelController(string controllerName) : base()
        {
            ControllerName = controllerName;
        }

        public ActionResult Index()
        {
            ViewBag.ControllerName = ControllerName;
            return View(@"~\Views\BasePersistentModel\Index.cshtml");
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var result = ReadContext.Set<T>().ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.ControllerName = ControllerName;
            var model = (T)Activator.CreateInstance(typeof(T));
            return View(@"~\Views\BasePersistentModel\Create.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T model)
        {
            if (ModelState.IsValid)
            {
                WriteContext.Set<T>().Add(model);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ? 
                        RedirectToAction("Index") : 
                        RedirectToAction("Edit", new { @id = model.Id });
                }
            }

            return View(@"~\Views\BasePersistentModel\Create.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ControllerName = typeof(T).Name;
            var model = ReadContext.Set<T>().Find(id);
            return View(@"~\Views\BasePersistentModel\Create.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T model)
        {
            if (ModelState.IsValid)
            {
                var modelRetrieved = WriteContext.Set<T>().Find(model.Id);
                Mapper.DynamicMap(model, modelRetrieved);
                if (WriteContext.TrySaveChanges(ModelState))
                {
                    return Request.FormSaveAndCloseClicked() ?
                        RedirectToAction("Index") :
                        RedirectToAction("Edit", new { @id = model.Id });
                }
            }

            return View(@"~\Views\BasePersistentModel\Create.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var model = (T)Activator.CreateInstance(typeof(T));
            model.Id = id;
            WriteContext.Set<T>().Attach(model);
            WriteContext.Set<T>().Remove(model);
            WriteContext.TrySaveChanges();
            return Json(new { Success = true });
        }
    }
}