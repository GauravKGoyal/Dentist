using Dentist.Models.Doctor;
using Dentist.ViewModels;

namespace Dentist.Controllers
{
    public class MembershipsController : BasePersistentModelController<Membership>
    {
        public MembershipsController()
            : base("Memberships")
        {
        }
    }
}
