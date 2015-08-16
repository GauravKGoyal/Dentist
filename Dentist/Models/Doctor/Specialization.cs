using System.Collections.Generic;

namespace Dentist.Models.Doctor
{
    public class Specialization : ModelWithName
    {
        public virtual List<Doctor> Doctors { get; set; }
    }
}