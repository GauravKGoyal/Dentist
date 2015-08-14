using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    public class GridController : BaseController
    {
        // GET: Grid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrowserItems([DataSourceRequest] DataSourceRequest request)
        {
            var query = ReadContext.Practices.Where(x => x.IsDeleted != true);
            var projectedQuery = query.ProjectTo<PracticeViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}