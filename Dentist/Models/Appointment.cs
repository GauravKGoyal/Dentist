using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Appointment : IValidatableObject
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int DoctorId { get; set; }

        [InverseProperty("Appointments")]
        public Doctor.Doctor Doctor { get; set; }

        public int PatientId { get; set; }

        [InverseProperty("PatientAppointments")]
        public Paitient Patient { get; set; }

        public int PracticeId { get; set; }

        [InverseProperty("PracticeAppointments")]
        public Practice Practice { get; set; }

        public string Description { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }

        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsBreak { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Doctor == null && DoctorId == 0)
            {
                results.Add(new ValidationResult("Doctor can not empty"));
            }

            if (Patient == null && PatientId == 0)
            {
                results.Add(new ValidationResult("Patient can not empty"));
            }

            if (Practice == null && PracticeId == 0)
            {
                results.Add(new ValidationResult("Practice can not empty"));
            }

            return results;
        }
    }
}