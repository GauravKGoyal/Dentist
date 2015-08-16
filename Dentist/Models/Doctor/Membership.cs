using System.Collections.Generic;

namespace Dentist.Models.Doctor
{
    public class Membership : ModelWithName
    {
        public virtual List<Doctor> Doctors { get; set; }
    }
}