using Dentist.Models.Tags;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        public double Quantity { get; set; }
        
        public int? TreatmentPlanId { get; set; }

        public TreatmentPlan TreatmentPlan { get; set; }

        public int ProcedureId { get; set; }

        public virtual Procedure Procedure { get; set; }
    }
}