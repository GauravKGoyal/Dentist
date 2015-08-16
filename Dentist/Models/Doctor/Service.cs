using System.Collections.Generic;

namespace Dentist.Models.Doctor
{
    public class Service : ModelWithName
    {
        public virtual List<Doctor> Doctors { get; set; }
    }
}