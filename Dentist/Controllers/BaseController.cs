using System.Web.Mvc;
using Autofac;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext Context;
        public BaseController()
        {
            Context = DependencyResolver.Current.GetService<ApplicationDbContext>();
        }
    }
}