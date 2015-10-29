using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class BaseApiController : ApiController
    {
        private ApplicationDbContext _readContext;
        private WriteContext _writeContext;
        public BaseApiController()
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
