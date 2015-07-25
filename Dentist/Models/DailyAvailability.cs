using System;

namespace Dentist.Models
{
    public class DailyAvailability
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int PracticeId { get; set; }
        public virtual Practice Practice { get; set; }

        public bool IsWorking { get; set; }
        public DateTime? StartTime1 { get; set; }
        public DateTime? EndTime1 { get; set; }
        public DateTime? StartTime2 { get; set; }
        public DateTime? EndTime2 { get; set; }
        public DateTime? StartTime3 { get; set; }
        public DateTime? EndTime3 { get; set; }
    }
}