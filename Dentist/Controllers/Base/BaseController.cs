using System.Web.Mvc;
using Dentist.Models;

namespace Dentist.Controllers.Base
{
    public class BaseController : Controller
    {
        private ApplicationDbContext _readContext;
        private WriteContext _writeContext;
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


        public WriteContext WriteContext
        {
            get { return _writeContext ?? (_writeContext = new WriteContext()); }
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