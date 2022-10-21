using ASK.BLL.Models;

namespace ASK.Models
{
    public class CalculationViewModel
    {
        public CalculationSetting_JSON_Model CalculationSettings { get; set; }
        public SensorRange_JSON_Model SensorRanges { get; set; }
        public Calculation_Model Calculation { get; set; }
    }
}
