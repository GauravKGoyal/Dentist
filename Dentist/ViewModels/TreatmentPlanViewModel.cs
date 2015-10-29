using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dentist.Models.Patient;

namespace Dentist.ViewModels
{
    public class TreatmentPlanViewModel
    {
            public int Id { get; set; }
            public virtual List<TreatmentViewModel> Treatments { get; set; }

            public int? PatientId { get; set; }
            public virtual Patient Patient { get; set; }
    }

    public class TreatmentViewModel
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }

        public int? TreatmentPlanId { get; set; }
        public TreatmentPlan TreatmentPlan { get; set; }        
    }

}
