using System;
using System.ComponentModel.DataAnnotations;

namespace Dentist.Models.Patient
{
    public class VitalSign
    {
        public int Id { get; set; }        
        public int Pulse { get; set; }
        public double Temperature { get; set; }
        public int SystolicBloodPressure { get; set; }
        public int DiastolicBloodPressure { get; set; }
        public double Weight { get; set; }
        public int ResperatoryRate { get; set; }
        [Required]
        public int PatientId { get; set; }
        public virtual Paitient Patient { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
