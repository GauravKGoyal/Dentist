using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentist.Controllers
{
    public class SearchDoctorsController : Controller
    {
        // GET: SearchDoctors
        public ActionResult Index()
        {
            return View();
        }
    }
}