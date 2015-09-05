using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class ExperienceViewModel
    {
        [Editable(false)]
        public int Id { get; set; }
        [UIHint("Year")]
        [Display(Name = "From Year")]
        [Required]
        public int FromYear { get; set; }
        [UIHint("Year")]
        [Display(Name = "To Year")]
        [Required]
        public int ToYear { get; set; }
        [Required]
        public string As { get; set; }
        [Required]
        public string At { get; set; }
        public int DoctorId { get; set; }
    }
}
