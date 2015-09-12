using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class JobState
    {
        public JobState()
        {
            Sittings = new List<Sitting>();
        }
        public int Id { get; set; }
        [Required]
        public string Descrition { get; set; }

        public virtual List<Sitting> Sittings { get; private set; }
    }
}