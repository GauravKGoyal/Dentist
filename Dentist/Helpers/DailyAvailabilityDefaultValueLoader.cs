using System;
using System.Collections.Generic;
using Dentist.Models;

namespace Dentist.Helpers
{
    public class DailyAvailabilityDefaultValueLoader : IDailyAvailabilityLoader
    {
        private bool _Success;
        private Doctor _Doctor;
        private int _PracticeId;
        private List<DailyAvailability> _DailyAvailabilities;

        public DailyAvailabilityDefaultValueLoader(List<DailyAvailability> dailyAvailabilities, Doctor doctor, int practiceId)
        {
            _DailyAvailabilities = dailyAvailabilities;
            _Doctor = doctor;
            _PracticeId = practiceId;
            Load();
        }

        private void Load()
        {
            var startTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0, 0);
            var endTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0, 0);
            var startTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 30, 0, 0);
            var endTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0, 0);

            Array daysOfWeek = Enum.GetValues(typeof(DayOfWeek));
            foreach (object dayOfWeek in daysOfWeek)
            {
                _DailyAvailabilities.Add(new DailyAvailability
                {
                    DayOfWeek = (DayOfWeek)dayOfWeek,
                    IsWorking = true,
                    StartTime1 = startTime1,
                    EndTime1 = endTime1,
                    StartTime2 = startTime2,
                    EndTime2 = endTime2,
                    Doctor = _Doctor,
                    PracticeId = _PracticeId
                });
            }
            _Success = true;
        }

        public bool Success()
        {
            return _Success;
        }
    }
}