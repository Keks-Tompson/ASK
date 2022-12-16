using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Models
{
    public class SensorRange_JSON_Model
    {
        public RangePPM_Model CO { get; set; } = new RangePPM_Model();
        public RangePPM_Model CO2 { get; set; } = new RangePPM_Model();
        public RangePPM_Model NO { get; set; } = new RangePPM_Model();
        public RangePPM_Model NO2 { get; set; } = new RangePPM_Model();
        public RangePPM_Model NOx { get; set; } = new RangePPM_Model();
        public RangePPM_Model SO2 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Dust { get; set; } = new RangePPM_Model();
        public RangePPM_Model CH4 { get; set; } = new RangePPM_Model();
        public RangePPM_Model H2S { get; set; } = new RangePPM_Model();
        public RangePPM_Model NH3 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Rezerv_1 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Rezerv_2 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Rezerv_3 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Rezerv_4 { get; set; } = new RangePPM_Model();
        public RangePPM_Model Rezerv_5 { get; set; } = new RangePPM_Model();

        public Range_Model O2Wet { get; set; } = new Range_Model();
        public Range_Model O2Dry { get; set; } = new Range_Model();
        public Range_Model H2O { get; set; } = new Range_Model();
        public Range_Model Pressure { get; set; } = new Range_Model();
        public Range_Model Temperature { get; set; } = new Range_Model();
        public Range_Model Speed { get; set; } = new Range_Model();

        public Range_Model Temperature_KIP { get; set; } = new Range_Model();
        public Range_Model Temperature_NOx { get; set; } = new Range_Model();
        public Range_Model Pressure_KIP { get; set; } = new Range_Model();
        public Range_Model Temperature_Room { get; set; } = new Range_Model();
        public Range_Model Temperature_PGS { get; set; } = new Range_Model();
        public Range_Model Temperature_Point_Dew { get; set; } = new Range_Model();  //Температруа точки росы воздуха КИП
        public Range_Model O2_Room { get; set; } = new Range_Model();                //Кислород в помщенеии
        public Range_Model O2_PGS { get; set; } = new Range_Model();                 //Кислород в помщении ПГС


        public SensorRange_JSON_Model()
        {
            CO.Max = 100.0;
            CO.Min = 0.0;
            CO.Is_Used = false;
            CO.Is_ppm = false;

            CO2.Max = 100.0;
            CO2.Min = 0.0;
            CO2.Is_Used = false;
            CO2.Is_ppm = false;

            NO.Max = 100.0;
            NO.Min = 0.0;
            NO.Is_Used = false;
            NO.Is_ppm = false;

            NO2.Max = 100.0;
            NO2.Min = 0.0;
            NO2.Is_ppm = false;

            NOx.Max = 100.0;
            NOx.Min = 0.0;
            NOx.Is_Used = false;
            NOx.Is_ppm = false;

            SO2.Max = 100.0;
            SO2.Min = 0.0;
            SO2.Is_Used = false;
            SO2.Is_ppm = false;

            Dust.Max = 100.0;
            Dust.Min = 0.0;
            Dust.Is_Used = false;
            Dust.Is_ppm = false;

            CH4.Max = 100.0;
            CH4.Min = 0.0;
            CH4.Is_Used = false;
            CH4.Is_ppm = false;

            H2S.Max = 100.0;
            H2S.Min = 0.0;
            H2S.Is_Used = false;
            H2S.Is_ppm = false;

            NH3.Max = 100.0;
            NH3.Min = 0.0;
            NH3.Is_Used = false;
            NH3.Is_ppm = false;

            Rezerv_1.Max = 100.0;
            Rezerv_1.Min = 0.0;
            Rezerv_1.Is_Used = false;
            Rezerv_1.Is_ppm = false;

            Rezerv_2.Max = 100.0;
            Rezerv_2.Min = 0.0;
            Rezerv_2.Is_Used = false;
            Rezerv_2.Is_ppm = false;

            Rezerv_3.Max = 100.0;
            Rezerv_3.Min = 0.0;
            Rezerv_3.Is_Used = false;
            Rezerv_3.Is_ppm = false;

            Rezerv_4.Max = 100.0;
            Rezerv_4.Min = 0.0;
            Rezerv_4.Is_Used = false;
            Rezerv_4.Is_ppm = false;

            Rezerv_5.Max = 100.0;
            Rezerv_5.Min = 0.0;
            Rezerv_5.Is_Used = false;
            Rezerv_5.Is_ppm = false;


            O2Wet.Max = 25.0;
            O2Wet.Min = 0.0;
            O2Wet.Is_Used = false;

            O2Dry.Max = 25.0;
            O2Dry.Min = 0.0;
            O2Dry.Is_Used = false;

            O2_Room.Max = 25.0;
            O2_Room.Min = 0.0;
            O2_Room.Is_Used = false;

            O2_PGS.Max = 25.0;
            O2_PGS.Min = 0.0;
            O2_PGS.Is_Used = false;

            H2O.Max = 100.0;
            H2O.Min = 0.0;
            H2O.Is_Used = false;

            Pressure.Max = 100.0;
            Pressure.Min = 0.0;
            Pressure.Is_Used = false;

            Pressure_KIP.Max = 100.0;
            Pressure_KIP.Min = 0.0;
            Pressure_KIP.Is_Used = false;

            Temperature.Max = 200.0;
            Temperature.Min = 0.0;
            Temperature.Is_Used = false;

            Temperature_Point_Dew.Max = 100.0;
            Temperature_Point_Dew.Min = 0.0;
            Temperature_Point_Dew.Is_Used = false;

            Temperature_Room.Max = 100.0;
            Temperature_Room.Min = 0.0;
            Temperature_Room.Is_Used = false;

            Temperature_PGS.Max = 100.0;
            Temperature_PGS.Min = 0.0;
            Temperature_PGS.Is_Used = false;

            Speed.Max = 100.0;
            Speed.Min = 0.0;
            Speed.Is_Used = false;

            Temperature_KIP.Max = 100.0;
            Temperature_KIP.Min = 0.0;
            Temperature_KIP.Is_Used = false;

            Temperature_NOx.Max = 100.0;
            Temperature_NOx.Min = 0.0;
            Temperature_NOx.Is_Used = false;
        }
    }
}
