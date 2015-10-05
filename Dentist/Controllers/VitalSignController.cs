using System;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dentist.Controllers.Base;
using Dentist.Models;
using Dentist.Models.Patient;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;

namespace Dentist.Controllers
{
    public class VitalSignController : PopupPageCrudController<VitalSign, VitalSignViewModel>
    {
        public ActionResult GetVitalSignBrowserItems([DataSourceRequest] DataSourceRequest request, int? patientId)
        {
            var query = ReadContext.Set<VitalSign>().AsQueryable();

            if (patientId != null)
            {
                query = query.Where(x => x.PatientId == patientId);
            }

            var projectedQuery = query.ProjectTo<VitalSignViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }


    }
}