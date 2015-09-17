using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class PersonListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int? AvatarId { get; set; }
    }
}