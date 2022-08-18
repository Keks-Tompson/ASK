
using ASK.BLL.Services;
using ASK.DAL;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.BLL.Helper.Report
{
    public class ReportMonth
    {
        public List<TableReportDay> tableReportDay { get; set; }    //Значение 20-минуток

        public PDZ_Active PDZ { get; set; }                         //Текущее значение ПДК


        public TableReportDay avgTotal_20M { get; set; }                  //Подвал, среднее занчение и сумма концентраций(+парамеры) и выбросов соответственно
        public TableReportDay sumTotal_20M { get; set; }                 //Подвал, сумма выбросов

        int NumberDays;

        public List<ReportDay> reportDays;


        public ReportMonth(DateTime yearMonth)
        {

            avgTotal_20M = new TableReportDay();
            sumTotal_20M = new TableReportDay();

            tableReportDay = new List<TableReportDay>();

            var dataNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);




            if (yearMonth >= dataNow)         
                    NumberDays = 0;
            else
                NumberDays = DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month);

            if (yearMonth.Year == DateTime.Now.Year && yearMonth.Month == DateTime.Now.Month)
                NumberDays = DateTime.Now.Day;





            for (int i = 0; i < NumberDays; i++)
            {
                tableReportDay.Add(new TableReportDay());

                var bufdayReport = new ReportDay(new DateTime(yearMonth.Year, yearMonth.Month, i + 1));

                tableReportDay[i].Date = new DateTime(yearMonth.Year, yearMonth.Month, i + 1);

                tableReportDay[i].Mode_ASK = bufdayReport.avgTotal_20M.Mode_ASK;


                if (bufdayReport.tableReportDay.Count > 0) tableReportDay[i].PDZ_Fuel = bufdayReport.tableReportDay[0].PDZ_Fuel; //Обязательно должен идти первее подсчёта процентов превышений
                else tableReportDay[i].PDZ_Fuel = 0;

                //Концентрации
                tableReportDay[i].CO_Conc = bufdayReport.avgTotal_20M.CO_Conc;
                tableReportDay[i].CO2_Conc = bufdayReport.avgTotal_20M.CO2_Conc;
                tableReportDay[i].NO_Conc = bufdayReport.avgTotal_20M.NO_Conc;
                tableReportDay[i].NO2_Conc = bufdayReport.avgTotal_20M.NO2_Conc;
                tableReportDay[i].NOx_Conc = bufdayReport.avgTotal_20M.NOx_Conc;
                tableReportDay[i].SO2_Conc = bufdayReport.avgTotal_20M.SO2_Conc;
                tableReportDay[i].Dust_Conc = bufdayReport.avgTotal_20M.Dust_Conc;
                tableReportDay[i].CH4_Conc = bufdayReport.avgTotal_20M.CH4_Conc;
                tableReportDay[i].H2S_Conc = bufdayReport.avgTotal_20M.H2S_Conc;
                tableReportDay[i].Add_Conc_1 = bufdayReport.avgTotal_20M.Add_Conc_1;
                tableReportDay[i].Add_Conc_2 = bufdayReport.avgTotal_20M.Add_Conc_2;
                tableReportDay[i].Add_Conc_3 = bufdayReport.avgTotal_20M.Add_Conc_3;
                tableReportDay[i].Add_Conc_4 = bufdayReport.avgTotal_20M.Add_Conc_4;
                tableReportDay[i].Add_Conc_5 = bufdayReport.avgTotal_20M.Add_Conc_5;

                tableReportDay[i].CO_Conc_Percent = bufdayReport.durationTotal_20M.CO_Conc_Percent;
                tableReportDay[i].CO2_Conc_Percent = bufdayReport.durationTotal_20M.CO2_Conc_Percent;
                tableReportDay[i].NO_Conc_Percent = bufdayReport.durationTotal_20M.NO_Conc_Percent;
                tableReportDay[i].NO2_Conc_Percent = bufdayReport.durationTotal_20M.NO2_Conc_Percent;
                tableReportDay[i].NOx_Conc_Percent = bufdayReport.durationTotal_20M.NOx_Conc_Percent;
                tableReportDay[i].SO2_Conc_Percent = bufdayReport.durationTotal_20M.SO2_Conc_Percent;
                tableReportDay[i].Dust_Conc_Percent = bufdayReport.durationTotal_20M.Dust_Conc_Percent;
                tableReportDay[i].CH4_Conc_Percent = bufdayReport.durationTotal_20M.CH4_Conc_Percent;
                tableReportDay[i].H2S_Conc_Percent = bufdayReport.durationTotal_20M.H2S_Conc_Percent;
                tableReportDay[i].Add_Conc_1_Percent = bufdayReport.durationTotal_20M.Add_Conc_1_Percent;
                tableReportDay[i].Add_Conc_2_Percent = bufdayReport.durationTotal_20M.Add_Conc_2_Percent;
                tableReportDay[i].Add_Conc_3_Percent = bufdayReport.durationTotal_20M.Add_Conc_3_Percent;
                tableReportDay[i].Add_Conc_4_Percent = bufdayReport.durationTotal_20M.Add_Conc_4_Percent;
                tableReportDay[i].Add_Conc_5_Percent = bufdayReport.durationTotal_20M.Add_Conc_5_Percent;



                //Выбросы
                tableReportDay[i].CO_Emis = bufdayReport.sumTotal_20M.CO_Emis;
                tableReportDay[i].CO2_Emis = bufdayReport.sumTotal_20M.CO2_Emis;
                tableReportDay[i].NO_Emis = bufdayReport.sumTotal_20M.NO_Emis;
                tableReportDay[i].NO2_Emis = bufdayReport.sumTotal_20M.NO2_Emis;
                tableReportDay[i].NOx_Emis = bufdayReport.sumTotal_20M.NOx_Emis;
                tableReportDay[i].SO2_Emis = bufdayReport.sumTotal_20M.SO2_Emis;
                tableReportDay[i].Dust_Emis = bufdayReport.sumTotal_20M.Dust_Emis;
                tableReportDay[i].CH4_Emis = bufdayReport.sumTotal_20M.CH4_Emis;
                tableReportDay[i].H2S_Emis = bufdayReport.sumTotal_20M.H2S_Emis;
                tableReportDay[i].Add_Emis_1 = bufdayReport.sumTotal_20M.Add_Emis_1;
                tableReportDay[i].Add_Emis_2 = bufdayReport.sumTotal_20M.Add_Emis_2;
                tableReportDay[i].Add_Emis_3 = bufdayReport.sumTotal_20M.Add_Emis_3;
                tableReportDay[i].Add_Emis_4 = bufdayReport.sumTotal_20M.Add_Emis_4;
                tableReportDay[i].Add_Emis_5 = bufdayReport.sumTotal_20M.Add_Emis_5;

                tableReportDay[i].CO_Emis_Percent = bufdayReport.avgTotal_20M.CO_Emis_Percent;
                tableReportDay[i].CO2_Emis_Percent = bufdayReport.avgTotal_20M.CO2_Emis_Percent;
                tableReportDay[i].NO_Emis_Percent = bufdayReport.avgTotal_20M.NO_Emis_Percent;
                tableReportDay[i].NO2_Emis_Percent = bufdayReport.avgTotal_20M.NO2_Emis_Percent;
                tableReportDay[i].NOx_Emis_Percent = bufdayReport.avgTotal_20M.NOx_Emis_Percent;
                tableReportDay[i].SO2_Emis_Percent = bufdayReport.avgTotal_20M.SO2_Emis_Percent;
                tableReportDay[i].Dust_Emis_Percent = bufdayReport.avgTotal_20M.Dust_Emis_Percent;
                tableReportDay[i].CH4_Emis_Percent = bufdayReport.avgTotal_20M.CH4_Emis_Percent;
                tableReportDay[i].H2S_Emis_Percent = bufdayReport.avgTotal_20M.H2S_Emis_Percent;
                tableReportDay[i].Add_Emis_1_Percent = bufdayReport.avgTotal_20M.Add_Emis_1_Percent;
                tableReportDay[i].Add_Emis_2_Percent = bufdayReport.avgTotal_20M.Add_Emis_2_Percent;
                tableReportDay[i].Add_Emis_3_Percent = bufdayReport.avgTotal_20M.Add_Emis_3_Percent;
                tableReportDay[i].Add_Emis_4_Percent = bufdayReport.avgTotal_20M.Add_Emis_4_Percent;
                tableReportDay[i].Add_Emis_5_Percent = bufdayReport.avgTotal_20M.Add_Emis_5_Percent;


                //Параметры
                tableReportDay[i].Pressure = bufdayReport.avgTotal_20M.Pressure;
                tableReportDay[i].Temperature = bufdayReport.avgTotal_20M.Temperature;
                tableReportDay[i].Temperature_KIP = bufdayReport.avgTotal_20M.Temperature_KIP;
                tableReportDay[i].Temperature_NOx = bufdayReport.avgTotal_20M.Temperature_NOx;
                tableReportDay[i].Speed = bufdayReport.avgTotal_20M.Speed;
                tableReportDay[i].Flow = bufdayReport.avgTotal_20M.Flow;
                tableReportDay[i].O2_Dry = bufdayReport.avgTotal_20M.O2_Dry;
                tableReportDay[i].O2_Wet = bufdayReport.avgTotal_20M.O2_Wet;
                tableReportDay[i].H2O = bufdayReport.avgTotal_20M.H2O;


                sumTotal_20M.Mode_ASK += bufdayReport.sumTotal_20M.Mode_ASK;
            }











            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Вытаскиваем ПДЗ на месяц
                PDZ pdz = new PDZ();
                PDZ_Service pdz_Service = new PDZ_Service(db);
                pdz = pdz_Service.FisrsPDZMonth(yearMonth);    

                PDZ = new PDZ_Active();

               

                        PDZ.Date = pdz.Date;
                        PDZ.NumberPDZ = pdz.NumberPDZ;

                        PDZ.CO_Conc = pdz.CO_Conc;
                        PDZ.CO2_Conc = pdz.CO2_Conc;
                        PDZ.NO_Conc = pdz.NO_Conc;
                        PDZ.NO2_Conc = pdz.NO2_Conc;
                        PDZ.NOx_Conc = pdz.NOx_Conc;
                        PDZ.SO2_Conc = pdz.SO2_Conc;
                        PDZ.Dust_Conc = pdz.Dust_Conc;
                        PDZ.CH4_Conc = pdz.CH4_Conc;
                        PDZ.H2S_Conc = pdz.H2S_Conc;
                        PDZ.Add_Conc_1 = pdz.Add_Conc_1;
                        PDZ.Add_Conc_2 = pdz.Add_Conc_2;
                        PDZ.Add_Conc_3 = pdz.Add_Conc_3;
                        PDZ.Add_Conc_4 = pdz.Add_Conc_4;
                        PDZ.Add_Conc_5 = pdz.Add_Conc_5;

                        PDZ.CO_Emis = pdz.CO_Emis;
                        PDZ.CO2_Emis = pdz.CO2_Emis;
                        PDZ.NO_Emis = pdz.NO_Emis;
                        PDZ.NO2_Emis = pdz.NO2_Emis;
                        PDZ.NOx_Emis = pdz.NOx_Emis;
                        PDZ.SO2_Emis = pdz.SO2_Emis;
                        PDZ.Dust_Emis = pdz.Dust_Emis;
                        PDZ.CH4_Emis = pdz.CH4_Emis;
                        PDZ.H2S_Emis = pdz.H2S_Emis;
                        PDZ.Add_Emis_1 = pdz.Add_Emis_1;
                        PDZ.Add_Emis_2 = pdz.Add_Emis_2;
                        PDZ.Add_Emis_3 = pdz.Add_Emis_3;
                        PDZ.Add_Emis_4 = pdz.Add_Emis_4;
                        PDZ.Add_Emis_5 = pdz.Add_Emis_5;

                        //PDZ[i + 1].Current = PDZs[i].Current;
                      
                 
            }












            

            foreach (TableReportDay tabletDay in tableReportDay)
            {
                //Концентрации
                sumTotal_20M.CO_Conc += tabletDay.CO_Conc;
                sumTotal_20M.CO2_Conc += tabletDay.CO2_Conc;
                sumTotal_20M.NOx_Conc += tabletDay.NOx_Conc;
                sumTotal_20M.SO2_Conc += tabletDay.SO2_Conc;
                sumTotal_20M.Dust_Conc += tabletDay.Dust_Conc;
                sumTotal_20M.CH4_Conc += tabletDay.CH4_Conc;
                sumTotal_20M.H2S_Conc += tabletDay.H2S_Conc;
                sumTotal_20M.Add_Conc_1 += tabletDay.Add_Conc_1;
                sumTotal_20M.Add_Conc_2 += tabletDay.Add_Conc_2;
                sumTotal_20M.Add_Conc_3 += tabletDay.Add_Conc_3;
                sumTotal_20M.Add_Conc_4 += tabletDay.Add_Conc_4;
                sumTotal_20M.Add_Conc_5 += tabletDay.Add_Conc_5;


                //Выбросы
                sumTotal_20M.CO_Emis += tabletDay.CO_Emis;
                sumTotal_20M.CO2_Emis += tabletDay.CO2_Emis;
                sumTotal_20M.NO_Emis += tabletDay.NO_Emis;
                sumTotal_20M.NOx_Emis += tabletDay.NOx_Emis;
                sumTotal_20M.NO2_Emis += tabletDay.NO2_Emis;
                sumTotal_20M.SO2_Emis += tabletDay.SO2_Emis;
                sumTotal_20M.Dust_Emis += tabletDay.Dust_Emis;
                sumTotal_20M.CH4_Emis += tabletDay.CH4_Emis;
                sumTotal_20M.H2S_Emis += tabletDay.H2S_Emis;
                sumTotal_20M.Add_Emis_1 += tabletDay.Add_Emis_1;
                sumTotal_20M.Add_Emis_2 += tabletDay.Add_Emis_2;
                sumTotal_20M.Add_Emis_3 += tabletDay.Add_Emis_3;
                sumTotal_20M.Add_Emis_4 += tabletDay.Add_Emis_4;
                sumTotal_20M.Add_Emis_5 += tabletDay.Add_Emis_5;


                //Концентрации процент
                avgTotal_20M.CO_Conc_Percent += tabletDay.CO_Conc_Percent;
                avgTotal_20M.CO2_Conc_Percent += tabletDay.CO2_Conc_Percent;
                avgTotal_20M.NOx_Conc_Percent += tabletDay.NOx_Conc_Percent;
                avgTotal_20M.SO2_Conc_Percent += tabletDay.SO2_Conc_Percent;
                avgTotal_20M.Dust_Conc_Percent += tabletDay.Dust_Conc_Percent;
                avgTotal_20M.CH4_Conc_Percent += tabletDay.CH4_Conc_Percent;
                avgTotal_20M.H2S_Conc_Percent += tabletDay.H2S_Conc_Percent;
                avgTotal_20M.Add_Conc_1_Percent += tabletDay.Add_Conc_1_Percent;
                avgTotal_20M.Add_Conc_2_Percent += tabletDay.Add_Conc_2_Percent;
                avgTotal_20M.Add_Conc_3_Percent += tabletDay.Add_Conc_3_Percent;
                avgTotal_20M.Add_Conc_4_Percent += tabletDay.Add_Conc_4_Percent;
                avgTotal_20M.Add_Conc_5_Percent += tabletDay.Add_Conc_5_Percent;


                //Выбросы процент
                avgTotal_20M.CO_Emis_Percent += tabletDay.CO_Emis_Percent;
                avgTotal_20M.CO2_Emis_Percent += tabletDay.CO2_Emis_Percent;
                avgTotal_20M.NO_Emis_Percent += tabletDay.NO_Emis_Percent;
                avgTotal_20M.NOx_Emis_Percent += tabletDay.NOx_Emis_Percent;
                avgTotal_20M.NO2_Emis_Percent += tabletDay.NO2_Emis_Percent;
                avgTotal_20M.SO2_Emis_Percent += tabletDay.SO2_Emis_Percent;
                avgTotal_20M.Dust_Emis_Percent += tabletDay.Dust_Emis_Percent;
                avgTotal_20M.CH4_Emis_Percent += tabletDay.CH4_Emis_Percent;
                avgTotal_20M.H2S_Emis_Percent += tabletDay.H2S_Emis_Percent;
                avgTotal_20M.Add_Emis_1_Percent += tabletDay.Add_Emis_1_Percent;
                avgTotal_20M.Add_Emis_2_Percent += tabletDay.Add_Emis_2_Percent;
                avgTotal_20M.Add_Emis_3_Percent += tabletDay.Add_Emis_3_Percent;
                avgTotal_20M.Add_Emis_4_Percent += tabletDay.Add_Emis_4_Percent;
                avgTotal_20M.Add_Emis_5_Percent += tabletDay.Add_Emis_5_Percent;


                //Параметры
                sumTotal_20M.Pressure += tabletDay.Pressure;
                sumTotal_20M.Temperature += tabletDay.Temperature;
                sumTotal_20M.Temperature_KIP += tabletDay.Temperature_KIP;
                sumTotal_20M.Temperature_NOx += tabletDay.Temperature_NOx;
                sumTotal_20M.Speed += tabletDay.Speed;
                sumTotal_20M.Flow += tabletDay.Flow;
                sumTotal_20M.O2_Dry += tabletDay.O2_Dry;
                sumTotal_20M.O2_Wet += tabletDay.O2_Wet;
                sumTotal_20M.H2O += tabletDay.H2O;

                //Считаем время простоя
                //if (tabletDay.Mode_ASK == 1.0)
                //{
                //    sumTotal_20M.Mode_ASK = sumTotal_20M.Mode_ASK + 1.0;
                //    avgTotal_20M.Mode_ASK = 1.0;
                //}



                //reportDay.PDZ_Fuel  
            }

            //Усредняем за 20 мин
            int counter = tableReportDay.Count;
            if (counter <= 1) counter = 1;

            //Концентрации
            avgTotal_20M.CO_Conc = Math.Round(sumTotal_20M.CO_Conc / counter, 3);
            avgTotal_20M.CO2_Conc = Math.Round(sumTotal_20M.CO2_Conc / counter, 3);
            avgTotal_20M.NO_Conc = Math.Round(sumTotal_20M.NO_Conc / counter, 3);
            avgTotal_20M.NO2_Conc = Math.Round(sumTotal_20M.NO2_Conc / counter, 3);
            avgTotal_20M.NOx_Conc = Math.Round(sumTotal_20M.NOx_Conc / counter, 3);
            avgTotal_20M.SO2_Conc = Math.Round(sumTotal_20M.SO2_Conc / counter, 3);
            avgTotal_20M.Dust_Conc = Math.Round(sumTotal_20M.Dust_Conc / counter, 3);
            avgTotal_20M.CH4_Conc = Math.Round(sumTotal_20M.CH4_Conc / counter, 3);
            avgTotal_20M.H2S_Conc = Math.Round(sumTotal_20M.H2S_Conc / counter, 3);
            avgTotal_20M.Add_Conc_1 = Math.Round(sumTotal_20M.Add_Conc_1 / counter, 3);
            avgTotal_20M.Add_Conc_2 = Math.Round(sumTotal_20M.Add_Conc_2 / counter, 3);
            avgTotal_20M.Add_Conc_3 = Math.Round(sumTotal_20M.Add_Conc_3 / counter, 3);
            avgTotal_20M.Add_Conc_4 = Math.Round(sumTotal_20M.Add_Conc_4 / counter, 3);
            avgTotal_20M.Add_Conc_5 = Math.Round(sumTotal_20M.Add_Conc_5 / counter, 3);




            //Выбросы
            avgTotal_20M.CO_Emis = Math.Round(sumTotal_20M.CO_Emis / counter, 3);
            avgTotal_20M.CO2_Emis = Math.Round(sumTotal_20M.CO2_Emis / counter, 3);
            avgTotal_20M.NO_Emis = Math.Round(sumTotal_20M.NO_Emis / counter, 3);
            avgTotal_20M.NO2_Emis = Math.Round(sumTotal_20M.NO2_Emis / counter, 3);
            avgTotal_20M.NOx_Emis = Math.Round(sumTotal_20M.NOx_Emis / counter, 3);
            avgTotal_20M.SO2_Emis = Math.Round(sumTotal_20M.SO2_Emis / counter, 3);
            avgTotal_20M.Dust_Emis = Math.Round(sumTotal_20M.Dust_Emis / counter, 3);
            avgTotal_20M.CH4_Emis = Math.Round(sumTotal_20M.CH4_Emis / counter, 3);
            avgTotal_20M.H2S_Emis = Math.Round(sumTotal_20M.H2S_Emis / counter, 3);
            avgTotal_20M.Add_Emis_1 = Math.Round(sumTotal_20M.Add_Emis_1 / counter, 3);
            avgTotal_20M.Add_Emis_2 = Math.Round(sumTotal_20M.Add_Emis_2 / counter, 3);
            avgTotal_20M.Add_Emis_3 = Math.Round(sumTotal_20M.Add_Emis_3 / counter, 3);
            avgTotal_20M.Add_Emis_4 = Math.Round(sumTotal_20M.Add_Emis_4 / counter, 3);
            avgTotal_20M.Add_Emis_5 = Math.Round(sumTotal_20M.Add_Emis_5 / counter, 3);
      


            //Параметры
            avgTotal_20M.Pressure = Math.Round(sumTotal_20M.Pressure / counter, 3);
            avgTotal_20M.Temperature = Math.Round(sumTotal_20M.Temperature / counter, 3);
            avgTotal_20M.Temperature_KIP = Math.Round(sumTotal_20M.Temperature_KIP / counter, 3);
            avgTotal_20M.Temperature_NOx = Math.Round(sumTotal_20M.Temperature_NOx / counter, 3);
            avgTotal_20M.Speed = Math.Round(sumTotal_20M.Speed / counter, 3);
            avgTotal_20M.Flow = Math.Round(sumTotal_20M.Flow / counter, 3);
            avgTotal_20M.O2_Dry = Math.Round(sumTotal_20M.O2_Dry / counter, 3);
            avgTotal_20M.O2_Wet = Math.Round(sumTotal_20M.O2_Wet / counter, 3);
            avgTotal_20M.H2O = Math.Round(sumTotal_20M.H2O / counter, 3);

            //Считаем время простоя
            //sumTotal_20M.Mode_ASK = Math.Round(sumTotal_20M.Mode_ASK / 3, 3);


            //Сумма за сутки
            sumTotal_20M.CO_Emis = Math.Round(sumTotal_20M.CO_Emis / 1000, 3);
            sumTotal_20M.CO2_Emis = Math.Round(sumTotal_20M.CO2_Emis / 1000, 3);
            sumTotal_20M.NO_Emis = Math.Round(sumTotal_20M.NO_Emis / 1000, 3);
            sumTotal_20M.NOx_Emis = Math.Round(sumTotal_20M.NOx_Emis / 1000, 3);
            sumTotal_20M.NO2_Emis = Math.Round(sumTotal_20M.NO2_Emis / 1000, 3);
            sumTotal_20M.SO2_Emis = Math.Round(sumTotal_20M.SO2_Emis / 1000, 3);
            sumTotal_20M.Dust_Emis = Math.Round(sumTotal_20M.Dust_Emis / 1000, 3);
            sumTotal_20M.CH4_Emis = Math.Round(sumTotal_20M.CH4_Emis / 1000, 3);
            sumTotal_20M.H2S_Emis = Math.Round(sumTotal_20M.H2S_Emis / 1000, 3);
            sumTotal_20M.Add_Emis_1 = Math.Round(sumTotal_20M.Add_Emis_1 / 1000, 3);
            sumTotal_20M.Add_Emis_2 = Math.Round(sumTotal_20M.Add_Emis_2 / 1000, 3);
            sumTotal_20M.Add_Emis_3 = Math.Round(sumTotal_20M.Add_Emis_3 / 1000, 3);
            sumTotal_20M.Add_Emis_4 = Math.Round(sumTotal_20M.Add_Emis_4 / 1000, 3);
            sumTotal_20M.Add_Emis_5 = Math.Round(sumTotal_20M.Add_Emis_5 / 1000, 3);

            sumTotal_20M.CO_Conc = 0.0;
            sumTotal_20M.CO2_Conc = 0.0;
            sumTotal_20M.NO_Conc = 0.0;
            sumTotal_20M.NOx_Conc = 0.0;
            sumTotal_20M.NO2_Conc = 0.0;
            sumTotal_20M.SO2_Conc = 0.0;
            sumTotal_20M.Dust_Conc = 0.0;
            sumTotal_20M.CH4_Conc = 0.0;
            sumTotal_20M.H2S_Conc = 0.0;
            sumTotal_20M.Add_Conc_1 = 0.0;
            sumTotal_20M.Add_Conc_2 = 0.0;
            sumTotal_20M.Add_Conc_3 = 0.0;
            sumTotal_20M.Add_Conc_4 = 0.0;
            sumTotal_20M.Add_Conc_5 = 0.0;

        }
    }
}
