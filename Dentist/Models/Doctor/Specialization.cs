using System.Collections.Generic;
using Dentist.Models.Tags;

namespace Dentist.Models.Doctor
{
    public class Specialization : IModelWithId, IModelWithName
    {
        public virtual List<Doctor> Doctors { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}