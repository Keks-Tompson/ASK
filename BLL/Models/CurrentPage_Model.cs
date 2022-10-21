using ASK.BLL.Helper.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class CurrentPage_Model
    {
        public ColorSensorParametrError_Model Color_Param { get; set; } = new ColorSensorParametrError_Model();
        public VisibilityReportOptions_JSON_Model VisibilityReportOptions { get; set; } = new VisibilityReportOptions_JSON_Model();

        public Sensor_4_20_Model SensorNow { get; set; } = new Sensor_4_20_Model();
        public Sensor_4_20_Model SensorScaledNow { get; set; } = new Sensor_4_20_Model();


        public Array20M_Model CurrentConcEmis { get; set; } = new Array20M_Model();
        public PDZ_String_Active_Model PDZ_Current_String { get; set; } = new PDZ_String_Active_Model();
        public ColorSensorEror_Model Color_Conc { get; set; } = new ColorSensorEror_Model();
        public ColorSensorEror_Model Color_Emis { get; set; } = new ColorSensorEror_Model();

        public string is_NotConnection_Text { get; set; }                                                       //Удалить и всё что будет связано заменить на нормальный код
        public string is_NotConnection_Color { get; set; }                                                      //Удалить и всё что будет связано заменить на нормальный код
    }
}
