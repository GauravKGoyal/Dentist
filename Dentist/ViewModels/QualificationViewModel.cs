using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class QualificationViewModel
    {
        [Editable(false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Qualification")]
        public string Name { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public string College { get; set; }
        [Required]
        [UIHint("Year")]
        [Display(Name = "Pass out year")]
        public int Year { get; set; }
    }
}
