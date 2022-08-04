using ASK.Controllers.Report;
using ASK.Data;
using ASK.Data.Services;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.Controllers.Add
{
    public class ReportDay
    {
        

        public List<TableReportDay> tableReportDay { get; set; }    //Значение 20-минуток

        public PDZ_Active PDZ { get; set; }                         //Текущее значение ПДК


        public TableReportDay avgTotal_20M{ get; set; }                  //Подвал, среднее занчение и сумма концентраций(+парамеры) и выбросов соответственно
        public TableReportDay sumTotal_20M { get; set; }                 //Подвал, сумма выбросов
        public TableReportDay durationTotal_20M { get; set; }                 //Подвал, сумма выбросов




        public ReportDay(DateTime date)
        {
            tableReportDay = new List<TableReportDay>();


            //Date = date;

            //PDZ_Day = new PDZ_Active();

            //PDZ_Fuel = 0;


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Вытаскиваем ПДЗ на текущие сутки
                List<PDZ> PDZs = new List<PDZ>();
                PDZ_Service pdz_Service = new PDZ_Service(db);
                PDZs = pdz_Service.Get_DayAll_PDZ(date).OrderBy(w => w.NumberPDZ).ToList(); //Проверить правильность сортировки!

                PDZ = new PDZ_Active();

                for (int i = 0; i < PDZs.Count; i++)
                {
                    if (PDZs[i].Current == true)
                    {

                        PDZ.Date = PDZs[i].Date;
                        PDZ.NumberPDZ = PDZs[i].NumberPDZ;

                        PDZ.CO_Conc = PDZs[i].CO_Conc;
                        PDZ.CO2_Conc = PDZs[i].CO2_Conc;
                        PDZ.NO_Conc = PDZs[i].NO_Conc;
                        PDZ.NO2_Conc = PDZs[i].NO2_Conc;
                        PDZ.NOx_Conc = PDZs[i].NOx_Conc;
                        PDZ.SO2_Conc = PDZs[i].SO2_Conc;
                        PDZ.Dust_Conc = PDZs[i].Dust_Conc;
                        PDZ.CH4_Conc = PDZs[i].CH4_Conc;
                        PDZ.H2S_Conc = PDZs[i].H2S_Conc;
                        PDZ.Add_Conc_1 = PDZs[i].Add_Conc_1;
                        PDZ.Add_Conc_2 = PDZs[i].Add_Conc_2;
                        PDZ.Add_Conc_3 = PDZs[i].Add_Conc_3;
                        PDZ.Add_Conc_4 = PDZs[i].Add_Conc_4;
                        PDZ.Add_Conc_5 = PDZs[i].Add_Conc_5;

                        PDZ.CO_Emis = PDZs[i].CO_Emis;
                        PDZ.CO2_Emis = PDZs[i].CO2_Emis;
                        PDZ.NO_Emis = PDZs[i].NO_Emis;
                        PDZ.NO2_Emis = PDZs[i].NO2_Emis;
                        PDZ.NOx_Emis = PDZs[i].NOx_Emis;
                        PDZ.SO2_Emis = PDZs[i].SO2_Emis;
                        PDZ.Dust_Emis = PDZs[i].Dust_Emis;
                        PDZ.CH4_Emis = PDZs[i].CH4_Emis;
                        PDZ.H2S_Emis = PDZs[i].H2S_Emis;
                        PDZ.Add_Emis_1 = PDZs[i].Add_Emis_1;
                        PDZ.Add_Emis_2 = PDZs[i].Add_Emis_2;
                        PDZ.Add_Emis_3 = PDZs[i].Add_Emis_3;
                        PDZ.Add_Emis_4 = PDZs[i].Add_Emis_4;
                        PDZ.Add_Emis_5 = PDZs[i].Add_Emis_5;

                        //PDZ[i + 1].Current = PDZs[i].Current;
                        break;
                    }
                }



                //Таблица концентраций
                List<AVG_20_MINUTES> avg_20_minutes = new List<AVG_20_MINUTES>();                         //Таблица 20-минуток из БД - за сутки
                AVG_20_MINUTES_Service avg_20_MINUTES_Service = new AVG_20_MINUTES_Service(db);
                avg_20_minutes = avg_20_MINUTES_Service.Get_DayAll_AVG_20_MINUTES(date);

                durationTotal_20M = new TableReportDay();


                tableReportDay.Clear();

                for (int i = 0; i < avg_20_minutes.Count; i++)
                {
                    tableReportDay.Add(new TableReportDay());


                    tableReportDay[i].Date = avg_20_minutes[i].Date;

                    tableReportDay[i].Mode_ASK = avg_20_minutes[i].Mode_ASK;


                    tableReportDay[i].PDZ_Fuel = avg_20_minutes[i].PDZ_Fuel; //Обязательно должен идти первее подсчёта процентов превышений

                    //Концентрации
                    tableReportDay[i].CO_Conc = avg_20_minutes[i].Conc_CO;
                    tableReportDay[i].CO2_Conc = avg_20_minutes[i].Conc_CO2;
                    tableReportDay[i].NO_Conc = avg_20_minutes[i].Conc_NO;
                    tableReportDay[i].NO2_Conc = avg_20_minutes[i].Conc_NO2;
                    tableReportDay[i].NOx_Conc = avg_20_minutes[i].Conc_NOx;
                    tableReportDay[i].SO2_Conc = avg_20_minutes[i].Conc_SO2;
                    tableReportDay[i].Dust_Conc = avg_20_minutes[i].Conc_Dust;
                    tableReportDay[i].CH4_Conc = avg_20_minutes[i].Conc_CH4;
                    tableReportDay[i].H2S_Conc = avg_20_minutes[i].Conc_H2S;
                    tableReportDay[i].Add_Conc_1 = avg_20_minutes[i].Conc_D1;
                    tableReportDay[i].Add_Conc_2 = avg_20_minutes[i].Conc_D2;
                    tableReportDay[i].Add_Conc_3 = avg_20_minutes[i].Conc_D3;
                    tableReportDay[i].Add_Conc_4 = avg_20_minutes[i].Conc_D4;
                    tableReportDay[i].Add_Conc_5 = avg_20_minutes[i].Conc_D5;
                    //Процент превышений Концентраций
                    if (PDZ.CO_Conc != 0.0 && tableReportDay[i].CO_Conc > PDZ.CO_Conc) { tableReportDay[i].CO_Conc_Percent = Math.Round((tableReportDay[i].CO_Conc / (PDZ.CO_Conc / 100.0)) - 100, 1); durationTotal_20M.CO_Conc_Percent++; }
                    if (PDZ.CO2_Conc != 0.0 && tableReportDay[i].CO2_Conc > PDZ.CO2_Conc) { tableReportDay[i].CO2_Conc_Percent = Math.Round((tableReportDay[i].CO2_Conc / (PDZ.CO2_Conc / 100.0)) - 100, 1); durationTotal_20M.CO2_Conc_Percent++; }
                    if (PDZ.NO_Conc != 0.0 && tableReportDay[i].NO_Conc > PDZ.NO_Conc) { tableReportDay[i].NO_Conc_Percent = Math.Round((tableReportDay[i].NO_Conc / (PDZ.NO_Conc / 100.0)) - 100, 1); durationTotal_20M.NO_Conc_Percent++; }
                    if (PDZ.NO2_Conc != 0.0 && tableReportDay[i].NO2_Conc > PDZ.NO2_Conc) { tableReportDay[i].NO2_Conc_Percent = Math.Round((tableReportDay[i].NO2_Conc / (PDZ.NO2_Conc / 100.0)) - 100, 1); durationTotal_20M.NO2_Conc_Percent++; }
                    if (PDZ.NOx_Conc != 0.0 && tableReportDay[i].NOx_Conc > PDZ.NOx_Conc) { tableReportDay[i].NOx_Conc_Percent = Math.Round((tableReportDay[i].NOx_Conc / (PDZ.NOx_Conc / 100.0)) - 100, 1); durationTotal_20M.NOx_Conc_Percent++; }
                    if (PDZ.SO2_Conc != 0.0 && tableReportDay[i].SO2_Conc > PDZ.SO2_Conc) { tableReportDay[i].SO2_Conc_Percent = Math.Round((tableReportDay[i].SO2_Conc / (PDZ.SO2_Conc / 100.0)) - 100, 1); durationTotal_20M.SO2_Conc_Percent++; }
                    if (PDZ.Dust_Conc != 0.0 && tableReportDay[i].Dust_Conc > PDZ.Dust_Conc) { tableReportDay[i].Dust_Conc_Percent = Math.Round((tableReportDay[i].Dust_Conc / (PDZ.Dust_Conc / 100.0)) - 100, 1); durationTotal_20M.Dust_Conc_Percent++; }
                    if (PDZ.CH4_Conc != 0.0 && tableReportDay[i].CH4_Conc > PDZ.CH4_Conc) { tableReportDay[i].CH4_Conc_Percent = Math.Round((tableReportDay[i].CH4_Conc / (PDZ.CH4_Conc / 100.0)) - 100, 1); durationTotal_20M.CH4_Conc_Percent++; }
                    if (PDZ.H2S_Conc != 0.0 && tableReportDay[i].H2S_Conc > PDZ.H2S_Conc) { tableReportDay[i].H2S_Conc_Percent = Math.Round((tableReportDay[i].H2S_Conc / (PDZ.H2S_Conc / 100.0)) - 100, 1); durationTotal_20M.H2S_Conc_Percent++; }
                    if (PDZ.Add_Conc_1 != 0.0 && tableReportDay[i].Add_Conc_1 > PDZ.Add_Conc_1) { tableReportDay[i].Add_Conc_1_Percent = Math.Round((tableReportDay[i].Add_Conc_1 / (PDZ.Add_Conc_1 / 100.0)) - 100, 1); durationTotal_20M.Add_Conc_1_Percent++; }
                    if (PDZ.Add_Conc_2 != 0.0 && tableReportDay[i].Add_Conc_2 > PDZ.Add_Conc_2) { tableReportDay[i].Add_Conc_2_Percent = Math.Round((tableReportDay[i].Add_Conc_2 / (PDZ.Add_Conc_2 / 100.0)) - 100, 1); durationTotal_20M.Add_Conc_2_Percent++; }
                    if (PDZ.Add_Conc_3 != 0.0 && tableReportDay[i].Add_Conc_3 > PDZ.Add_Conc_3) { tableReportDay[i].Add_Conc_3_Percent = Math.Round((tableReportDay[i].Add_Conc_3 / (PDZ.Add_Conc_3 / 100.0)) - 100, 1); durationTotal_20M.Add_Conc_3_Percent++; }
                    if (PDZ.Add_Conc_4 != 0.0 && tableReportDay[i].Add_Conc_4 > PDZ.Add_Conc_4) { tableReportDay[i].Add_Conc_4_Percent = Math.Round((tableReportDay[i].Add_Conc_4 / (PDZ.Add_Conc_4 / 100.0)) - 100, 1); durationTotal_20M.Add_Conc_4_Percent++; }
                    if (PDZ.Add_Conc_5 != 0.0 && tableReportDay[i].Add_Conc_5 > PDZ.Add_Conc_5) { tableReportDay[i].Add_Conc_5_Percent = Math.Round((tableReportDay[i].Add_Conc_5 / (PDZ.Add_Conc_5 / 100.0)) - 100, 1); durationTotal_20M.Add_Conc_5_Percent++; }






                    //Выбросы
                    tableReportDay[i].CO_Emis = avg_20_minutes[i].Emis_CO;
                    tableReportDay[i].CO2_Emis = avg_20_minutes[i].Emis_CO2;
                    tableReportDay[i].NO_Emis = avg_20_minutes[i].Emis_NO;
                    tableReportDay[i].NO2_Emis = avg_20_minutes[i].Emis_NO2;
                    tableReportDay[i].NOx_Emis = avg_20_minutes[i].Emis_NOx;
                    tableReportDay[i].SO2_Emis = avg_20_minutes[i].Emis_SO2;
                    tableReportDay[i].Dust_Emis = avg_20_minutes[i].Emis_Dust;
                    tableReportDay[i].CH4_Emis = avg_20_minutes[i].Emis_CH4;
                    tableReportDay[i].H2S_Conc = avg_20_minutes[i].Emis_H2S;
                    tableReportDay[i].Add_Emis_1 = avg_20_minutes[i].Emis_D1;
                    tableReportDay[i].Add_Emis_2 = avg_20_minutes[i].Emis_D2;
                    tableReportDay[i].Add_Emis_3 = avg_20_minutes[i].Emis_D3;
                    tableReportDay[i].Add_Emis_4 = avg_20_minutes[i].Emis_D4;
                    tableReportDay[i].Add_Emis_5 = avg_20_minutes[i].Emis_D5;
                    //Процент превышений Выбросы
                    if (PDZ.CO_Emis != 0.0 && tableReportDay[i].CO_Emis > PDZ.CO_Emis) { tableReportDay[i].CO_Emis_Percent = Math.Round((tableReportDay[i].CO_Emis / (PDZ.CO_Emis / 100.0)) - 100, 1); durationTotal_20M.CO_Emis_Percent++; }
                    if (PDZ.CO2_Emis != 0.0 && tableReportDay[i].CO2_Emis > PDZ.CO2_Emis) { tableReportDay[i].CO2_Emis_Percent = Math.Round((tableReportDay[i].CO2_Emis / (PDZ.CO2_Emis / 100.0)) - 100, 1); durationTotal_20M.CO2_Emis_Percent++; }
                    if (PDZ.NO_Emis != 0.0 && tableReportDay[i].NO_Emis > PDZ.NO_Emis) { tableReportDay[i].NO_Emis_Percent = Math.Round((tableReportDay[i].NO_Emis / (PDZ.NO_Emis / 100.0)) - 100, 1); durationTotal_20M.NO_Emis_Percent++; }
                    if (PDZ.NO2_Emis != 0.0 && tableReportDay[i].NO2_Emis > PDZ.NO2_Emis) { tableReportDay[i].NO2_Emis_Percent = Math.Round((tableReportDay[i].NO2_Emis / (PDZ.NO2_Emis / 100.0)) - 100, 1); durationTotal_20M.NO2_Emis_Percent++; }
                    if (PDZ.NOx_Emis != 0.0 && tableReportDay[i].NOx_Emis > PDZ.NOx_Emis) { tableReportDay[i].NOx_Emis_Percent = Math.Round((tableReportDay[i].NOx_Emis / (PDZ.NOx_Emis / 100.0)) - 100, 1); durationTotal_20M.NOx_Emis_Percent++; }
                    if (PDZ.SO2_Emis != 0.0 && tableReportDay[i].SO2_Emis > PDZ.SO2_Emis) { tableReportDay[i].SO2_Emis_Percent = Math.Round((tableReportDay[i].SO2_Emis / (PDZ.SO2_Emis / 100.0)) - 100, 1); durationTotal_20M.SO2_Emis_Percent++; }
                    if (PDZ.Dust_Emis != 0.0 && tableReportDay[i].Dust_Emis > PDZ.Dust_Emis) { tableReportDay[i].Dust_Emis_Percent = Math.Round((tableReportDay[i].Dust_Emis / (PDZ.Dust_Emis / 100.0)) - 100, 1); durationTotal_20M.Dust_Emis_Percent++; }
                    if (PDZ.CH4_Emis != 0.0 && tableReportDay[i].CH4_Emis > PDZ.CH4_Emis) { tableReportDay[i].CH4_Emis_Percent = Math.Round((tableReportDay[i].CH4_Emis / (PDZ.CH4_Emis / 100.0)) - 100, 1); durationTotal_20M.CH4_Emis_Percent++; }
                    if (PDZ.H2S_Emis != 0.0 && tableReportDay[i].H2S_Emis > PDZ.H2S_Emis) { tableReportDay[i].H2S_Emis_Percent = Math.Round((tableReportDay[i].H2S_Emis / (PDZ.H2S_Emis / 100.0)) - 100, 1); durationTotal_20M.H2S_Emis_Percent++; }
                    if (PDZ.Add_Emis_1 != 0.0 && tableReportDay[i].Add_Emis_1 > PDZ.Add_Emis_1) { tableReportDay[i].Add_Emis_1_Percent = Math.Round((tableReportDay[i].Add_Emis_1 / (PDZ.Add_Emis_1 / 100.0)) - 100, 1); durationTotal_20M.Add_Emis_1_Percent++; }
                    if (PDZ.Add_Emis_2 != 0.0 && tableReportDay[i].Add_Emis_2 > PDZ.Add_Emis_2) { tableReportDay[i].Add_Emis_2_Percent = Math.Round((tableReportDay[i].Add_Emis_2 / (PDZ.Add_Emis_2 / 100.0)) - 100, 1); durationTotal_20M.Add_Emis_2_Percent++; }
                    if (PDZ.Add_Emis_3 != 0.0 && tableReportDay[i].Add_Emis_3 > PDZ.Add_Emis_3) { tableReportDay[i].Add_Emis_3_Percent = Math.Round((tableReportDay[i].Add_Emis_3 / (PDZ.Add_Emis_3 / 100.0)) - 100, 1); durationTotal_20M.Add_Emis_3_Percent++; }
                    if (PDZ.Add_Emis_4 != 0.0 && tableReportDay[i].Add_Emis_4 > PDZ.Add_Emis_4) { tableReportDay[i].Add_Emis_4_Percent = Math.Round((tableReportDay[i].Add_Emis_4 / (PDZ.Add_Emis_4 / 100.0)) - 100, 1); durationTotal_20M.Add_Emis_4_Percent++; }
                    if (PDZ.Add_Emis_5 != 0.0 && tableReportDay[i].Add_Emis_5 > PDZ.Add_Emis_5) { tableReportDay[i].Add_Emis_5_Percent = Math.Round((tableReportDay[i].Add_Emis_5 / (PDZ.Add_Emis_5 / 100.0)) - 100, 1); durationTotal_20M.Add_Emis_5_Percent++; }


                    //Параметры
                    tableReportDay[i].Pressure = avg_20_minutes[i].Pressure;
                    tableReportDay[i].Temperature = avg_20_minutes[i].Temperature;
                    tableReportDay[i].Temperature_KIP = avg_20_minutes[i].Temperature_KIP;
                    tableReportDay[i].Temperature_NOx = avg_20_minutes[i].Temperature_NOx;
                    tableReportDay[i].Speed = avg_20_minutes[i].Speed;
                    tableReportDay[i].Flow = avg_20_minutes[i].Flow;
                    tableReportDay[i].O2_Dry = avg_20_minutes[i].O2_Dry;
                    tableReportDay[i].O2_Wet = avg_20_minutes[i].O2_Wet;
                    tableReportDay[i].H2O = avg_20_minutes[i].H2O;
                }
            }


            tableReportDay = tableReportDay.OrderBy(x => x.Date).ToList();

            //Высчитывает среднее и суммараное значение концентрацйи, выбросов и параметров
            avgTotal_20M = new TableReportDay();
            sumTotal_20M = new TableReportDay();

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
                if (tabletDay.Mode_ASK == 1.0)
                {
                    sumTotal_20M.Mode_ASK = sumTotal_20M.Mode_ASK + 1.0;
                    avgTotal_20M.Mode_ASK = 1.0;
                }
                
                

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
            //Процент превышений Концентраций
            if (PDZ.CO_Conc != 0.0 && avgTotal_20M.CO_Conc > PDZ.CO_Conc) avgTotal_20M.CO_Conc_Percent = Math.Round((avgTotal_20M.CO_Conc / (PDZ.CO_Conc / 100.0)) - 100,1);
            if (PDZ.CO2_Conc != 0.0 && avgTotal_20M.CO2_Conc > PDZ.CO2_Conc) avgTotal_20M.CO2_Conc_Percent = Math.Round((avgTotal_20M.CO2_Conc / (PDZ.CO2_Conc / 100.0)) - 100,1);
            if (PDZ.NO_Conc != 0.0 && avgTotal_20M.NO_Conc > PDZ.NO_Conc) avgTotal_20M.NO_Conc_Percent = Math.Round((avgTotal_20M.NO_Conc / (PDZ.NO_Conc / 100.0)) - 100,1);
            if (PDZ.NO2_Conc != 0.0 && avgTotal_20M.NO2_Conc > PDZ.NO2_Conc) avgTotal_20M.NO2_Conc_Percent = Math.Round((avgTotal_20M.NO2_Conc / (PDZ.NO2_Conc / 100.0)) - 100,1);
            if (PDZ.NOx_Conc != 0.0 && avgTotal_20M.NOx_Conc > PDZ.NOx_Conc) avgTotal_20M.NOx_Conc_Percent = Math.Round((avgTotal_20M.NOx_Conc / (PDZ.NOx_Conc / 100.0)) - 100,1);
            if (PDZ.SO2_Conc != 0.0 && avgTotal_20M.SO2_Conc > PDZ.SO2_Conc) avgTotal_20M.SO2_Conc_Percent = Math.Round((avgTotal_20M.SO2_Conc / (PDZ.SO2_Conc / 100.0)) - 100,1);
            if (PDZ.Dust_Conc != 0.0 && avgTotal_20M.Dust_Conc > PDZ.Dust_Conc) avgTotal_20M.Dust_Conc_Percent = Math.Round((avgTotal_20M.Dust_Conc / (PDZ.Dust_Conc / 100.0)) - 100,1);
            if (PDZ.CH4_Conc != 0.0 && avgTotal_20M.CH4_Conc > PDZ.CH4_Conc) avgTotal_20M.CH4_Conc_Percent = Math.Round((avgTotal_20M.CH4_Conc / (PDZ.CH4_Conc / 100.0)) - 100,1);
            if (PDZ.H2S_Conc != 0.0 && avgTotal_20M.H2S_Conc > PDZ.H2S_Conc) avgTotal_20M.H2S_Conc_Percent = Math.Round((avgTotal_20M.H2S_Conc / (PDZ.H2S_Conc / 100.0)) - 100,1);
            if (PDZ.Add_Conc_1 != 0.0 && avgTotal_20M.Add_Conc_1 > PDZ.Add_Conc_1) avgTotal_20M.Add_Conc_1_Percent = Math.Round((avgTotal_20M.Add_Conc_1 / (PDZ.Add_Conc_1 / 100.0)) - 100,1);
            if (PDZ.Add_Conc_2 != 0.0 && avgTotal_20M.Add_Conc_2 > PDZ.Add_Conc_2) avgTotal_20M.Add_Conc_2_Percent = Math.Round((avgTotal_20M.Add_Conc_2 / (PDZ.Add_Conc_2 / 100.0)) - 100,1);
            if (PDZ.Add_Conc_3 != 0.0 && avgTotal_20M.Add_Conc_3 > PDZ.Add_Conc_3) avgTotal_20M.Add_Conc_3_Percent = Math.Round((avgTotal_20M.Add_Conc_3 / (PDZ.Add_Conc_3 / 100.0)) - 100,1);
            if (PDZ.Add_Conc_4 != 0.0 && avgTotal_20M.Add_Conc_4 > PDZ.Add_Conc_4) avgTotal_20M.Add_Conc_4_Percent = Math.Round((avgTotal_20M.Add_Conc_4 / (PDZ.Add_Conc_4 / 100.0)) - 100,1);
            if (PDZ.Add_Conc_5 != 0.0 && avgTotal_20M.Add_Conc_5 > PDZ.Add_Conc_5) avgTotal_20M.Add_Conc_5_Percent = Math.Round((avgTotal_20M.Add_Conc_5 / (PDZ.Add_Conc_5 / 100.0)) - 100,1);




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
            //Процент превышений Выбросов
            if (PDZ.CO_Emis != 0.0 && avgTotal_20M.CO_Emis > PDZ.CO_Emis) avgTotal_20M.CO_Emis_Percent = Math.Round((avgTotal_20M.CO_Emis / (PDZ.CO_Emis / 100.0)) - 100,1);
            if (PDZ.CO2_Emis != 0.0 && avgTotal_20M.CO2_Emis > PDZ.CO2_Emis) avgTotal_20M.CO2_Emis_Percent = Math.Round((avgTotal_20M.CO2_Emis / (PDZ.CO2_Emis / 100.0)) - 100,1);
            if (PDZ.NO_Emis != 0.0 && avgTotal_20M.NO_Emis > PDZ.NO_Emis) avgTotal_20M.NO_Emis_Percent = Math.Round((avgTotal_20M.NO_Emis / (PDZ.NO_Emis / 100.0)) - 100,1);
            if (PDZ.NO2_Emis != 0.0 && avgTotal_20M.NO2_Emis > PDZ.NO2_Emis) avgTotal_20M.NO2_Emis_Percent = Math.Round((avgTotal_20M.NO2_Emis / (PDZ.NO2_Emis / 100.0)) - 100,1);
            if (PDZ.NOx_Emis != 0.0 && avgTotal_20M.NOx_Emis > PDZ.NOx_Emis) avgTotal_20M.NOx_Emis_Percent = Math.Round((avgTotal_20M.NOx_Emis / (PDZ.NOx_Emis / 100.0)) - 100,1);
            if (PDZ.SO2_Emis != 0.0 && avgTotal_20M.SO2_Emis > PDZ.SO2_Emis) avgTotal_20M.SO2_Emis_Percent = Math.Round((avgTotal_20M.SO2_Emis / (PDZ.SO2_Emis / 100.0)) - 100,1);
            if (PDZ.Dust_Emis != 0.0 && avgTotal_20M.Dust_Emis > PDZ.Dust_Emis) avgTotal_20M.Dust_Emis_Percent = Math.Round((avgTotal_20M.Dust_Emis / (PDZ.Dust_Emis / 100.0)) - 100,1);
            if (PDZ.CH4_Emis != 0.0 && avgTotal_20M.CH4_Emis > PDZ.CH4_Emis) avgTotal_20M.CH4_Emis_Percent = Math.Round((avgTotal_20M.CH4_Emis / (PDZ.CH4_Emis / 100.0)) - 100,1);
            if (PDZ.H2S_Emis != 0.0 && avgTotal_20M.H2S_Emis > PDZ.H2S_Emis) avgTotal_20M.H2S_Emis_Percent = Math.Round((avgTotal_20M.H2S_Emis / (PDZ.H2S_Emis / 100.0)) - 100,1);
            if (PDZ.Add_Emis_1 != 0.0 && avgTotal_20M.Add_Emis_1 > PDZ.Add_Emis_1) avgTotal_20M.Add_Emis_1_Percent = Math.Round((avgTotal_20M.Add_Emis_1 / (PDZ.Add_Emis_1 / 100.0)) - 100,1);
            if (PDZ.Add_Emis_2 != 0.0 && avgTotal_20M.Add_Emis_2 > PDZ.Add_Emis_2) avgTotal_20M.Add_Emis_2_Percent = Math.Round((avgTotal_20M.Add_Emis_2 / (PDZ.Add_Emis_2 / 100.0)) - 100,1);
            if (PDZ.Add_Emis_3 != 0.0 && avgTotal_20M.Add_Emis_3 > PDZ.Add_Emis_3) avgTotal_20M.Add_Emis_3_Percent = Math.Round((avgTotal_20M.Add_Emis_3 / (PDZ.Add_Emis_3 / 100.0)) - 100,1);
            if (PDZ.Add_Emis_4 != 0.0 && avgTotal_20M.Add_Emis_4 > PDZ.Add_Emis_4) avgTotal_20M.Add_Emis_4_Percent = Math.Round((avgTotal_20M.Add_Emis_4 / (PDZ.Add_Emis_4 / 100.0)) - 100,1);
            if (PDZ.Add_Emis_5 != 0.0 && avgTotal_20M.Add_Emis_5 > PDZ.Add_Emis_5) avgTotal_20M.Add_Emis_5_Percent = Math.Round((avgTotal_20M.Add_Emis_5 / (PDZ.Add_Emis_5 / 100.0)) - 100,1);


            


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
            sumTotal_20M.Mode_ASK = Math.Round(sumTotal_20M.Mode_ASK /3, 3);


            //Сумма за сутки
            sumTotal_20M.CO_Emis = Math.Round(sumTotal_20M.CO_Emis * 1.2, 3);
            sumTotal_20M.CO2_Emis = Math.Round(sumTotal_20M.CO2_Emis * 1.2, 3);
            sumTotal_20M.NO_Emis = Math.Round(sumTotal_20M.NO_Emis * 1.2, 3);
            sumTotal_20M.NOx_Emis = Math.Round(sumTotal_20M.NOx_Emis * 1.2, 3);
            sumTotal_20M.NO2_Emis = Math.Round(sumTotal_20M.NO2_Emis * 1.2, 3);
            sumTotal_20M.SO2_Emis = Math.Round(sumTotal_20M.SO2_Emis * 1.2, 3);
            sumTotal_20M.Dust_Emis = Math.Round(sumTotal_20M.Dust_Emis * 1.2, 3);
            sumTotal_20M.CH4_Emis = Math.Round(sumTotal_20M.CH4_Emis * 1.2, 3);
            sumTotal_20M.H2S_Emis = Math.Round(sumTotal_20M.H2S_Emis * 1.2, 3);
            sumTotal_20M.Add_Emis_1 = Math.Round(sumTotal_20M.Add_Emis_1 * 1.2, 3);
            sumTotal_20M.Add_Emis_2 = Math.Round(sumTotal_20M.Add_Emis_2 * 1.2, 3);
            sumTotal_20M.Add_Emis_3 = Math.Round(sumTotal_20M.Add_Emis_3 * 1.2, 3);
            sumTotal_20M.Add_Emis_4 = Math.Round(sumTotal_20M.Add_Emis_4 * 1.2, 3);
            sumTotal_20M.Add_Emis_5 = Math.Round(sumTotal_20M.Add_Emis_5 * 1.2, 3);
            
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


            //Считаем длительность превышений
            durationTotal_20M.CO_Conc_Percent = Math.Round(durationTotal_20M.CO_Conc_Percent / 3 ,1);
            durationTotal_20M.CO2_Conc_Percent = Math.Round(durationTotal_20M.CO2_Conc_Percent / 3, 1);
            durationTotal_20M.NO_Conc_Percent = Math.Round(durationTotal_20M.NO_Conc_Percent / 3, 1);
            durationTotal_20M.NO2_Conc_Percent = Math.Round(durationTotal_20M.NO2_Conc_Percent / 3, 1);
            durationTotal_20M.NOx_Conc_Percent = Math.Round(durationTotal_20M.NOx_Conc_Percent / 3, 1);
            durationTotal_20M.SO2_Conc_Percent = Math.Round(durationTotal_20M.SO2_Conc_Percent / 3, 1);
            durationTotal_20M.Dust_Conc_Percent = Math.Round(durationTotal_20M.Dust_Conc_Percent / 3, 1);
            durationTotal_20M.CH4_Conc_Percent = Math.Round(durationTotal_20M.CH4_Conc_Percent / 3, 1);
            durationTotal_20M.H2S_Conc_Percent = Math.Round(durationTotal_20M.H2S_Conc_Percent / 3, 1);
            durationTotal_20M.Add_Conc_1_Percent = Math.Round(durationTotal_20M.Add_Conc_1_Percent / 3, 1);
            durationTotal_20M.Add_Conc_2_Percent = Math.Round(durationTotal_20M.Add_Conc_2_Percent / 3, 1);
            durationTotal_20M.Add_Conc_3_Percent = Math.Round(durationTotal_20M.Add_Conc_3_Percent / 3, 1);
            durationTotal_20M.Add_Conc_4_Percent = Math.Round(durationTotal_20M.Add_Conc_4_Percent / 3, 1);
            durationTotal_20M.Add_Conc_5_Percent = Math.Round(durationTotal_20M.Add_Conc_5_Percent / 3, 1);

            durationTotal_20M.CO_Emis_Percent = Math.Round(durationTotal_20M.CO_Emis_Percent / 3, 1);
            durationTotal_20M.CO2_Emis_Percent = Math.Round(durationTotal_20M.CO2_Emis_Percent / 3, 1);
            durationTotal_20M.NO_Emis_Percent = Math.Round(durationTotal_20M.NO_Emis_Percent / 3, 1);
            durationTotal_20M.NO2_Emis_Percent = Math.Round(durationTotal_20M.NO2_Emis_Percent / 3, 1);
            durationTotal_20M.NOx_Emis_Percent = Math.Round(durationTotal_20M.NOx_Emis_Percent / 3, 1);
            durationTotal_20M.SO2_Emis_Percent = Math.Round(durationTotal_20M.SO2_Emis_Percent / 3, 1);
            durationTotal_20M.Dust_Emis_Percent = Math.Round(durationTotal_20M.Dust_Emis_Percent / 3, 1);
            durationTotal_20M.CH4_Emis_Percent = Math.Round(durationTotal_20M.CH4_Emis_Percent / 3, 1);
            durationTotal_20M.H2S_Emis_Percent = Math.Round(durationTotal_20M.H2S_Emis_Percent / 3, 1);
            durationTotal_20M.Add_Emis_1_Percent = Math.Round(durationTotal_20M.Add_Emis_1_Percent / 3, 1);
            durationTotal_20M.Add_Emis_2_Percent = Math.Round(durationTotal_20M.Add_Emis_2_Percent / 3, 1);
            durationTotal_20M.Add_Emis_3_Percent = Math.Round(durationTotal_20M.Add_Emis_3_Percent / 3, 1);
            durationTotal_20M.Add_Emis_4_Percent = Math.Round(durationTotal_20M.Add_Emis_4_Percent / 3, 1);
            durationTotal_20M.Add_Emis_5_Percent = Math.Round(durationTotal_20M.Add_Emis_5_Percent / 3, 1);

        }
    }
}
