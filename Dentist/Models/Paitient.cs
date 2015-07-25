using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models
{
    public class Paitient : Person
    {
        [InverseProperty("Patient")]
        public virtual List<Appointment> PatientAppointments { get; set; }

        public virtual Practice Practice { get; set; }
        public int PracticeId { get; set; }
    }
}