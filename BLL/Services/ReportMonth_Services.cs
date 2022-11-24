using ASK.BLL.Helper;
using ASK.BLL.Interfaces;
using ASK.BLL.Models;
using ASK.DAL.Interfaces;
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASK.BLL.Services
{
    public class ReportMonth_Services : IReportMonth
    {
        private readonly IPDZ _PDZ_Repository;
        private readonly IReportDay _ReportDay_Services;



        public ReportMonth_Services(IReportDay ReportDay_Services, IPDZ PDZ_Repository)
        {
            _PDZ_Repository = PDZ_Repository;
            _ReportDay_Services = ReportDay_Services;
        }



        public ReportMonth_Model Generate(DateTime yearMonth)
        {
            ReportMonth_Model reportMonth = new ReportMonth_Model();
            reportMonth.TypeReport = TypeReports.ReportMonth;                                                                         //Указывает тип отчёта для обработчика в excel

            var dataNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (yearMonth >= dataNow)
                reportMonth.NumberDays = 0;
            else
                reportMonth.NumberDays = DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month);

            if (yearMonth.Year == DateTime.Now.Year && yearMonth.Month == DateTime.Now.Month)
                reportMonth.NumberDays = DateTime.Now.Day;

            for (int i = 0; i < reportMonth.NumberDays; i++)
            {
                reportMonth.TableReport.Add(new TableReport_Model());
                var bufdayReport = _ReportDay_Services.Generate(new DateTime(yearMonth.Year, yearMonth.Month, i + 1));

                reportMonth.TableReport[i].Date = new DateTime(yearMonth.Year, yearMonth.Month, i + 1);
                reportMonth.TableReport[i].Mode_ASK = bufdayReport.avgTotal_20M.Mode_ASK;

                if (bufdayReport.TableReport.Count > 0) reportMonth.TableReport[i].PDZ_Fuel = bufdayReport.TableReport[0].PDZ_Fuel; //Обязательно должен идти первее подсчёта процентов превышений
                else reportMonth.TableReport[i].PDZ_Fuel = 0;

                //Концентрации
                reportMonth.TableReport[i].CO_Conc = bufdayReport.avgTotal_20M.CO_Conc;
                reportMonth.TableReport[i].CO2_Conc = bufdayReport.avgTotal_20M.CO2_Conc;
                reportMonth.TableReport[i].NO_Conc = bufdayReport.avgTotal_20M.NO_Conc;
                reportMonth.TableReport[i].NO2_Conc = bufdayReport.avgTotal_20M.NO2_Conc;
                reportMonth.TableReport[i].NOx_Conc = bufdayReport.avgTotal_20M.NOx_Conc;
                reportMonth.TableReport[i].SO2_Conc = bufdayReport.avgTotal_20M.SO2_Conc;
                reportMonth.TableReport[i].Dust_Conc = bufdayReport.avgTotal_20M.Dust_Conc;
                reportMonth.TableReport[i].CH4_Conc = bufdayReport.avgTotal_20M.CH4_Conc;
                reportMonth.TableReport[i].H2S_Conc = bufdayReport.avgTotal_20M.H2S_Conc;
                reportMonth.TableReport[i].Add_Conc_1 = bufdayReport.avgTotal_20M.Add_Conc_1;
                reportMonth.TableReport[i].Add_Conc_2 = bufdayReport.avgTotal_20M.Add_Conc_2;
                reportMonth.TableReport[i].Add_Conc_3 = bufdayReport.avgTotal_20M.Add_Conc_3;
                reportMonth.TableReport[i].Add_Conc_4 = bufdayReport.avgTotal_20M.Add_Conc_4;
                reportMonth.TableReport[i].Add_Conc_5 = bufdayReport.avgTotal_20M.Add_Conc_5;

                //Концентрации длит. прев.
                reportMonth.TableReport[i].CO_Conc_Percent = bufdayReport.durationTotal_20M.CO_Conc_Percent;
                reportMonth.TableReport[i].CO2_Conc_Percent = bufdayReport.durationTotal_20M.CO2_Conc_Percent;
                reportMonth.TableReport[i].NO_Conc_Percent = bufdayReport.durationTotal_20M.NO_Conc_Percent;
                reportMonth.TableReport[i].NO2_Conc_Percent = bufdayReport.durationTotal_20M.NO2_Conc_Percent;
                reportMonth.TableReport[i].NOx_Conc_Percent = bufdayReport.durationTotal_20M.NOx_Conc_Percent;
                reportMonth.TableReport[i].SO2_Conc_Percent = bufdayReport.durationTotal_20M.SO2_Conc_Percent;
                reportMonth.TableReport[i].Dust_Conc_Percent = bufdayReport.durationTotal_20M.Dust_Conc_Percent;
                reportMonth.TableReport[i].CH4_Conc_Percent = bufdayReport.durationTotal_20M.CH4_Conc_Percent;
                reportMonth.TableReport[i].H2S_Conc_Percent = bufdayReport.durationTotal_20M.H2S_Conc_Percent;
                reportMonth.TableReport[i].Add_Conc_1_Percent = bufdayReport.durationTotal_20M.Add_Conc_1_Percent;
                reportMonth.TableReport[i].Add_Conc_2_Percent = bufdayReport.durationTotal_20M.Add_Conc_2_Percent;
                reportMonth.TableReport[i].Add_Conc_3_Percent = bufdayReport.durationTotal_20M.Add_Conc_3_Percent;
                reportMonth.TableReport[i].Add_Conc_4_Percent = bufdayReport.durationTotal_20M.Add_Conc_4_Percent;
                reportMonth.TableReport[i].Add_Conc_5_Percent = bufdayReport.durationTotal_20M.Add_Conc_5_Percent;

                //Выбросы
                reportMonth.TableReport[i].CO_Emis = bufdayReport.sumTotal_20M.CO_Emis;
                reportMonth.TableReport[i].CO2_Emis = bufdayReport.sumTotal_20M.CO2_Emis;
                reportMonth.TableReport[i].NO_Emis = bufdayReport.sumTotal_20M.NO_Emis;
                reportMonth.TableReport[i].NO2_Emis = bufdayReport.sumTotal_20M.NO2_Emis;
                reportMonth.TableReport[i].NOx_Emis = bufdayReport.sumTotal_20M.NOx_Emis;
                reportMonth.TableReport[i].SO2_Emis = bufdayReport.sumTotal_20M.SO2_Emis;
                reportMonth.TableReport[i].Dust_Emis = bufdayReport.sumTotal_20M.Dust_Emis;
                reportMonth.TableReport[i].CH4_Emis = bufdayReport.sumTotal_20M.CH4_Emis;
                reportMonth.TableReport[i].H2S_Emis = bufdayReport.sumTotal_20M.H2S_Emis;
                reportMonth.TableReport[i].Add_Emis_1 = bufdayReport.sumTotal_20M.Add_Emis_1;
                reportMonth.TableReport[i].Add_Emis_2 = bufdayReport.sumTotal_20M.Add_Emis_2;
                reportMonth.TableReport[i].Add_Emis_3 = bufdayReport.sumTotal_20M.Add_Emis_3;
                reportMonth.TableReport[i].Add_Emis_4 = bufdayReport.sumTotal_20M.Add_Emis_4;
                reportMonth.TableReport[i].Add_Emis_5 = bufdayReport.sumTotal_20M.Add_Emis_5;

                //Выбросы длит. прев.
                reportMonth.TableReport[i].CO_Emis_Percent = bufdayReport.durationTotal_20M.CO_Emis_Percent;
                reportMonth.TableReport[i].CO2_Emis_Percent = bufdayReport.durationTotal_20M.CO2_Emis_Percent;
                reportMonth.TableReport[i].NO_Emis_Percent = bufdayReport.durationTotal_20M.NO_Emis_Percent;
                reportMonth.TableReport[i].NO2_Emis_Percent = bufdayReport.durationTotal_20M.NO2_Emis_Percent;
                reportMonth.TableReport[i].NOx_Emis_Percent = bufdayReport.durationTotal_20M.NOx_Emis_Percent;
                reportMonth.TableReport[i].SO2_Emis_Percent = bufdayReport.durationTotal_20M.SO2_Emis_Percent;
                reportMonth.TableReport[i].Dust_Emis_Percent = bufdayReport.durationTotal_20M.Dust_Emis_Percent;
                reportMonth.TableReport[i].CH4_Emis_Percent = bufdayReport.durationTotal_20M.CH4_Emis_Percent;
                reportMonth.TableReport[i].H2S_Emis_Percent = bufdayReport.durationTotal_20M.H2S_Emis_Percent;
                reportMonth.TableReport[i].Add_Emis_1_Percent = bufdayReport.durationTotal_20M.Add_Emis_1_Percent;
                reportMonth.TableReport[i].Add_Emis_2_Percent = bufdayReport.durationTotal_20M.Add_Emis_2_Percent;
                reportMonth.TableReport[i].Add_Emis_3_Percent = bufdayReport.durationTotal_20M.Add_Emis_3_Percent;
                reportMonth.TableReport[i].Add_Emis_4_Percent = bufdayReport.durationTotal_20M.Add_Emis_4_Percent;
                reportMonth.TableReport[i].Add_Emis_5_Percent = bufdayReport.durationTotal_20M.Add_Emis_5_Percent;

                //Параметры
                reportMonth.TableReport[i].Pressure = bufdayReport.avgTotal_20M.Pressure;
                reportMonth.TableReport[i].Temperature = bufdayReport.avgTotal_20M.Temperature;
                reportMonth.TableReport[i].Temperature_KIP = bufdayReport.avgTotal_20M.Temperature_KIP;
                reportMonth.TableReport[i].Temperature_NOx = bufdayReport.avgTotal_20M.Temperature_NOx;
                reportMonth.TableReport[i].Speed = bufdayReport.avgTotal_20M.Speed;
                reportMonth.TableReport[i].Flow = bufdayReport.avgTotal_20M.Flow;
                reportMonth.TableReport[i].O2_Dry = bufdayReport.avgTotal_20M.O2_Dry;
                reportMonth.TableReport[i].O2_Wet = bufdayReport.avgTotal_20M.O2_Wet;
                reportMonth.TableReport[i].H2O = bufdayReport.avgTotal_20M.H2O;

                reportMonth.sumTotal_20M.Mode_ASK = Math.Round(reportMonth.sumTotal_20M.Mode_ASK + bufdayReport.sumTotal_20M.Mode_ASK, 3);
            }
            //Вытаскиваем ПДЗ на месяц
            var pdz = _PDZ_Repository.FisrsPDZMonth(yearMonth);

            reportMonth.PDZ.Date = pdz.Date;
            reportMonth.PDZ.NumberPDZ = pdz.NumberPDZ;

            reportMonth.PDZ.CO_Conc = pdz.CO_Conc;
            reportMonth.PDZ.CO2_Conc = pdz.CO2_Conc;
            reportMonth.PDZ.NO_Conc = pdz.NO_Conc;
            reportMonth.PDZ.NO2_Conc = pdz.NO2_Conc;
            reportMonth.PDZ.NOx_Conc = pdz.NOx_Conc;
            reportMonth.PDZ.SO2_Conc = pdz.SO2_Conc;
            reportMonth.PDZ.Dust_Conc = pdz.Dust_Conc;
            reportMonth.PDZ.CH4_Conc = pdz.CH4_Conc;
            reportMonth.PDZ.H2S_Conc = pdz.H2S_Conc;
            reportMonth.PDZ.Add_Conc_1 = pdz.Add_Conc_1;
            reportMonth.PDZ.Add_Conc_2 = pdz.Add_Conc_2;
            reportMonth.PDZ.Add_Conc_3 = pdz.Add_Conc_3;
            reportMonth.PDZ.Add_Conc_4 = pdz.Add_Conc_4;
            reportMonth.PDZ.Add_Conc_5 = pdz.Add_Conc_5;

            reportMonth.PDZ.CO_Emis = pdz.CO_Emis;
            reportMonth.PDZ.CO2_Emis = pdz.CO2_Emis;
            reportMonth.PDZ.NO_Emis = pdz.NO_Emis;
            reportMonth.PDZ.NO2_Emis = pdz.NO2_Emis;
            reportMonth.PDZ.NOx_Emis = pdz.NOx_Emis;
            reportMonth.PDZ.SO2_Emis = pdz.SO2_Emis;
            reportMonth.PDZ.Dust_Emis = pdz.Dust_Emis;
            reportMonth.PDZ.CH4_Emis = pdz.CH4_Emis;
            reportMonth.PDZ.H2S_Emis = pdz.H2S_Emis;
            reportMonth.PDZ.Add_Emis_1 = pdz.Add_Emis_1;
            reportMonth.PDZ.Add_Emis_2 = pdz.Add_Emis_2;
            reportMonth.PDZ.Add_Emis_3 = pdz.Add_Emis_3;
            reportMonth.PDZ.Add_Emis_4 = pdz.Add_Emis_4;
            reportMonth.PDZ.Add_Emis_5 = pdz.Add_Emis_5;
            //PDZ[i + 1].Current = PDZs[i].Current;

            foreach (var tabletDay in reportMonth.TableReport)
            {
                //Концентрации
                reportMonth.sumTotal_20M.CO_Conc += tabletDay.CO_Conc;
                reportMonth.sumTotal_20M.CO2_Conc += tabletDay.CO2_Conc;
                reportMonth.sumTotal_20M.NOx_Conc += tabletDay.NOx_Conc;
                reportMonth.sumTotal_20M.SO2_Conc += tabletDay.SO2_Conc;
                reportMonth.sumTotal_20M.Dust_Conc += tabletDay.Dust_Conc;
                reportMonth.sumTotal_20M.CH4_Conc += tabletDay.CH4_Conc;
                reportMonth.sumTotal_20M.H2S_Conc += tabletDay.H2S_Conc;
                reportMonth.sumTotal_20M.Add_Conc_1 += tabletDay.Add_Conc_1;
                reportMonth.sumTotal_20M.Add_Conc_2 += tabletDay.Add_Conc_2;
                reportMonth.sumTotal_20M.Add_Conc_3 += tabletDay.Add_Conc_3;
                reportMonth.sumTotal_20M.Add_Conc_4 += tabletDay.Add_Conc_4;
                reportMonth.sumTotal_20M.Add_Conc_5 += tabletDay.Add_Conc_5;

                //Выбросы
                reportMonth.sumTotal_20M.CO_Emis += tabletDay.CO_Emis;
                reportMonth.sumTotal_20M.CO2_Emis += tabletDay.CO2_Emis;
                reportMonth.sumTotal_20M.NO_Emis += tabletDay.NO_Emis;
                reportMonth.sumTotal_20M.NOx_Emis += tabletDay.NOx_Emis;
                reportMonth.sumTotal_20M.NO2_Emis += tabletDay.NO2_Emis;
                reportMonth.sumTotal_20M.SO2_Emis += tabletDay.SO2_Emis;
                reportMonth.sumTotal_20M.Dust_Emis += tabletDay.Dust_Emis;
                reportMonth.sumTotal_20M.CH4_Emis += tabletDay.CH4_Emis;
                reportMonth.sumTotal_20M.H2S_Emis += tabletDay.H2S_Emis;
                reportMonth.sumTotal_20M.Add_Emis_1 += tabletDay.Add_Emis_1;
                reportMonth.sumTotal_20M.Add_Emis_2 += tabletDay.Add_Emis_2;
                reportMonth.sumTotal_20M.Add_Emis_3 += tabletDay.Add_Emis_3;
                reportMonth.sumTotal_20M.Add_Emis_4 += tabletDay.Add_Emis_4;
                reportMonth.sumTotal_20M.Add_Emis_5 += tabletDay.Add_Emis_5;

                //Концентрации длит. прев.
                reportMonth.avgTotal_20M.CO_Conc_Percent += tabletDay.CO_Conc_Percent;
                reportMonth.avgTotal_20M.CO2_Conc_Percent += tabletDay.CO2_Conc_Percent;
                reportMonth.avgTotal_20M.NOx_Conc_Percent += tabletDay.NOx_Conc_Percent;
                reportMonth.avgTotal_20M.SO2_Conc_Percent += tabletDay.SO2_Conc_Percent;
                reportMonth.avgTotal_20M.Dust_Conc_Percent += tabletDay.Dust_Conc_Percent;
                reportMonth.avgTotal_20M.CH4_Conc_Percent += tabletDay.CH4_Conc_Percent;
                reportMonth.avgTotal_20M.H2S_Conc_Percent += tabletDay.H2S_Conc_Percent;
                reportMonth.avgTotal_20M.Add_Conc_1_Percent += tabletDay.Add_Conc_1_Percent;
                reportMonth.avgTotal_20M.Add_Conc_2_Percent += tabletDay.Add_Conc_2_Percent;
                reportMonth.avgTotal_20M.Add_Conc_3_Percent += tabletDay.Add_Conc_3_Percent;
                reportMonth.avgTotal_20M.Add_Conc_4_Percent += tabletDay.Add_Conc_4_Percent;
                reportMonth.avgTotal_20M.Add_Conc_5_Percent += tabletDay.Add_Conc_5_Percent;

                //Выбросы длит. прев.
                reportMonth.avgTotal_20M.CO_Emis_Percent += tabletDay.CO_Emis_Percent;
                reportMonth.avgTotal_20M.CO2_Emis_Percent += tabletDay.CO2_Emis_Percent;
                reportMonth.avgTotal_20M.NO_Emis_Percent += tabletDay.NO_Emis_Percent;
                reportMonth.avgTotal_20M.NOx_Emis_Percent += tabletDay.NOx_Emis_Percent;
                reportMonth.avgTotal_20M.NO2_Emis_Percent += tabletDay.NO2_Emis_Percent;
                reportMonth.avgTotal_20M.SO2_Emis_Percent += tabletDay.SO2_Emis_Percent;
                reportMonth.avgTotal_20M.Dust_Emis_Percent += tabletDay.Dust_Emis_Percent;
                reportMonth.avgTotal_20M.CH4_Emis_Percent += tabletDay.CH4_Emis_Percent;
                reportMonth.avgTotal_20M.H2S_Emis_Percent += tabletDay.H2S_Emis_Percent;
                reportMonth.avgTotal_20M.Add_Emis_1_Percent += tabletDay.Add_Emis_1_Percent;
                reportMonth.avgTotal_20M.Add_Emis_2_Percent += tabletDay.Add_Emis_2_Percent;
                reportMonth.avgTotal_20M.Add_Emis_3_Percent += tabletDay.Add_Emis_3_Percent;
                reportMonth.avgTotal_20M.Add_Emis_4_Percent += tabletDay.Add_Emis_4_Percent;
                reportMonth.avgTotal_20M.Add_Emis_5_Percent += tabletDay.Add_Emis_5_Percent;

                //Параметры
                reportMonth.sumTotal_20M.Pressure += tabletDay.Pressure;
                reportMonth.sumTotal_20M.Temperature += tabletDay.Temperature;
                reportMonth.sumTotal_20M.Temperature_KIP += tabletDay.Temperature_KIP;
                reportMonth.sumTotal_20M.Temperature_NOx += tabletDay.Temperature_NOx;
                reportMonth.sumTotal_20M.Speed += tabletDay.Speed;
                reportMonth.sumTotal_20M.Flow += tabletDay.Flow;
                reportMonth.sumTotal_20M.O2_Dry += tabletDay.O2_Dry;
                reportMonth.sumTotal_20M.O2_Wet += tabletDay.O2_Wet;
                reportMonth.sumTotal_20M.H2O += tabletDay.H2O;
            }
            //Усредняем за 20 мин
            int counter = reportMonth.TableReport.Count;
            if (counter <= 1) counter = 1;

            //Концентрации
            reportMonth.avgTotal_20M.CO_Conc = Math.Round(reportMonth.sumTotal_20M.CO_Conc / counter, 3);
            reportMonth.avgTotal_20M.CO2_Conc = Math.Round(reportMonth.sumTotal_20M.CO2_Conc / counter, 3);
            reportMonth.avgTotal_20M.NO_Conc = Math.Round(reportMonth.sumTotal_20M.NO_Conc / counter, 3);
            reportMonth.avgTotal_20M.NO2_Conc = Math.Round(reportMonth.sumTotal_20M.NO2_Conc / counter, 3);
            reportMonth.avgTotal_20M.NOx_Conc = Math.Round(reportMonth.sumTotal_20M.NOx_Conc / counter, 3);
            reportMonth.avgTotal_20M.SO2_Conc = Math.Round(reportMonth.sumTotal_20M.SO2_Conc / counter, 3);
            reportMonth.avgTotal_20M.Dust_Conc = Math.Round(reportMonth.sumTotal_20M.Dust_Conc / counter, 3);
            reportMonth.avgTotal_20M.CH4_Conc = Math.Round(reportMonth.sumTotal_20M.CH4_Conc / counter, 3);
            reportMonth.avgTotal_20M.H2S_Conc = Math.Round(reportMonth.sumTotal_20M.H2S_Conc / counter, 3);
            reportMonth.avgTotal_20M.Add_Conc_1 = Math.Round(reportMonth.sumTotal_20M.Add_Conc_1 / counter, 3);
            reportMonth.avgTotal_20M.Add_Conc_2 = Math.Round(reportMonth.sumTotal_20M.Add_Conc_2 / counter, 3);
            reportMonth.avgTotal_20M.Add_Conc_3 = Math.Round(reportMonth.sumTotal_20M.Add_Conc_3 / counter, 3);
            reportMonth.avgTotal_20M.Add_Conc_4 = Math.Round(reportMonth.sumTotal_20M.Add_Conc_4 / counter, 3);
            reportMonth.avgTotal_20M.Add_Conc_5 = Math.Round(reportMonth.sumTotal_20M.Add_Conc_5 / counter, 3);

            //Выбросы
            reportMonth.avgTotal_20M.CO_Emis = Math.Round(reportMonth.sumTotal_20M.CO_Emis / counter, 3);
            reportMonth.avgTotal_20M.CO2_Emis = Math.Round(reportMonth.sumTotal_20M.CO2_Emis / counter, 3);
            reportMonth.avgTotal_20M.NO_Emis = Math.Round(reportMonth.sumTotal_20M.NO_Emis / counter, 3);
            reportMonth.avgTotal_20M.NO2_Emis = Math.Round(reportMonth.sumTotal_20M.NO2_Emis / counter, 3);
            reportMonth.avgTotal_20M.NOx_Emis = Math.Round(reportMonth.sumTotal_20M.NOx_Emis / counter, 3);
            reportMonth.avgTotal_20M.SO2_Emis = Math.Round(reportMonth.sumTotal_20M.SO2_Emis / counter, 3);
            reportMonth.avgTotal_20M.Dust_Emis = Math.Round(reportMonth.sumTotal_20M.Dust_Emis / counter, 3);
            reportMonth.avgTotal_20M.CH4_Emis = Math.Round(reportMonth.sumTotal_20M.CH4_Emis / counter, 3);
            reportMonth.avgTotal_20M.H2S_Emis = Math.Round(reportMonth.sumTotal_20M.H2S_Emis / counter, 3);
            reportMonth.avgTotal_20M.Add_Emis_1 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_1 / counter, 3);
            reportMonth.avgTotal_20M.Add_Emis_2 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_2 / counter, 3);
            reportMonth.avgTotal_20M.Add_Emis_3 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_3 / counter, 3);
            reportMonth.avgTotal_20M.Add_Emis_4 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_4 / counter, 3);
            reportMonth.avgTotal_20M.Add_Emis_5 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_5 / counter, 3);

            //Параметры
            reportMonth.avgTotal_20M.Pressure = Math.Round(reportMonth.sumTotal_20M.Pressure / counter, 3);
            reportMonth.avgTotal_20M.Temperature = Math.Round(reportMonth.sumTotal_20M.Temperature / counter, 3);
            reportMonth.avgTotal_20M.Temperature_KIP = Math.Round(reportMonth.sumTotal_20M.Temperature_KIP / counter, 3);
            reportMonth.avgTotal_20M.Temperature_NOx = Math.Round(reportMonth.sumTotal_20M.Temperature_NOx / counter, 3);
            reportMonth.avgTotal_20M.Speed = Math.Round(reportMonth.sumTotal_20M.Speed / counter, 3);
            reportMonth.avgTotal_20M.Flow = Math.Round(reportMonth.sumTotal_20M.Flow / counter, 3);
            reportMonth.avgTotal_20M.O2_Dry = Math.Round(reportMonth.sumTotal_20M.O2_Dry / counter, 3);
            reportMonth.avgTotal_20M.O2_Wet = Math.Round(reportMonth.sumTotal_20M.O2_Wet / counter, 3);
            reportMonth.avgTotal_20M.H2O = Math.Round(reportMonth.sumTotal_20M.H2O / counter, 3);

            //Считаем время простоя
            //sumTotal_20M.Mode_ASK = Math.Round(sumTotal_20M.Mode_ASK / 3, 3);

            //Сумма за сутки
            reportMonth.sumTotal_20M.CO_Emis = Math.Round(reportMonth.sumTotal_20M.CO_Emis / 1000, 3);
            reportMonth.sumTotal_20M.CO2_Emis = Math.Round(reportMonth.sumTotal_20M.CO2_Emis / 1000, 3);
            reportMonth.sumTotal_20M.NO_Emis = Math.Round(reportMonth.sumTotal_20M.NO_Emis / 1000, 3);
            reportMonth.sumTotal_20M.NOx_Emis = Math.Round(reportMonth.sumTotal_20M.NOx_Emis / 1000, 3);
            reportMonth.sumTotal_20M.NO2_Emis = Math.Round(reportMonth.sumTotal_20M.NO2_Emis / 1000, 3);
            reportMonth.sumTotal_20M.SO2_Emis = Math.Round(reportMonth.sumTotal_20M.SO2_Emis / 1000, 3);
            reportMonth.sumTotal_20M.Dust_Emis = Math.Round(reportMonth.sumTotal_20M.Dust_Emis / 1000, 3);
            reportMonth.sumTotal_20M.CH4_Emis = Math.Round(reportMonth.sumTotal_20M.CH4_Emis / 1000, 3);
            reportMonth.sumTotal_20M.H2S_Emis = Math.Round(reportMonth.sumTotal_20M.H2S_Emis / 1000, 3);
            reportMonth.sumTotal_20M.Add_Emis_1 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_1 / 1000, 3);
            reportMonth.sumTotal_20M.Add_Emis_2 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_2 / 1000, 3);
            reportMonth.sumTotal_20M.Add_Emis_3 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_3 / 1000, 3);
            reportMonth.sumTotal_20M.Add_Emis_4 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_4 / 1000, 3);
            reportMonth.sumTotal_20M.Add_Emis_5 = Math.Round(reportMonth.sumTotal_20M.Add_Emis_5 / 1000, 3);

            reportMonth.sumTotal_20M.CO_Conc = 0.0;
            reportMonth.sumTotal_20M.CO2_Conc = 0.0;
            reportMonth.sumTotal_20M.NO_Conc = 0.0;
            reportMonth.sumTotal_20M.NOx_Conc = 0.0;
            reportMonth.sumTotal_20M.NO2_Conc = 0.0;
            reportMonth.sumTotal_20M.SO2_Conc = 0.0;
            reportMonth.sumTotal_20M.Dust_Conc = 0.0;
            reportMonth.sumTotal_20M.CH4_Conc = 0.0;
            reportMonth.sumTotal_20M.H2S_Conc = 0.0;
            reportMonth.sumTotal_20M.Add_Conc_1 = 0.0;
            reportMonth.sumTotal_20M.Add_Conc_2 = 0.0;
            reportMonth.sumTotal_20M.Add_Conc_3 = 0.0;
            reportMonth.sumTotal_20M.Add_Conc_4 = 0.0;
            reportMonth.sumTotal_20M.Add_Conc_5 = 0.0;






            //Интересные вещи происходят,получаю хорошие (3 знака после запятой) значения лишь сумирую их и выскакивает от 6 знаков после запятой
            //Концентрации длит. прев.
            reportMonth.avgTotal_20M.CO_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.CO_Conc_Percent, 3);
            reportMonth.avgTotal_20M.CO2_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.CO2_Conc_Percent, 3);
            reportMonth.avgTotal_20M.NOx_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.NOx_Conc_Percent, 3);
            reportMonth.avgTotal_20M.SO2_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.SO2_Conc_Percent, 3);
            reportMonth.avgTotal_20M.Dust_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.Dust_Conc_Percent, 3);
            reportMonth.avgTotal_20M.CH4_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.CH4_Conc_Percent, 3);
            reportMonth.avgTotal_20M.H2S_Conc_Percent += Math.Round(reportMonth.avgTotal_20M.H2S_Conc_Percent, 3);
            reportMonth.avgTotal_20M.Add_Conc_1_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Conc_1_Percent, 3);
            reportMonth.avgTotal_20M.Add_Conc_2_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Conc_2_Percent, 3);
            reportMonth.avgTotal_20M.Add_Conc_3_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Conc_3_Percent, 3);
            reportMonth.avgTotal_20M.Add_Conc_4_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Conc_4_Percent, 3);
            reportMonth.avgTotal_20M.Add_Conc_5_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Conc_5_Percent, 3);

            //Выбросы длит. прев.
            reportMonth.avgTotal_20M.CO_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.CO_Emis_Percent, 3);
            reportMonth.avgTotal_20M.CO2_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.CO2_Emis_Percent, 3);
            reportMonth.avgTotal_20M.NO_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.NO_Emis_Percent, 3);
            reportMonth.avgTotal_20M.NOx_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.NOx_Emis_Percent, 3);
            reportMonth.avgTotal_20M.NO2_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.NO2_Emis_Percent, 3);
            reportMonth.avgTotal_20M.SO2_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.SO2_Emis_Percent, 3);
            reportMonth.avgTotal_20M.Dust_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.Dust_Emis_Percent, 3);
            reportMonth.avgTotal_20M.CH4_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.CH4_Emis_Percent, 3);
            reportMonth.avgTotal_20M.H2S_Emis_Percent += Math.Round(reportMonth.avgTotal_20M.H2S_Emis_Percent, 3);
            reportMonth.avgTotal_20M.Add_Emis_1_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Emis_1_Percent, 3);
            reportMonth.avgTotal_20M.Add_Emis_2_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Emis_2_Percent, 3);
            reportMonth.avgTotal_20M.Add_Emis_3_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Emis_3_Percent, 3);
            reportMonth.avgTotal_20M.Add_Emis_4_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Emis_4_Percent, 3);
            reportMonth.avgTotal_20M.Add_Emis_5_Percent += Math.Round(reportMonth.avgTotal_20M.Add_Emis_5_Percent, 3);




            return reportMonth;
        }
    }
}
