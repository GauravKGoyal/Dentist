using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Enums;

namespace Dentist.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int DoctorId { get; set; }

        [InverseProperty("Appointments")]
        public Doctor Doctor { get; set; }

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
    }
}