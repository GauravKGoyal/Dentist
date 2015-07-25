using System;
using System.ComponentModel.DataAnnotations;

namespace Dentist.ViewModels
{
    public class DailyAvailabilityViewModel
    {
        private DateTime? _startTime1;
        private DateTime? _endTime1;
        private DateTime? _startTime2;
        private DateTime? _endTime2;
        private DateTime? _startTime3;
        private DateTime? _endTime3;

        public int Id { get; set; }
        [Display(Name = "Day")]
        public DayOfWeek DayOfWeek { get; set; }
        [Display(Name = "Person")]
        public int DailyAvailabilityViewModelPersonId { get; set; }
        [Display(Name = "Practice")]
        public int DailyAvailabilityViewModelPracticeId { get; set; }
        [Display(Name = "Practice")]
        public string DailyAvailabilityViewModelPracticeName { get; set; }

        [Display(Name = "Start1")]
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

        [Display(Name = "Start3")]
        public DateTime? StartTime3
        {
            get
            {
                return IsWorking ? _startTime3 : null;
            }
            set
            {
                _startTime3 = value;
            }
        }

        [Display(Name = "End3")]
        public DateTime? EndTime3
        {
            get
            {
                return IsWorking ? _endTime3 : null;
            }
            set
            {
                _endTime3 = value;
            }
        }

        public bool IsWorking { get; set; }
    }
}