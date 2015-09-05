using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentist.ViewModels
{
    public class AwardViewModel
    {
        [Editable(false)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [UIHint("Year")]
        public int Year { get; set; }
        [Required]
        public int DoctorId { get; set; }
    }
}
