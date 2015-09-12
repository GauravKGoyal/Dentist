using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class VitalSignViewModel
    {
        [Editable(false)]
        public int Id { get; set; }        
        public int Pulse { get; set; }
        public double Temperature { get; set; }
        public int SystolicBloodPressure { get; set; }
        public int DiastolicBloodPressure { get; set; }
        public double Weight { get; set; }
        public int ResperatoryRate { get; set; }
        [Required]
        public int PatientId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
