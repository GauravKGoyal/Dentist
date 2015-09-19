using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dentist.Models.Tags;

namespace Dentist.ViewModels
{
    public class VitalSignViewModel : IModelWithId
    {
        [Editable(false)]
        public int Id { get; set; }

        [UIHint("Integer")]
        [Display(Name = "Pulse(BPM)")]
        public int? Pulse { get; set; }

        [UIHint("Number")]
        [Display(Name = "Temperature(°C)")]
        public double? Temperature { get; set; }

        [UIHint("Integer")]
        public int? SystolicBloodPressure { get; set; }

        [UIHint("Integer")]
        public int? DiastolicBloodPressure { get; set; }

        [UIHint("Number")]
        public double? Weight { get; set; }

        [UIHint("Integer")]
        [Display(Name ="Resperatory Rate")]
        public int? ResperatoryRate { get; set; }

        [Display(Name = "Recorded On")]
        public DateTime RecordedDate { get; set; }

        [Display(Name = "Patient")]
        [UIHint("PatientSelector")]
        public int PatientId { get; set; }

        [Display(Name = "Doctor")]
        [UIHint("DoctorSelector")]
        public int DoctorId { get; set; }

        [Display(Name = "Practice")]
        public int PracticeId { get; set; }
        
    }
}
