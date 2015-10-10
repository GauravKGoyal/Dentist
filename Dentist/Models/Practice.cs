using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Models.Patient;

namespace Dentist.Models
{
    public class Practice
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string PracticeTagline { get; set; }
        [StringLength(100)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        public virtual List<Patient.Patient> Patients { get; set; }
        public virtual List<Doctor.Doctor> Doctors { get; set; }

        [InverseProperty("Practice")]
        public virtual List<Appointment> PracticeAppointments { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }


        public bool IsDeleted { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
        [StringLength(10)]
        public string Color { get; set; }
    }
}