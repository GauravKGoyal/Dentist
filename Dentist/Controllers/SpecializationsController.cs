using Dentist.Models.Doctor;

namespace Dentist.Controllers
{
    public class SpecializationsController : BasePersistentModelController<Specialization>
    {
        public SpecializationsController()
            : base("Specializations")
        {
        }
    }
}
