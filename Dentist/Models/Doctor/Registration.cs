using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Doctor
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string College { get; set; }
    }
}