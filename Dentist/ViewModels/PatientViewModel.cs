using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.ViewModels
{
    public class PatientViewModel : PersonViewModel,IModelWithId
    {
        [Display(Name="Practice")]
        public int PatientViewPracticeId { get; set; }

        public bool IsDoctor
        {
            get { return false; }
        }

    }
}