using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Dentist.Controllers.Base;
using Dentist.Models.Patient;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Dentist.Controllers
{
    public class PatientNoteController : BaseController
    {
        // GET: PatientNote
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPatientNotesBrowserItems([DataSourceRequest] DataSourceRequest request, int? patientId)
        {
            var query = ReadContext.Set<PatientNote>().AsQueryable();

            if (patientId != null)
            {
                query = query.Where(x => x.PatientId == patientId);
            }

            var projectedQuery = query.ProjectTo<PatientNoteViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
      
    }
}