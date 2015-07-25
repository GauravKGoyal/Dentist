using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Enums;

namespace Dentist.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int AddressId { get; set; }

        public AddressViewModel Address { get; set; }
        public bool IsDeleted { get; set; }
    }
}