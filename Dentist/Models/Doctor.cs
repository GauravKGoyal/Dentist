using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentist.Models
{
    public class Doctor : Person
    {
        public virtual List<DailyAvailability> DailyAvailabilities { get; set; }

        [InverseProperty("Doctor")]
        public virtual List<Appointment> Appointments { get; set; }

        public virtual List<Practice> Practices { get; set; }

        public string Color { get; set; }

        public void SetupWeeklyAvailabilityForPractice(int practiceId)
        {
            //todo get start time and end time from practice availability timings
            var startTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0, 0);
            var endTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0, 0);
            var startTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 30, 0, 0);
            var endTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0, 0);

            if (DailyAvailabilities == null)
            {
                DailyAvailabilities = new List<DailyAvailability>();
            }

            Array daysOfWeek = Enum.GetValues(typeof (DayOfWeek));
            foreach (object dayOfWeek in daysOfWeek)
            {
                DailyAvailabilities.Add(new DailyAvailability
                {
                    DayOfWeek = (DayOfWeek) dayOfWeek,
                    IsWorking = true,
                    StartTime1 = startTime1,
                    EndTime1 = endTime1,
                    StartTime2 = startTime2,
                    EndTime2 = endTime2,
                    Doctor = this,
                    PracticeId = practiceId
                });
            }
        }
    }
}