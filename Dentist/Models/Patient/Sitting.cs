using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class Sitting
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        [Required]
        public int JobStateId { get; set; }
        public virtual JobState JobState { get; set; }
        [Required]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}