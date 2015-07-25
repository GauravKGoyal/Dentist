using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models
{
    public class Practice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PracticeTagline { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public virtual List<Paitient> Paitients { get; set; }
        public virtual List<Doctor> Doctors { get; set; }

        [InverseProperty("Practice")]
        public virtual List<Appointment> PracticeAppointments { get; set; }

        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }
        public bool IsDeleted { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string Color { get; set; }
    }
}