using System.Collections.Generic;
using Dentist.Models.Tags;

namespace Dentist.Models.Doctor
{
    public class CareService : IModelWithId, IModelWithName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
    }
}