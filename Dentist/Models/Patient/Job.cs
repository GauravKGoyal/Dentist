using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class Job
    {
        public Job()
        {
            Sittings = new List<Sitting>();
        }
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        public int TreatmentPlanId { get; set; }
        public TreatmentPlan TreatmentPlan { get; set; }

        public virtual List<Sitting> Sittings { get; private set; }
    }
}