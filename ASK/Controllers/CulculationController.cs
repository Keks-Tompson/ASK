using ASK.Models;
using ASK.BLL.Helper.Setting;
using Microsoft.AspNetCore.Mvc;
using ASK.BLL.Interfaces;

namespace ASK.Controllers
{
    public class CalculationController : Controller
    {
        CalculationViewModel calculationViewModel = new CalculationViewModel();

        private readonly ICalculation _Calculation_Services;



        public CalculationController(ICalculation Calculation_Services)
        {
            _Calculation_Services = Calculation_Services;
        }



        public IActionResult Index()
        {
            return View();
        }



        public JsonResult GetCurrentCalculation()
        {
            //изменить
            calculationViewModel.CalculationSettings = GlobalStaticSettingsASK.CalculationSetting;
            calculationViewModel.SensorRanges = GlobalStaticSettingsASK.SensorRange;
            calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.CO_4_20mA, GlobalStaticSettingsASK.SensorRange.CO.Is_ppm, 1.14);
            calculationViewModel.Calculation.Name = "CO";

            return Json(calculationViewModel);
        }
    }
}
