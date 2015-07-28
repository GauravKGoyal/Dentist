using System;

namespace Dentist.Models
{
    public class DailyAvailabilitySetting
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsWorking { get; set; }
        public DateTime? StartTime1 { get; set; }
        public DateTime? EndTime1 { get; set; }
        public DateTime? StartTime2 { get; set; }
        public DateTime? EndTime2 { get; set; }
    }
}
