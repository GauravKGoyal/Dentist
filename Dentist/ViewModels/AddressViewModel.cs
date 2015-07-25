using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
    }
}