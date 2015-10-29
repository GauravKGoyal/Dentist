using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.Models.Patient
{
    public class VitalSign : IModelWithId
    {
        public int Id { get; set; }        
        public int? Pulse { get; set; }
        public double? Temperature { get; set; }
        public int? SystolicBloodPressure { get; set; }
        public int? DiastolicBloodPressure { get; set; }
        public double? Weight { get; set; }
        public int? ResperatoryRate { get; set; }
        public DateTime RecordedDate { get; set; }

        // Recorded for
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        // Recorded By
        public int DoctorId { get; set; }
        public virtual Models.Doctor.Doctor Doctor { get; set; }

        // Recorded at
        public int PracticeId { get; set; }
        public virtual Practice Practice { get; set; }

    }
}
