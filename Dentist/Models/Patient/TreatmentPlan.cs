using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class TreatmentPlan
    {
        public int Id { get; set; }
        public virtual List<Job> Jobs { get; set; }
        [Required]
        public int PatientId { get; set; }
        public virtual Paitient Patient { get; set; }
    }
}