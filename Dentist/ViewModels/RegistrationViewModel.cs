using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentist.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Registration Number")]
        public string Number { get; set; }
        [Required]
        [Display(Name = "Registration College")]
        public string College { get; set; }
    }
}
