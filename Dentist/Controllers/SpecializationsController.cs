using System.Linq;
using System.Web.Mvc;
using Dentist.Controllers.Base;
using Dentist.Models.Doctor;

namespace Dentist.Controllers
{
    public class SpecializationsController : PageCrudController<Specialization>
    {
        public SpecializationsController()
            : base("Specializations")
        {
        }

        public JsonResult GetAllIdTexts()
        {
            var query = ReadContext.Specializations
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
