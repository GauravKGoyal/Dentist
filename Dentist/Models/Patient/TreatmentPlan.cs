using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    /// <summary>
    /// Treatment plan can be at system level or patient level
    /// </summary>
    public class TreatmentPlan
    {
        public int Id { get; set; }
        public virtual List<Treatment> Treatments { get; set; }
        
        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}