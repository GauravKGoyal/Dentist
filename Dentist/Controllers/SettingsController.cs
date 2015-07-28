using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Dentist.Migrations;
using Dentist.Models;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebGrease.Css.Extensions;

namespace Dentist.Controllers
{
    public class SettingsController : BaseController
    {
        // GET: Settings
        public ActionResult DailyAvailabilitySetting()
        {
            return View();
        }

        public ActionResult GetDailyAvailabilitySetting([DataSourceRequest] DataSourceRequest request)
        {
            // Create settings if they don't exist
            if (!Context.DailyAvailabilitySettings.Any())
            {
                CreateDefaultDailyAvailabilitySettings();
            }

            var dailyAvailabilitySettings = Context.DailyAvailabilitySettings.ToList();
            var viewModels = Mapper.Map<List<DailyAvailabilitySettingViewModel>>(dailyAvailabilitySettings);
            return Json(viewModels.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDailyAvailabilitySetting([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<DailyAvailabilitySettingViewModel> dailyAvailabilitySettingViewModels)
        {
            dailyAvailabilitySettingViewModels = dailyAvailabilitySettingViewModels.ToList();
            if (ModelState.IsValid)
            {
                var idsToLoad = dailyAvailabilitySettingViewModels.Select(x => x.Id);
                var loadedDailyAvailabilitySettings = Context.DailyAvailabilitySettings.Where(x => idsToLoad.Contains(x.Id));
                foreach (var dailyAvailabilitySetting in loadedDailyAvailabilitySettings)
                {
                    var id = dailyAvailabilitySetting.Id;
                    var viewModel = dailyAvailabilitySettingViewModels.Single(x => x.Id == id);
                    Mapper.Map(viewModel, dailyAvailabilitySetting);
                }
                Context.SaveChanges();
            }

            return Json(dailyAvailabilitySettingViewModels.ToDataSourceResult(request, ModelState));
        }

        public ActionResult EditCalenderSetting()
        {
            // Create settings if it doesn't exist
            if (!Context.CalenderSettings.Any())
            {
                CreateDefaultCalenderSettings();
            }

            var calenderSetting = Context.CalenderSettings.First();
            var viewModel = Mapper.Map<CalenderSettingViewModel>(calenderSetting);
            return View(viewModel);
        }

        private void CreateDefaultCalenderSettings()
        {
            Context.CalenderSettings.Add(new CalenderSetting()
            {
                DayStartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0, 0),
                DayEndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0, 0),
                WorkWeekStartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0, 0),
                WorkWeekEndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0, 0),
                WorkWeekStartDay = DayOfWeek.Monday,
                WorkWeekEndDay = DayOfWeek.Friday
            });

            Context.SaveChanges();
        }

        [HttpPost]
        public ActionResult EditCalenderSetting(CalenderSettingViewModel calenderSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var calenderSetting = Context.CalenderSettings.FirstOrDefault();
                Mapper.Map(calenderSettingViewModel, calenderSetting);
                Context.SaveChanges();
            }
            return View(calenderSettingViewModel);
        }
        private void CreateDefaultDailyAvailabilitySettings()
        {
            var startTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0, 0);
            var endTime1 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 0, 0, 0);
            var startTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 30, 0, 0);
            var endTime2 = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0, 0);
            Array daysOfWeek = Enum.GetValues(typeof (DayOfWeek));
            foreach (object dayOfWeek in daysOfWeek)
            {
                Context.DailyAvailabilitySettings.Add(new DailyAvailabilitySetting()
                {
                    DayOfWeek = (DayOfWeek) dayOfWeek,
                    IsWorking = true,
                    StartTime1 = startTime1,
                    EndTime1 = endTime1,
                    StartTime2 = startTime2,
                    EndTime2 = endTime2
                });
            }
            Context.SaveChanges();
        }
    }
}
