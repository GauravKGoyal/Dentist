using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class JobState
    {
        public JobState()
        {
        }
        public int Id { get; set; }
        [Required]
        public string Descrition { get; set; }

    }
}