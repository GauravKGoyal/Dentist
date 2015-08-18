using Dentist.Models.Doctor;
using Dentist.ViewModels;

namespace Dentist.Controllers
{
    public class MembershipsController : CrudController<Membership>
    {
        public MembershipsController()
            : base("Memberships")
        {
        }
    }
}
