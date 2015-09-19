using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Models.Patient;
using Dentist.Models.Tags;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers.Base
{
    public class PopupPageCrudController<T, TVm> : BaseController 
        where T : class, IModelWithId
        where TVm : class, IModelWithId
    {
        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = ReadContext.Set<T>().ProjectTo<TVm>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TVm viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<T>(viewModel);
                WriteContext.Set<T>().Add(model);
                WriteContext.TrySaveChanges(ModelState);

                viewModel.Id = model.Id;
            }
            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, TVm viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = WriteContext.Set<T>().Find(viewModel.Id);
                Mapper.Map(viewModel, model);

                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TVm viewModel)
        {
            var model = (T)Activator.CreateInstance(typeof(T));
            model.Id = viewModel.Id;
            WriteContext.Set<T>().Attach(model);
            WriteContext.Set<T>().Remove(model);
            WriteContext.TrySaveChanges(ModelState);

            return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }
    }
}
