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
        public ActionResult GetVitalSignBrowserItems([DataSourceRequest] DataSourceRequest request, int patientId)
        {
            if (patientId == 0)
            {
                throw new ArgumentException("Patient id cannot be 0", "patientId");
            }

            var query = ReadContext.Set<VitalSign>().Where(x => x.PatientId == patientId)
                        .ProjectTo<VitalSignViewModel>();
            var result = query.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }     

    }
}