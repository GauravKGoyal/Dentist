using System.Linq;
using System.Web.Mvc;
using Dentist.Models.Doctor;

namespace Dentist.Controllers
{
    public class ServicesController : BasePersistentModelController<Service>
    {
        public ServicesController()
            : base("Services")
        {
        }


        public JsonResult GetAllIdTexts()
        {
            var query = ReadContext.Services
                                .Select(x => new
                                {
                                    x.Id,
                                    Text = x.Name,
                                })
                                .OrderBy(x => x.Text);

            var result = query.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
