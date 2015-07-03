using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dentist.Models;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dentist.Controllers
{
    public class TryController : Controller
    {
        // GET: Try
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult DateTime()
        {
            var obj = new SomeObject();
            obj.DateTimeLocal = System.DateTime.Now;
            obj.DateTimeUtc = System.DateTime.UtcNow;
            obj.DateTimeLocalSerilized = JsonConvert.SerializeObject(obj.DateTimeLocal);
            obj.DateTimeLocalISOSerilized = JsonConvert.SerializeObject(obj.DateTimeLocal, new IsoDateTimeConverter());
            obj.DateTimeLocalJavaScriptSerilized = JsonConvert.SerializeObject(obj.DateTimeLocal, new JavaScriptDateTimeConverter());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }

    public class SomeObject
    {
        public DateTime DateTimeLocal { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public string DateTimeLocalSerilized { get; set; }
        public string DateTimeLocalISOSerilized { get; set; }
        public string DateTimeLocalJavaScriptSerilized { get; set; }
    }


}