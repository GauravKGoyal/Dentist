using System.Linq;
using System.Web.Mvc;
using Dentist.Controllers.Base;
using Dentist.Models.Doctor;
using Dentist.ViewModels;
using Dentist.Models;

namespace Dentist.Controllers
{
    public class ProceduresController : PageCrudController<Procedure>
    {
        public ProceduresController()
            : base("Procedures")
        {}

       
    }
}
