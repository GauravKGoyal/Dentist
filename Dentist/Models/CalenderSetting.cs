using System;

namespace Dentist.Models
{
    public class CalenderSetting
    {
        public int Id { get; set; }
        public DateTime DayStartTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public DateTime WorkWeekStartTime { get; set; }
        public DateTime WorkWeekEndTime { get; set; }
        public DayOfWeek WorkWeekStartDay { get; set; }
        public DayOfWeek WorkWeekEndDay { get; set; }

    }
}