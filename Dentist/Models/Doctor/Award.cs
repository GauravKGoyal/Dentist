using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Doctor
{
    public class Award
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public virtual Models.Doctor.Doctor Doctor { get; set; }
    }
}