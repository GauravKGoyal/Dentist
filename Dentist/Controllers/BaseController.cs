using System.Web.Mvc;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext Context = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}