using System;
using System.Web.Mvc;
using AutoMapper;
using Dentist.Helpers;
using Dentist.Models.Tags;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers.Base
{
    // IModelWithName - Name property is required by the grid
    // IModelWithId - Id property is required by the grid and for Deleting a record
    // Note this controller does not consider view model approach
    public class PageCrudController<T> : BaseController where T : class, IModelWithId, IModelWithName
    {
        public string ControllerName { get; set; }

        protected PageCrudController(string controllerName) : base()
        {
            ControllerName = controllerName;
        }

        public ActionResult Index()
        {
            ViewBag.ControllerName = ControllerName;
            return View(@"~\Views\Crud\Index.cshtml");
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
            return View(@"~\Views\Crud\Create.cshtml", model);
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

            return View(@"~\Views\Crud\Create.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ControllerName = ControllerName;
            var model = ReadContext.Set<T>().Find(id);
            return View(@"~\Views\Crud\Create.cshtml", model);
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

            return View(@"~\Views\Crud\Create.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var model = (T)Activator.CreateInstance(typeof(T));
            model.Id = id;
            WriteContext.Set<T>().Attach(model);
            WriteContext.Set<T>().Remove(model);
            var errorMessage = "";
            var changesSaved = WriteContext.TrySaveChanges(out errorMessage);
            return Json(new { Success = changesSaved, ErrorMessage = errorMessage });
        }
    }
}