using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Models.Tags;

namespace Dentist.Models.Doctor
{
    public class Specialization : IModelWithId, IModelWithName
    {
        public virtual List<Doctor> Doctors { get; set; }
        public int Id { get; set; }
        
        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Name { get; set; }
    }
}