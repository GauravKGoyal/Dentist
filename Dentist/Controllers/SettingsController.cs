using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Dentist.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

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
            var dailyAvailabilitySettings = ReadContext.DailyAvailabilitySettings.ToList();
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
                var loadedDailyAvailabilitySettings = WriteContext.DailyAvailabilitySettings.Where(x => idsToLoad.Contains(x.Id));
                foreach (var dailyAvailabilitySetting in loadedDailyAvailabilitySettings)
                {
                    var id = dailyAvailabilitySetting.Id;
                    var viewModel = dailyAvailabilitySettingViewModels.Single(x => x.Id == id);
                    Mapper.Map(viewModel, dailyAvailabilitySetting);
                }
                WriteContext.TrySaveChanges(ModelState);
            }

            return Json(dailyAvailabilitySettingViewModels.ToDataSourceResult(request, ModelState));
        }

        public ActionResult EditCalenderSetting()
        {            
            var calenderSetting = ReadContext.CalenderSettings.First();
            var viewModel = Mapper.Map<CalenderSettingViewModel>(calenderSetting);
            return View(viewModel);
        } 

        [HttpPost]
        public ActionResult EditCalenderSetting(CalenderSettingViewModel calenderSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var calenderSetting = WriteContext.CalenderSettings.FirstOrDefault();
                Mapper.Map(calenderSettingViewModel, calenderSetting);
                WriteContext.TrySaveChanges(ModelState);
            }
            return View(calenderSettingViewModel);
        }
        
    }
}
