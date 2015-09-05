using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class DailyAvailabilitySettingViewModel
    {
        private DateTime? _startTime1;
        private DateTime? _endTime1;
        private DateTime? _startTime2;
        private DateTime? _endTime2;
      
        //[ReadOnly(true)] doesnot bring the values to view model treats it as a readonly property
        [Editable(false)]
        public int Id { get; set; }

        [Display(Name = "Day")]
        public DayOfWeek DayOfWeek { get; set; }
      

        [Display(Name = "Start1")]
        [UIHint("Time")]
        public DateTime? StartTime1
        {
            get
            {
                return IsWorking ? _startTime1 : null;
            }
            set
            {
                _startTime1 = value;                
            }
        }

        [Display(Name = "End1")]
        [UIHint("Time")]
        public DateTime? EndTime1
        {
            get
            {
                return IsWorking ? _endTime1 : null;
            }
            set
            {
                _endTime1 = value;
            }
        }

        [Display(Name = "Start2")]
        [UIHint("Time")]
        public DateTime? StartTime2
        {
            get
            {
                return IsWorking ? _startTime2 : null;
            }
            set
            {
                _startTime2 = value;
            }
        }

        [Display(Name = "End2")]
        [UIHint("Time")]
        public DateTime? EndTime2
        {
            get
            {
                return IsWorking ? _endTime2 : null;
            }
            set
            {
                _endTime2 = value;
            }
        }

    
        public bool IsWorking { get; set; }
    }
}