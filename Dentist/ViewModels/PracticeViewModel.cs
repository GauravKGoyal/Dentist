using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class PracticeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name="Practice Tag Line")]
        public string PracticeTagline { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int AddressId { get; set; }
        public AddressViewModel Address { get; set; }
        public string Color { get; set; }
    }
}