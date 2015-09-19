using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    /// <summary>
    /// Treatment can have many sub Treatments
    /// A Treatment could be used as a template or can be assigned against treatment plan
    /// </summary>
    public class Treatment
    {
        public Treatment()
        {
        }
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        
        public int? TreatmentPlanId { get; set; }
        public TreatmentPlan TreatmentPlan { get; set; }

        public int? ParentTreatmentId { get; set; }
        public virtual Treatment ParentTreatment { get; set; }

        public virtual ICollection<Treatment> ChildrenTreatments { get; set; }
    }
}