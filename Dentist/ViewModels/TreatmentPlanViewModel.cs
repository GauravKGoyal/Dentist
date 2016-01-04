using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dentist.Models.Patient;

namespace Dentist.ViewModels
{
    public class TreatmentPlanDto
    {
        public int Id { get; set; }
        public List<TreatmentDto> Treatments { get; set; }

        [Required]
        public DateTime RecordedDate { get; set; }
        public int? PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFullName
        {
            get
            {
                return PatientFirstName + " " + PatientLastName;
            }
        }
    }

    public class TreatmentDto
    {
        public int Id { get; set; }
        [StringLength(100)]
       
        public double Quantity { get; set; }
       
        public int? TreatmentPlanId { get; set; }

        public int ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string ObjectState { get; set; }
    }


}
