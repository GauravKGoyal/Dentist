using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dentist.Controllers.Base;
using Microsoft.Owin.Security.Provider;

namespace Dentist.Controllers
{
    public class NoteTypeController : BaseController
    {
        public JsonResult GetAllIdTexts()
        {
            var query = ReadContext.NoteTypes
                                .Select(x => new
                                {
                                    x.Id,
                                    Text = x.Description,
                                })
                                .OrderBy(x => x.Text);

            var result = query.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}