using System.Web.Mvc;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext Db = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}