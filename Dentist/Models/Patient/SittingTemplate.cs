using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class SittingTemplate
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public JobState JobState { get; set; }
        [Required]
        public int JobTemplateId { get; set; }
        public virtual JobTemplate JobTemplate { get; set; }
    }
}