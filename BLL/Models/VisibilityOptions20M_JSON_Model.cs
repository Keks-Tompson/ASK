using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class VisibilityOptions20M_JSON_Model
    {
        public string CheckBox { get; set; } = "checked";

        public string CO_Conc { get; set; } = "table-cell";
        public string CO2_Conc { get; set; } = "none";
        public string NO_Conc { get; set; } = "none";
        public string NO2_Conc { get; set; } = "none";
        public string NOx_Conc { get; set; } = "table-cell";
        public string SO2_Conc { get; set; } = "table-cell";
        public string Dust_Conc { get; set; } = "table-cell";
        public string CH4_Conc { get; set; } = "none";
        public string H2S_Conc { get; set; } = "none";
        public string Add_Conc_1 { get; set; } = "none";
        public string Add_Conc_2 { get; set; } = "none";
        public string Add_Conc_3 { get; set; } = "none";
        public string Add_Conc_4 { get; set; } = "none";
        public string Add_Conc_5 { get; set; } = "none";


        //Выбросы
        public string CO_Emis { get; set; } = "table-cell";
        public string CO2_Emis { get; set; } = "none";
        public string NO_Emis { get; set; } = "table-cell";
        public string NO2_Emis { get; set; } = "table-cell";
        public string NOx_Emis { get; set; } = "table-cell";
        public string SO2_Emis { get; set; } = "table-cell";
        public string CH4_Emis { get; set; } = "none";
        public string H2S_Emis { get; set; } = "none";
        public string Dust_Emis { get; set; } = "table-cell";
        public string Add_Emis_1 { get; set; } = "none";
        public string Add_Emis_2 { get; set; } = "none";
        public string Add_Emis_3 { get; set; } = "none";
        public string Add_Emis_4 { get; set; } = "none";
        public string Add_Emis_5 { get; set; } = "none";
        //Процент превышений выбросы


        //Доп
        public string O2_Wet { get; set; } = "table-cell";
        public string O2_Dry { get; set; } = "table-cell";
        public string H2O { get; set; } = "none";


        //Параметры
        public string Pressure { get; set; } = "table-cell";
        public string Temperature { get; set; } = "table-cell";
        public string Speed { get; set; } = "table-cell";
        public string Flow { get; set; } = "table-cell";

        public string Temperature_KIP { get; set; } = "none";
        public string Temperature_NOx { get; set; } = "none";


        public string Mode_ASK { get; set; } = "table-cell";
        public string PDZ_Fuel { get; set; } = "table-cell";
    }
}
