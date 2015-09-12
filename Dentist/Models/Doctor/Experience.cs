using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Doctor
{
    public class Experience
    {
        public int Id { get; set; }
        [Required]
        public int FromYear { get; set; }
        [Required]
        public int ToYear { get; set; }
        [Required]
        [StringLength(100)]
        public string As { get; set; }
        [Required]
        [StringLength(100)]
        public string At { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}