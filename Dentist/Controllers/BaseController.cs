using System.Web.Mvc;
using Autofac;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationDbContext _readContext;
        private ApplicationDbContext _writeContext;
        public BaseController()
        {
        }

        public ApplicationDbContext ReadContext
        {
            get
            {
                if (_readContext == null)
                {
                    _readContext = new ApplicationDbContext();
                    _readContext.Configuration.ProxyCreationEnabled = false;
                    _readContext.Configuration.LazyLoadingEnabled = false;
                }
                return _readContext;
            } 
        }


        public ApplicationDbContext WriteContext
        {
            get
            {
                if (_writeContext == null)
                {
                    _writeContext = new ApplicationDbContext();
                    _writeContext.Configuration.ProxyCreationEnabled = true;
                    _writeContext.Configuration.LazyLoadingEnabled = true;
                }
                return _writeContext;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_readContext != null)
                    _readContext.Dispose();
                if (_writeContext != null)
                    _writeContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}