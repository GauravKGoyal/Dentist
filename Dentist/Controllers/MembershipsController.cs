using System.Linq;
using System.Web.Mvc;
using Dentist.Models.Doctor;
using Dentist.ViewModels;

namespace Dentist.Controllers
{
    public class MembershipsController : CrudController<Membership>
    {
        public MembershipsController()
            : base("Memberships")
        {}

        public JsonResult GetAllIdTexts()
        {
            var query = ReadContext.Memberships
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
