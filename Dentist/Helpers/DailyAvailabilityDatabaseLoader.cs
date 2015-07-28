using System.Collections.Generic;
using System.Linq;
using Dentist.Models;

namespace Dentist.Helpers
{
    public class DailyAvailabilityDatabaseLoader : IDailyAvailabilityLoader
    {
        private bool _Success;
        private Doctor _Doctor;
        private int _PracticeId;
        private List<DailyAvailability> _DailyAvailabilities;

        public DailyAvailabilityDatabaseLoader(List<DailyAvailability> dailyAvailabilities, Doctor doctor, int practiceId)
        {
            _DailyAvailabilities = dailyAvailabilities;
            _Doctor = doctor;
            _PracticeId = practiceId;
            Load();
        }

        private void Load()
        {
            var context = new ApplicationDbContext();
            var dailyAvailabilitySettings = context.DailyAvailabilitySettings.ToList();
            if (dailyAvailabilitySettings.Count == 0)
            {
                _Success = false;
                return;
            }

            foreach (var dailyAvailabilitySetting in dailyAvailabilitySettings)
            {
                _DailyAvailabilities.Add(new DailyAvailability
                {
                    DayOfWeek = dailyAvailabilitySetting.DayOfWeek,
                    IsWorking = dailyAvailabilitySetting.IsWorking,
                    StartTime1 = dailyAvailabilitySetting.StartTime1,
                    EndTime1 = dailyAvailabilitySetting.EndTime1,
                    StartTime2 = dailyAvailabilitySetting.StartTime2,
                    EndTime2 = dailyAvailabilitySetting.EndTime2,
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