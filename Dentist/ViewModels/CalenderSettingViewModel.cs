using System;
using System.ComponentModel.DataAnnotations;
using Dentist.Helpers;

namespace Dentist.ViewModels
{
    public class CalenderSettingViewModel
    {
        public int Id { get; set; }

        [UIHint("Time")]
        [DataType(DataType.Time)]
        [Display(Name = "Day Start Time")]
        public DateTime DayStartTime { get; set; }

        [UIHint("Time")]
        [DataType(DataType.Time)]        
        [GreaterThan("DayEndTime", "DayStartTime", "Day end time has to be greater than Day start time")]
        [Display(Name = "Day End Time")]
        public DateTime DayEndTime { get; set; }

        [UIHint("Time")]
        [DataType(DataType.Time)]
        [Display(Name = "Work Week Start Time")]
        public DateTime WorkWeekStartTime { get; set; }

        [UIHint("Time")]
        [DataType(DataType.Time)]
        [Display(Name = "Work Week End Time")]
        [GreaterThan("WorkWeekEndTime", "WorkWeekStartTime", "Week end time has to be greater than Week start time")]
        public DateTime WorkWeekEndTime { get; set; }

        [Display(Name = "Work Week Start Day")]
        public DayOfWeek WorkWeekStartDay { get; set; }

        [Display(Name = "Work Week End Day")]
        public DayOfWeek WorkWeekEndDay { get; set; }

    }
}