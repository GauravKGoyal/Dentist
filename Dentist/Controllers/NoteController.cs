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
    public class NoteController : PopupPageCrudController<Note, NoteViewModel>
    {
        // GET: PatientNotes
        public ActionResult Index()
        {
            return View();
        }      

        public ActionResult GetNotesBrowserItems([DataSourceRequest] DataSourceRequest request, int? patientNoteId)
        {
            var query = ReadContext.Set<Note>().AsQueryable();

            if (patientNoteId != null)
            {
                query = query.Where(x => x.PatientNoteId == patientNoteId);
            }

            var projectedQuery = query.ProjectTo<NoteViewModel>();
            var result = projectedQuery.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}