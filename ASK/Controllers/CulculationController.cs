using ASK.Models;
using ASK.BLL.Helper.Setting;
using Microsoft.AspNetCore.Mvc;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;

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
            calculationViewModel.CalculationSettings = GlobalStaticSettingsASK.CalculationSetting;
            calculationViewModel.SensorRanges = GlobalStaticSettingsASK.SensorRange;

            switch (GlobalStaticSettingsASK.CalculationSetting.TypeViews)
            {
                case TypeCalulationViews.CO:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.CO_4_20mA, GlobalStaticSettingsASK.SensorRange.CO.Is_ppm, 1.14);
                    calculationViewModel.Calculation.Name = "CO";
                    break;

                case TypeCalulationViews.CO2:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.CO2_4_20mA, GlobalStaticSettingsASK.SensorRange.CO2.Is_ppm, 1.98);
                    calculationViewModel.Calculation.Name = "CO₂";
                    break;

                case TypeCalulationViews.SO2:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.SO2_4_20mA, GlobalStaticSettingsASK.SensorRange.SO2.Is_ppm, 2.92);
                    calculationViewModel.Calculation.Name = "SO₂";
                    break;

                case TypeCalulationViews.CH4:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.CH4_4_20mA, GlobalStaticSettingsASK.SensorRange.CH4.Is_ppm, 0.65);
                    calculationViewModel.Calculation.Name = "CH₄";
                    break;

                case TypeCalulationViews.H2S:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.H2S_4_20mA, GlobalStaticSettingsASK.SensorRange.H2S.Is_ppm, 1.36);
                    calculationViewModel.Calculation.Name = "H₂S";
                    break;

                case TypeCalulationViews.NH3:
                    calculationViewModel.Calculation = _Calculation_Services.Count(GlobalStaticSettingsASK.CalculationSetting, GlobalStaticSettingsASK.SensorScaledNow.NH3_4_20mA, GlobalStaticSettingsASK.SensorRange.NH3.Is_ppm, 0.73);
                    calculationViewModel.Calculation.Name = "NH₃";
                    break;
            }

            return Json(calculationViewModel);
        }



        public IActionResult ViewsUpdate(int number)
        {
            switch (number)
            {
                case 0:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.CO;
                    break;

                case 1:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.CO2;
                    break;

                case 2:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.SO2;
                    break;

                case 3:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.CH4;
                    break;

                case 4:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.H2S;
                    break;

                case 5:
                    GlobalStaticSettingsASK.CalculationSetting.TypeViews = TypeCalulationViews.NH3;
                    break;
            }

            return new NoContentResult();
        }
    }
}
