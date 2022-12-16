using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class TableReport_Model
    {
        public DateTime Date { get; set; }

        //Концентрации
        public double CO_Conc { get; set; } = 0;
        public double CO2_Conc { get; set; } = 0;
        public double NO_Conc { get; set; } = 0;
        public double NO2_Conc { get; set; } = 0;
        public double NOx_Conc { get; set; } = 0;
        public double SO2_Conc { get; set; } = 0;
        public double Dust_Conc { get; set; } = 0;
        public double CH4_Conc { get; set; } = 0;
        public double H2S_Conc { get; set; } = 0;
        public double NH3_Conc { get; set; } = 0;
        public double Add_Conc_1 { get; set; } = 0;
        public double Add_Conc_2 { get; set; } = 0;
        public double Add_Conc_3 { get; set; } = 0;
        public double Add_Conc_4 { get; set; } = 0;
        public double Add_Conc_5 { get; set; } = 0;

        //Проценты превышений концентраций
        public double CO_Conc_Percent { get; set; } = 0;
        public double CO2_Conc_Percent { get; set; } = 0;
        public double NO_Conc_Percent { get; set; } = 0;
        public double NO2_Conc_Percent { get; set; } = 0;
        public double NOx_Conc_Percent { get; set; } = 0;
        public double SO2_Conc_Percent { get; set; } = 0;
        public double Dust_Conc_Percent { get; set; } = 0;
        public double CH4_Conc_Percent { get; set; } = 0;
        public double H2S_Conc_Percent { get; set; } = 0;
        public double NH3_Conc_Percent { get; set; } = 0;
        public double Add_Conc_1_Percent { get; set; } = 0;
        public double Add_Conc_2_Percent { get; set; } = 0;
        public double Add_Conc_3_Percent { get; set; } = 0;
        public double Add_Conc_4_Percent { get; set; } = 0;
        public double Add_Conc_5_Percent { get; set; } = 0;

        //Выбросы
        public double CO_Emis { get; set; } = 0;
        public double CO2_Emis { get; set; } = 0;
        public double NO_Emis { get; set; } = 0;
        public double NO2_Emis { get; set; } = 0;
        public double NOx_Emis { get; set; } = 0;
        public double SO2_Emis { get; set; } = 0;
        public double CH4_Emis { get; set; } = 0;
        public double H2S_Emis { get; set; } = 0;
        public double NH3_Emis { get; set; } = 0;
        public double Dust_Emis { get; set; } = 0;
        public double Add_Emis_1 { get; set; } = 0;
        public double Add_Emis_2 { get; set; } = 0;
        public double Add_Emis_3 { get; set; } = 0;
        public double Add_Emis_4 { get; set; } = 0;
        public double Add_Emis_5 { get; set; } = 0;

        //Процент превышений выбросы
        public double CO_Emis_Percent { get; set; } = 0;
        public double CO2_Emis_Percent { get; set; } = 0;
        public double NO_Emis_Percent { get; set; } = 0;
        public double NO2_Emis_Percent { get; set; } = 0;
        public double NOx_Emis_Percent { get; set; } = 0;
        public double SO2_Emis_Percent { get; set; } = 0;
        public double CH4_Emis_Percent { get; set; } = 0;
        public double NH3_Emis_Percent { get; set; } = 0;
        public double H2S_Emis_Percent { get; set; } = 0;
        public double Dust_Emis_Percent { get; set; } = 0;
        public double Add_Emis_1_Percent { get; set; } = 0;
        public double Add_Emis_2_Percent { get; set; } = 0;
        public double Add_Emis_3_Percent { get; set; } = 0;
        public double Add_Emis_4_Percent { get; set; } = 0;
        public double Add_Emis_5_Percent { get; set; } = 0;


        //Доп
        public double O2_Wet { get; set; } = 0;
        public double O2_Dry { get; set; } = 0;
        public double H2O { get; set; } = 0;

        //Параметры
        public double Pressure { get; set; } = 0;
        public double Temperature { get; set; } = 0;
        public double Speed { get; set; } = 0;
        public double Flow { get; set; } = 0;

        public double Temperature_KIP { get; set; } = 0;
        public double Temperature_NOx { get; set; } = 0;
        public double Pressure_KIP { get; set; } = 0;
        public double Temperature_Room { get; set; } = 0;
        public double Temperature_PGS { get; set; } = 0;
        public double Temperature_Point_Dew { get; set; } = 0;      //Температруа точки росы воздуха КИП
        public double O2_Room { get; set; } = 0;                    //Кислород в помщенеии
        public double O2_PGS { get; set; } = 0;                     //Кислород в помщении ПГС


        public double Mode_ASK { get; set; } = 0;                   //Пусть будет double, я так хочу
        public int PDZ_Fuel { get; set; } = 0;
    }
}
