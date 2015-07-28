using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dentist.Helpers;

namespace Dentist.Models
{
    public class Doctor : Person
    {
        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; set; }

        public virtual List<Practice> Practices { get; set; }

        public string Color { get; set; }

        public void SetDefaultWeeklyAvailabilityForPractice(int practiceId)
        {                    
            DailyAvailabilities = new List<DailyAvailability>();
            IDailyAvailabilityLoader loader = new DailyAvailabilityDatabaseLoader(DailyAvailabilities, this, practiceId);            
        }
    }
}

