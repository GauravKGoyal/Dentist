using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Dentist.Models.Tags;

namespace Dentist.Models.Doctor
{
    public class CareService : IModelWithId, IModelWithName
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        public string Name { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
    }
}