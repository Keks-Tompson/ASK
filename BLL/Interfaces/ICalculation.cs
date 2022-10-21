using ASK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Interfaces
{
    public interface ICalculation
    {
        public Calculation_Model Count(CalculationSetting_JSON_Model calculationSetting, double sensorNow, bool is_ppm, double r_ppm = 1.0);

    }
}
