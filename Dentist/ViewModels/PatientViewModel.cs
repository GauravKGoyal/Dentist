using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class PatientViewModel : PersonViewModel
    {
        [Display(Name="Practice")]
        public int PatientViewPracticeId { get; set; }

        public bool IsDoctor
        {
            get { return false; }
        }

    }
}