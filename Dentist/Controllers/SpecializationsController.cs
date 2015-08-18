using Dentist.Models.Doctor;

namespace Dentist.Controllers
{
    public class SpecializationsController : CrudController<Specialization>
    {
        public SpecializationsController()
            : base("Specializations")
        {
        }
    }
}
