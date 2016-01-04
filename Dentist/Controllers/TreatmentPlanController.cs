using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Dentist.Controllers.Base;

namespace Dentist.Controllers
{
    public class TreatmentPlanController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
     }
}