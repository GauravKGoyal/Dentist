using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models.Doctor
{
    public class Registration
    {
        public Registration()
        {
            Doctors = new List<Doctor>();
        }
        public int Id { get; set; }
       
        public virtual ICollection<Doctor> Doctors { get; set; }

        [Required]
        public string Number { get; set; }
        [Required]
        public string College { get; set; }
    }
}