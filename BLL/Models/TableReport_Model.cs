using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class TableReport_Model
    {
        public DateTime Date { get; set; }

        //Концентрации
        public double CO_Conc { get; set; }
        public double CO2_Conc { get; set; }
        public double NO_Conc { get; set; }
        public double NO2_Conc { get; set; }
        public double NOx_Conc { get; set; }
        public double SO2_Conc { get; set; }
        public double Dust_Conc { get; set; }
        public double CH4_Conc { get; set; }
        public double H2S_Conc { get; set; }
        public double Add_Conc_1 { get; set; }
        public double Add_Conc_2 { get; set; }
        public double Add_Conc_3 { get; set; }
        public double Add_Conc_4 { get; set; }
        public double Add_Conc_5 { get; set; }
        
        //Проценты превышений концентраций
        public double CO_Conc_Percent { get; set; }
        public double CO2_Conc_Percent { get; set; }
        public double NO_Conc_Percent { get; set; }
        public double NO2_Conc_Percent { get; set; }
        public double NOx_Conc_Percent { get; set; }
        public double SO2_Conc_Percent { get; set; }
        public double Dust_Conc_Percent { get; set; }
        public double CH4_Conc_Percent { get; set; }
        public double H2S_Conc_Percent { get; set; }
        public double Add_Conc_1_Percent { get; set; }
        public double Add_Conc_2_Percent { get; set; }
        public double Add_Conc_3_Percent { get; set; }
        public double Add_Conc_4_Percent { get; set; }
        public double Add_Conc_5_Percent { get; set; }

        //Выбросы
        public double CO_Emis { get; set; }
        public double CO2_Emis { get; set; }
        public double NO_Emis { get; set; }
        public double NO2_Emis { get; set; }
        public double NOx_Emis { get; set; }
        public double SO2_Emis { get; set; }
        public double CH4_Emis { get; set; }
        public double H2S_Emis { get; set; }
        public double Dust_Emis { get; set; }
        public double Add_Emis_1 { get; set; }
        public double Add_Emis_2 { get; set; }
        public double Add_Emis_3 { get; set; }
        public double Add_Emis_4 { get; set; }
        public double Add_Emis_5 { get; set; }
        
        //Процент превышений выбросы
        public double CO_Emis_Percent { get; set; }
        public double CO2_Emis_Percent { get; set; }
        public double NO_Emis_Percent { get; set; }
        public double NO2_Emis_Percent { get; set; }
        public double NOx_Emis_Percent { get; set; }
        public double SO2_Emis_Percent { get; set; }
        public double CH4_Emis_Percent { get; set; }
        public double H2S_Emis_Percent { get; set; }
        public double Dust_Emis_Percent { get; set; }
        public double Add_Emis_1_Percent { get; set; }
        public double Add_Emis_2_Percent { get; set; }
        public double Add_Emis_3_Percent { get; set; }
        public double Add_Emis_4_Percent { get; set; }
        public double Add_Emis_5_Percent { get; set; }


        //Доп
        public double O2_Wet { get; set; }
        public double O2_Dry { get; set; }
        public double H2O { get; set; }

        //Параметры
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Speed { get; set; }
        public double Flow { get; set; }
        public double Temperature_KIP { get; set; }
        public double Temperature_NOx { get; set; }


        public double Mode_ASK { get; set; } //Пусть будет double, я так хочу
        public int PDZ_Fuel { get; set; }
    }
}
