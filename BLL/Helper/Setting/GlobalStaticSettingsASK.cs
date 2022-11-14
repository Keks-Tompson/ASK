using ASK.BLL.Helper.Chart;
using ASK.BLL.Models;
using ASK.BLL.Services;
using ASK.DAL;
using ASK.DAL.Models;
using ASK.DAL.Repository;
using NModbus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASK.BLL.Helper.Setting
{
    public class GlobalStaticSettingsASK
    {
        static int delayedStart = 0;                //Отложенный старт для записис аварий в БД


        public static bool is_simulation = true;    //Режим симуляции 4-20 сигналов
        private static double O2_Last = 1;          //Режим симуляции 4-20 сигналов старый O2
        private static Random rnd = new Random();   //Режим симуляции 4-20 рандом

        //Chart
        public static Chart_CurrentValue ChartCurrent { get; set; } = new Chart_CurrentValue();
        public static List<Chart_CurrentValue> ChartList { get; set; } = new List<Chart_CurrentValue>();
        public static int CounterChart { get; set; } = 0;

        public static SettingOptions_JSON_Model SettingOptions { get; set; }
        public static VisibilityReportOptions_JSON_Model VisibilityReportOptions { get; set; }
        public static CalculationSetting_JSON_Model CalculationSetting { get; set; }                            //Хранит параметры/необходимые условия для расчёта концентрацй/выбросов
        public static PDZ_JSON_Model PDZ { get; set; }                                                          //Все 3 вида топлива ПДЗ 
        public static PDZ_Active_Model PDZ_Current { get; set; } = new PDZ_Active_Model();                      //Текущие ПДЗ в double
        public static PDZ_String_Active_Model PDZ_Current_String { get; set; } = new PDZ_String_Active_Model(); //Текущие ПДЗ в string (-/-)
        public static SensorRange_JSON_Model SensorRange { get; set; }
        public static List<Sensor_4_20_Model> Sensor_4_20s = new List<Sensor_4_20_Model>();                     //Будем хранить значение 4-20мА 
        public static Sensor_4_20_Model SensorNow { get; set; } = new Sensor_4_20_Model();                      //Будем хранить текущее значения датчиков (4-20)
        public static Sensor_4_20_Model SensorScaledNow { get; set; } = new Sensor_4_20_Model();                //Будет хранить текущие значения прямых показаний датчиков 
        public static List<Array20M_Model> Array20Ms { get; set; } = new List<Array20M_Model>();                //Текущие значения формируемой 20 минутки
        public static Array20M_Model CurrentConcEmis { get; set; } = new Array20M_Model();                      //Текущие значение концентрацйи и выбросов после приведения

        //Цвета 
        public static Color ColorExcess { get; set; } = Color.FromArgb(247, 213, 213);                          //Цвет превышения
        //public static Color ColorExcess { get; set; } = Color.FromArgb(0, 14, 14, 3);                         //Цвет превышения
        public static Color ColorHeader1 { get; set; } = Color.FromArgb(255, 230, 168);                         //Цвет заголовка 1
        public static Color ColorHeader2 { get; set; } = Color.FromArgb(182, 242, 250);                         //Цвет заголовка 2

        public static bool stopGetSernsorNow = false; //Если идёт запись в БД 20М тормозит поток обновления данных, пока запись не запишется; 

        //всё для пинга ПЛК 
        static Ping ping = new Ping();
        static PingReply connected;

        //Нет связи с плк
        //public static bool isNotConnection = false;

        //Аварии
        public static GlobalAlarm_Model globalAlarms = new GlobalAlarm_Model();                                 //Глобальные аварии
        public static AllAlarm_Model allAlarm = new AllAlarm_Model();                                           //Все остальные варии будут хранится тут          


        public static void SaveSettingOptionsJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SettingOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(SettingOptions, typeof(SettingOptions_JSON_Model)));
            file.Close();
        }



        public static void SaveVisibilityReportOptionsJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\VisibilityReportOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(VisibilityReportOptions, typeof(VisibilityReportOptions_JSON_Model)));
            file.Close();
        }



        public static void SaveCalculationSettingJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\CalculationSettingJSON.json");
            file.WriteLine(JsonSerializer.Serialize(CalculationSetting, typeof(CalculationSetting_JSON_Model)));
            file.Close();
        }



        public static void SavePdz_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\PdzJSON.json");
            file.WriteLine(JsonSerializer.Serialize(PDZ, typeof(PDZ_JSON_Model)));
            file.Close();
            GetCurrentPDZ();
        }



        public static void SaveSensorRange_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SensorRangeJSON.JSON");
            file.WriteLine(JsonSerializer.Serialize(SensorRange, typeof(SensorRange_JSON_Model)));
            file.Close();

            UpdateVisibilityAlarm();
        }



        static GlobalStaticSettingsASK()
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SettingOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SettingOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SettingOptionsJSON.json");
                SettingOptions = JsonSerializer.Deserialize<SettingOptions_JSON_Model>(data);
            }
            else //не существует
            {
                SettingOptions = new SettingOptions_JSON_Model();

                SaveSettingOptionsJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  VisibilityReportOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\VisibilityReportOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\VisibilityReportOptionsJSON.json");
                VisibilityReportOptions = JsonSerializer.Deserialize<VisibilityReportOptions_JSON_Model>(data);
            }
            else //не существует
            {
                VisibilityReportOptions = new VisibilityReportOptions_JSON_Model();

                SaveVisibilityReportOptionsJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  CalculationSettingJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\CalculationSettingJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\CalculationSettingJSON.json");
                CalculationSetting = JsonSerializer.Deserialize<CalculationSetting_JSON_Model>(data);
            }
            else //не существует
            {
                CalculationSetting = new CalculationSetting_JSON_Model();

                SaveCalculationSettingJSON();
            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  PdzJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\PdzJSON.json"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\PdzJSON.json");
                PDZ = JsonSerializer.Deserialize<PDZ_JSON_Model>(data);
            }
            else
            {
                PDZ = new PDZ_JSON_Model();
                SavePdz_JSON();

            }



            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SensorRangeJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SensorRangeJSON.JSON"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SensorRangeJSON.JSON");
                SensorRange = JsonSerializer.Deserialize<SensorRange_JSON_Model>(data);
            }
            else
            {
                SensorRange = new SensorRange_JSON_Model();
                SaveSensorRange_JSON();
            }

            UpdateVisibilityAlarm();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var accident_log_Service = new ACCIDENT_LOG_Repository(db);
                accident_log_Service.StratSystem_ACCIDENT_LOG();
            }

            GetCurrentPDZ();
            ChartList.Add(new Chart_CurrentValue()); ///Первая нулевая точка //Потом удалить!
        }



        //Обновляем используемые аварии
        public static void UpdateVisibilityAlarm()
        {
            if (SensorRange.CO.Is_Used)
                allAlarm.SensorAlarm.CO.Is_Used = true;
            else
                allAlarm.SensorAlarm.CO.Is_Used = false;


            if (SensorRange.CO2.Is_Used)
                allAlarm.SensorAlarm.CO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.CO2.Is_Used = false;


            if (SensorRange.NO.Is_Used)
                allAlarm.SensorAlarm.NO.Is_Used = true;
            else
                allAlarm.SensorAlarm.NO.Is_Used = false;


            if (SensorRange.NO2.Is_Used)
                allAlarm.SensorAlarm.NO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.NO2.Is_Used = false;


            if (SensorRange.NOx.Is_Used)
                allAlarm.SensorAlarm.NOx.Is_Used = true;
            else
                allAlarm.SensorAlarm.NOx.Is_Used = false;


            if (SensorRange.SO2.Is_Used)
                allAlarm.SensorAlarm.SO2.Is_Used = true;
            else
                allAlarm.SensorAlarm.SO2.Is_Used = false;


            if (SensorRange.Dust.Is_Used)
                allAlarm.SensorAlarm.Dust.Is_Used = true;
            else
                allAlarm.SensorAlarm.Dust.Is_Used = false;


            if (SensorRange.CH4.Is_Used)
                allAlarm.SensorAlarm.CH4.Is_Used = true;
            else
                allAlarm.SensorAlarm.CH4.Is_Used = false;


            if (SensorRange.H2S.Is_Used)
                allAlarm.SensorAlarm.H2S.Is_Used = true;
            else
                allAlarm.SensorAlarm.H2S.Is_Used = false;
        }



        public static async Task Add_20M_Async()
        {
            stopGetSernsorNow = true;                   //стопим запись данных в Array20Ms

            await Task.Delay(TimeSpan.FromSeconds(1));  //Выжидаем, что бы поток который прошёл успел остановится
            await Task.Run(() => Add_20M());

            stopGetSernsorNow = false;                  //Рарешаем работу записи
        }
        public static void Add_20M()
        {
            if (Array20Ms.Count > 0)
            {
                Random random = new Random();
                AVG_20_MINUTES new20M = new AVG_20_MINUTES();

                try
                {
                    new20M.Conc_CO = Math.Round(Array20Ms.Average(a => a.CO_Conc), 3);
                    new20M.Conc_CO2 = Math.Round(Array20Ms.Average(a => a.CO2_Conc), 3);
                    new20M.Conc_NO = Math.Round(Array20Ms.Average(a => a.NO_Conc), 3);
                    new20M.Conc_NO2 = Math.Round(Array20Ms.Average(a => a.NO2_Conc), 3);
                    new20M.Conc_NOx = Math.Round(Array20Ms.Average(a => a.NOx_Conc), 3);
                    new20M.Conc_SO2 = Math.Round(Array20Ms.Average(a => a.SO2_Conc), 3);
                    new20M.Conc_Dust = Math.Round(Array20Ms.Average(a => a.Dust_Conc), 3);
                    new20M.Conc_CH4 = Math.Round(Array20Ms.Average(a => a.CH4_Conc), 3);
                    new20M.Conc_H2S = Math.Round(Array20Ms.Average(a => a.H2S_Conc), 3);
                    new20M.Conc_D1 = Math.Round(Array20Ms.Average(a => a.Add_Conc_1), 3);
                    new20M.Conc_D2 = Math.Round(Array20Ms.Average(a => a.Add_Conc_2), 3);
                    new20M.Conc_D3 = Math.Round(Array20Ms.Average(a => a.Add_Conc_3), 3);
                    new20M.Conc_D4 = Math.Round(Array20Ms.Average(a => a.Add_Conc_4), 3);
                    new20M.Conc_D5 = Math.Round(Array20Ms.Average(a => a.Add_Conc_5), 3);

                    new20M.Emis_CO = Math.Round(Array20Ms.Average(a => a.CO_Emis), 3);
                    new20M.Emis_CO2 = Math.Round(Array20Ms.Average(a => a.CO2_Emis), 3);
                    new20M.Emis_NO = Math.Round(Array20Ms.Average(a => a.NO_Emis), 3);
                    new20M.Emis_NO2 = Math.Round(Array20Ms.Average(a => a.NO2_Emis), 3);
                    new20M.Emis_NOx = Math.Round(Array20Ms.Average(a => a.NOx_Emis), 3);
                    new20M.Emis_SO2 = Math.Round(Array20Ms.Average(a => a.SO2_Emis), 3);
                    new20M.Emis_CH4 = Math.Round(Array20Ms.Average(a => a.CH4_Emis), 3);
                    new20M.Emis_H2S = Math.Round(Array20Ms.Average(a => a.H2S_Emis), 3);
                    new20M.Emis_Dust = Math.Round(Array20Ms.Average(a => a.Dust_Emis), 3);
                    new20M.Emis_D1 = Math.Round(Array20Ms.Average(a => a.Add_Emis_1), 3);
                    new20M.Emis_D2 = Math.Round(Array20Ms.Average(a => a.Add_Emis_2), 3);
                    new20M.Emis_D3 = Math.Round(Array20Ms.Average(a => a.Add_Emis_3), 3);
                    new20M.Emis_D4 = Math.Round(Array20Ms.Average(a => a.Add_Emis_4), 3);
                    new20M.Emis_D5 = Math.Round(Array20Ms.Average(a => a.Add_Emis_5), 3);

                    new20M.O2_Wet = Math.Round(Array20Ms.Average(a => a.O2_Wet), 3);
                    new20M.O2_Dry = Math.Round(Array20Ms.Average(a => a.O2_Dry), 3);
                    new20M.H2O = Math.Round(Array20Ms.Average(a => a.H2O), 3);

                    new20M.Pressure = Math.Round(Array20Ms.Average(a => a.Pressure), 3);
                    new20M.Temperature = Math.Round(Array20Ms.Average(a => a.Temperature), 3);
                    new20M.Speed = Math.Round(Array20Ms.Average(a => a.Speed), 3);
                    new20M.Flow = Math.Round(Array20Ms.Average(a => a.Flow), 3);
                    new20M.Temperature_KIP = Math.Round(Array20Ms.Average(a => a.Temperature_KIP), 3);
                    new20M.Temperature_NOx = Math.Round(Array20Ms.Average(a => a.Temperature_NOx), 3);

                    if (globalAlarms.Is_Stop.Is_Used) new20M.Mode_ASK = 1;
                    else
                    {
                        if (globalAlarms.Is_NotProcess.Is_Used) 
                            new20M.Mode_ASK = 2;
                        else
                            new20M.Mode_ASK = 0;
                    }
                    new20M.PDZ_Fuel = PDZ.Is_Active;

                }
                catch
                {

                }
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        AVG_20_MINUTES_Repository avg_20_M_Service = new AVG_20_MINUTES_Repository(db);
                        avg_20_M_Service.Create_AVG_20_MINUTES(new AVG_20_MINUTES()
                        {
                            Date = DateTime.Now,

                            //Временно 
                            Conc_CO = new20M.Conc_CO,
                            Conc_CO2 = new20M.Conc_CO2,
                            Conc_NO = new20M.Conc_NO,
                            Conc_NO2 = new20M.Conc_NO2,
                            Conc_NOx = new20M.Conc_NOx,
                            Conc_SO2 = new20M.Conc_SO2,
                            Conc_Dust = new20M.Conc_Dust,
                            Conc_CH4 = new20M.Conc_CH4,
                            Conc_H2S = new20M.Conc_H2S,
                            Conc_D1 = new20M.Conc_D1,
                            Conc_D2 = new20M.Conc_D2,
                            Conc_D3 = new20M.Conc_D3,
                            Conc_D4 = new20M.Conc_D4,
                            Conc_D5 = new20M.Conc_D5,

                            Emis_CO = new20M.Emis_CO,
                            Emis_CO2 = new20M.Emis_CO2,
                            Emis_NO = new20M.Emis_NO,
                            Emis_NO2 = new20M.Emis_NO2,
                            Emis_NOx = new20M.Emis_NOx,
                            Emis_SO2 = new20M.Emis_SO2,
                            Emis_CH4 = new20M.Emis_CH4,
                            Emis_H2S = new20M.Emis_H2S,
                            Emis_Dust = new20M.Emis_Dust,
                            Emis_D1 = new20M.Emis_D1,
                            Emis_D2 = new20M.Emis_D2,
                            Emis_D3 = new20M.Emis_D3,
                            Emis_D4 = new20M.Emis_D4,
                            Emis_D5 = new20M.Emis_D5,

                            O2_Wet = new20M.O2_Wet,
                            O2_Dry = new20M.O2_Dry,
                            H2O = new20M.H2O,

                            Pressure = new20M.Pressure,
                            Temperature = new20M.Temperature,
                            Speed = new20M.Speed,
                            Flow = new20M.Flow,
                            Temperature_KIP = new20M.Temperature_KIP,
                            Temperature_NOx = new20M.Temperature_NOx,

                            Mode_ASK = new20M.Mode_ASK,
                            PDZ_Fuel = new20M.PDZ_Fuel
                        });
                    }
                    catch
                    {

                    }
                    //GlobalStaticSettingsASK.VisibilityReportOptions.data_add_20M = GlobalStaticSettingsASK.VisibilityReportOptions.data_add_20M.AddMinutes(20);
                }
            }
            Array20Ms.Clear();
        }



        public static async Task DeleteOldSensor_4_20m_Async()
        {
            await Task.Run(() => DeleteOldSensor_4_20m());
        }



        public static void DeleteOldSensor_4_20m()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    SENSOR_4_20_10sec_Repository Sensors_4_20db = new SENSOR_4_20_10sec_Repository(db);
                    Sensors_4_20db.DeleteOld(DateTime.Now.AddDays(-1));
                }
                catch
                {

                }
            }
        }



        public static async Task GetNow_ConcEmisAsync()
        {
            await Task.Run(() => GetNow_ConcEmis());
        }



        public static void GetNow_ConcEmis()
        {
            GetSensorNow();
            RunConvertSernsor();
            Normalization_ConcEmis();

            if (delayedStart > 0)
                ChekSensorBreak();
            else
                delayedStart++;

            Sensor_4_20s.Add((Sensor_4_20_Model)SensorNow.Clone());

            if (Sensor_4_20s.Count > 999)
                Sensor_4_20s.RemoveAt(0);

            if (!stopGetSernsorNow)
                Array20Ms.Add((Array20M_Model)CurrentConcEmis.Clone());

            if (CounterChart > 8)
            {
                CounterChart = 0;

                //ChartCurrent.Getsimulation();
                if (ChartList.Count > 699)
                    ChartList.RemoveAt(0);

                //График
                ChartCurrent.DateString = DateTime.Now.ToLongTimeString();

                ChartCurrent.CO = CurrentConcEmis.CO_Conc;
                ChartCurrent.CO2 = CurrentConcEmis.CO2_Conc;
                ChartCurrent.NO = CurrentConcEmis.NO_Conc;
                ChartCurrent.NO2 = CurrentConcEmis.NO2_Conc;
                ChartCurrent.NOx = CurrentConcEmis.NOx_Conc;
                ChartCurrent.SO2 = CurrentConcEmis.SO2_Conc;
                ChartCurrent.Dust = CurrentConcEmis.Dust_Conc;
                ChartCurrent.CH4 = CurrentConcEmis.CH4_Conc;
                ChartCurrent.H2S = CurrentConcEmis.H2S_Conc;
                ChartCurrent.Add_Conc_1 = CurrentConcEmis.Add_Conc_1;
                ChartCurrent.Add_Conc_2 = CurrentConcEmis.Add_Conc_2;
                ChartCurrent.Add_Conc_3 = CurrentConcEmis.Add_Conc_3;
                ChartCurrent.Add_Conc_4 = CurrentConcEmis.Add_Conc_4;
                ChartCurrent.Add_Conc_5 = CurrentConcEmis.Add_Conc_5;

                ChartCurrent.O2_Wet = CurrentConcEmis.O2_Wet;
                ChartCurrent.O2_Dry = CurrentConcEmis.O2_Dry;

                ChartList.Add((Chart_CurrentValue)ChartCurrent.Clone());

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        SENSOR_4_20_10sec_Repository Sensors_4_20db = new SENSOR_4_20_10sec_Repository(db);
                        Sensors_4_20db.Create_SENSOR_4_20_10sec(new SENSOR_4_20_10sec
                        {
                            Date = SensorNow.Date,

                            CO = SensorNow.CO_4_20mA,
                            CO2 = SensorNow.CO2_4_20mA,
                            NO = SensorNow.NO_4_20mA,
                            NO2 = SensorNow.NO2_4_20mA,
                            NOx = SensorNow.NOx_4_20mA,
                            SO2 = SensorNow.SO2_4_20mA,
                            Dust = SensorNow.Dust_4_20mA,
                            CH4 = SensorNow.CH4_4_20mA,
                            H2S = SensorNow.H2S_4_20mA,

                            Rezerv_1 = SensorNow.Rezerv_1_4_20mA,
                            Rezerv_2 = SensorNow.Rezerv_2_4_20mA,
                            Rezerv_3 = SensorNow.Rezerv_3_4_20mA,
                            Rezerv_4 = SensorNow.Rezerv_4_4_20mA,
                            Rezerv_5 = SensorNow.Rezerv_5_4_20mA,

                            O2_Wet = SensorNow.O2_Wet_4_20mA,
                            O2_Dry = SensorNow.O2_Dry_4_20mA,
                            H2O = SensorNow.H2S_4_20mA,

                            Pressure = SensorNow.Pressure_4_20mA,
                            Temperature = SensorNow.Temperature_4_20mA,
                            Speed = SensorNow.Speed_4_20mA,

                            Temperature_KIP = SensorNow.Temperature_KIP_4_20mA,
                            Temperature_NOx = SensorNow.Temperature_NOx_4_20mA
                        });
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                CounterChart++;
            }
        }



        public static async Task Add_PDZ_Async()
        {
            await Task.Run(() => Add_PDZ());
        }



        public static void Add_PDZ()
        {
            GetCurrentPDZ();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<PDZ> PDZs = new List<PDZ>();
                PDZ_Repository pdz_Service = new PDZ_Repository(db);

                if (!pdz_Service.FindPDZDay())
                {
                    bool buff_IsActive;

                    if (PDZ.Is_Active == 1)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_1_CO_Conc,
                        CO2_Conc = PDZ.PDZ_1_CO2_Conc,
                        NO_Conc = PDZ.PDZ_1_NO_Conc,
                        NO2_Conc = PDZ.PDZ_1_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_1_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_1_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_1_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_1_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_1_H2S_Conc,
                        Add_Conc_1 = PDZ.PDZ_1_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_1_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_1_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_1_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_1_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_1_CO_Emis,
                        CO2_Emis = PDZ.PDZ_1_CO2_Emis,
                        NO_Emis = PDZ.PDZ_1_NO_Emis,
                        NO2_Emis = PDZ.PDZ_1_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_1_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_1_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_1_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_1_H2S_Emis,
                        Dust_Emis = PDZ.PDZ_1_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_1_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_1_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_1_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_1_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_1_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_1_Number,

                        Current = buff_IsActive
                    });

                    if (PDZ.Is_Active == 2)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_2_CO_Conc,
                        CO2_Conc = PDZ.PDZ_2_CO2_Conc,
                        NO_Conc = PDZ.PDZ_2_NO_Conc,
                        NO2_Conc = PDZ.PDZ_2_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_2_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_2_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_2_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_2_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_2_H2S_Conc,
                        Add_Conc_1 = PDZ.PDZ_2_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_2_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_2_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_2_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_2_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_2_CO_Emis,
                        CO2_Emis = PDZ.PDZ_2_CO2_Emis,
                        NO_Emis = PDZ.PDZ_2_NO_Emis,
                        NO2_Emis = PDZ.PDZ_2_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_2_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_2_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_2_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_2_H2S_Emis,
                        Dust_Emis = PDZ.PDZ_2_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_2_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_2_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_2_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_2_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_2_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_2_Number,

                        Current = buff_IsActive
                    });

                    if (PDZ.Is_Active == 3)
                        buff_IsActive = true;
                    else
                        buff_IsActive = false;

                    pdz_Service.Create_PDZ(new PDZ()
                    {
                        Date = DateTime.Now,

                        CO_Conc = PDZ.PDZ_3_CO_Conc,
                        CO2_Conc = PDZ.PDZ_3_CO2_Conc,
                        NO_Conc = PDZ.PDZ_3_NO_Conc,
                        NO2_Conc = PDZ.PDZ_3_NO2_Conc,
                        NOx_Conc = PDZ.PDZ_3_NOx_Conc,
                        SO2_Conc = PDZ.PDZ_3_SO2_Conc,
                        Dust_Conc = PDZ.PDZ_3_Dust_Conc,
                        CH4_Conc = PDZ.PDZ_3_CH4_Conc,
                        H2S_Conc = PDZ.PDZ_3_H2S_Conc,
                        Add_Conc_1 = PDZ.PDZ_3_Add_Conc_1,
                        Add_Conc_2 = PDZ.PDZ_3_Add_Conc_2,
                        Add_Conc_3 = PDZ.PDZ_3_Add_Conc_3,
                        Add_Conc_4 = PDZ.PDZ_3_Add_Conc_4,
                        Add_Conc_5 = PDZ.PDZ_3_Add_Conc_5,

                        CO_Emis = PDZ.PDZ_3_CO_Emis,
                        CO2_Emis = PDZ.PDZ_3_CO2_Emis,
                        NO_Emis = PDZ.PDZ_3_NO_Emis,
                        NO2_Emis = PDZ.PDZ_3_NO2_Emis,
                        NOx_Emis = PDZ.PDZ_3_NOx_Emis,
                        SO2_Emis = PDZ.PDZ_3_SO2_Emis,
                        CH4_Emis = PDZ.PDZ_3_CH4_Emis,
                        H2S_Emis = PDZ.PDZ_3_H2S_Emis,
                        Dust_Emis = PDZ.PDZ_3_Dust_Emis,
                        Add_Emis_1 = PDZ.PDZ_3_Add_Emis_1,
                        Add_Emis_2 = PDZ.PDZ_3_Add_Emis_2,
                        Add_Emis_3 = PDZ.PDZ_3_Add_Emis_3,
                        Add_Emis_4 = PDZ.PDZ_3_Add_Emis_4,
                        Add_Emis_5 = PDZ.PDZ_3_Add_Emis_5,

                        NumberPDZ = PDZ.PDZ_3_Number,

                        Current = buff_IsActive
                    });
                }
            }
        }



        public static void GetSensorNow()
        {
            string IpAdres = "192.168.1.153";

            

            try
            {
                connected = ping.Send(IpAdres, 10);    //Проверяем соедение с таймингом 0.5 сек

                ushort[] registers;                     //Будующий масиив считываемых WORD из ПЛК

                if (connected.Status == IPStatus.Success)
                {
                    if (globalAlarms.Is_NotConnection.Value)
                    {
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                            globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_NotConnection);
                        }
                        globalAlarms.Is_NotConnection.Value = false;
                    }

                    using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        using (TcpClient client = new TcpClient(IpAdres, 502))
                        {
                            var factory = new ModbusFactory();
                            IModbusMaster master = factory.CreateMaster(client);

                            byte slaveId = 1;
                            ushort startAddress = 32000;
                            ushort numInputs = 44;

                            registers = master.ReadInputRegisters(slaveId, startAddress, numInputs);
                        }
                    }
                    int i = 0; //Первый байт
                    int j = 1; //Второй байт

                    SensorNow.Date = DateTime.Now;

                    //CO_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.CO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //CO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.CO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NO_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.NO_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.NO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //NOx_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //SO2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.SO2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Dust_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Dust_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //CH4_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.CH4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //H2S_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.H2S_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }


                    i += 2;
                    j += 2;
                    //Rezerv_1_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Rezerv_1_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_2_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Rezerv_2_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_3_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Rezerv_3_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_4_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Rezerv_4_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Rezerv_5_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Rezerv_5_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2_Wet_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.O2_Wet_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //O2_Dry_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.O2_Dry_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //H2O_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.H2O_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Pressure_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Pressure_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Temperature_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Temperature_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Speed_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Speed_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }

                    i += 2;
                    j += 2;
                    //Temperature_KIP_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Temperature_KIP_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }


                    i += 2;
                    j += 2;
                    //Temperature_NOx_4-20mA
                    if (!float.IsNaN(NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i])))
                    {
                        SensorNow.Temperature_NOx_4_20mA = Math.Round((NModbus.Utility.ModbusUtility.GetSingle(registers[j], registers[i]) / 1000), 3);
                    }
                }
                else
                {
                    if (!globalAlarms.Is_NotConnection.Value)
                    {
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                            globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotConnection);
                        }
                        globalAlarms.Is_NotConnection.Value = true;
                    }

                    SensorNow.Date = DateTime.Now;

                    //Режим симуляции
                    if (is_simulation)
                    {
                        SensorNow.CO_4_20mA = Simulation_4_20(SensorNow.CO_4_20mA);
                        SensorNow.CO2_4_20mA = Simulation_4_20(SensorNow.CO2_4_20mA);
                        SensorNow.NO_4_20mA = Simulation_4_20(SensorNow.NO_4_20mA);
                        SensorNow.NO2_4_20mA = Simulation_4_20(SensorNow.NO2_4_20mA);
                        SensorNow.NOx_4_20mA = Simulation_4_20(SensorNow.NOx_4_20mA);
                        SensorNow.SO2_4_20mA = Simulation_4_20(SensorNow.SO2_4_20mA);
                        SensorNow.Dust_4_20mA = Simulation_4_20(SensorNow.Dust_4_20mA);
                        SensorNow.CH4_4_20mA = Simulation_4_20(SensorNow.CH4_4_20mA);
                        SensorNow.H2S_4_20mA = Simulation_4_20(SensorNow.H2S_4_20mA);

                        SensorNow.Rezerv_1_4_20mA = Simulation_4_20(SensorNow.Rezerv_1_4_20mA); 
                        SensorNow.Rezerv_2_4_20mA = Simulation_4_20(SensorNow.Rezerv_2_4_20mA);
                        SensorNow.Rezerv_3_4_20mA = Simulation_4_20(SensorNow.Rezerv_3_4_20mA);
                        SensorNow.Rezerv_4_4_20mA = Simulation_4_20(SensorNow.Rezerv_3_4_20mA);
                        SensorNow.Rezerv_5_4_20mA = Simulation_4_20(SensorNow.Rezerv_5_4_20mA);

                        SensorNow.O2_Wet_4_20mA = Simulation_4_20(SensorNow.O2_Wet_4_20mA, true);
                        SensorNow.O2_Dry_4_20mA = SensorNow.O2_Wet_4_20mA;
                        O2_Last = SensorNow.O2_Wet_4_20mA;
                        SensorNow.H2O_4_20mA = Simulation_4_20(SensorNow.H2O_4_20mA);

                        SensorNow.Pressure_4_20mA = Simulation_4_20(SensorNow.Pressure_4_20mA);
                        SensorNow.Temperature_4_20mA = Simulation_4_20(SensorNow.Temperature_4_20mA);
                        SensorNow.Speed_4_20mA = Simulation_4_20(SensorNow.Speed_4_20mA);

                        SensorNow.Temperature_KIP_4_20mA = Simulation_4_20(SensorNow.Temperature_KIP_4_20mA);
                        SensorNow.Temperature_NOx_4_20mA = Simulation_4_20(SensorNow.Temperature_NOx_4_20mA);
                    }
                    else
                    {
                        SensorNow.CO_4_20mA = 0.0;
                        SensorNow.CO2_4_20mA = 0.0;
                        SensorNow.NO_4_20mA = 0.0;
                        SensorNow.NO2_4_20mA = 0.0;
                        SensorNow.NOx_4_20mA = 0.0;
                        SensorNow.SO2_4_20mA = 0.0;
                        SensorNow.Dust_4_20mA = 0.0;
                        SensorNow.CH4_4_20mA = 0.0;
                        SensorNow.H2S_4_20mA = 0.0;

                        SensorNow.Rezerv_1_4_20mA = 0.0;
                        SensorNow.Rezerv_2_4_20mA = 0.0;
                        SensorNow.Rezerv_3_4_20mA = 0.0;
                        SensorNow.Rezerv_4_4_20mA = 0.0;
                        SensorNow.Rezerv_5_4_20mA = 0.0;

                        SensorNow.O2_Wet_4_20mA = 0.0;
                        SensorNow.O2_Dry_4_20mA = 0.0;
                        SensorNow.H2O_4_20mA = 0.0;

                        SensorNow.Pressure_4_20mA = 0.0;
                        SensorNow.Temperature_4_20mA = 0.0;
                        SensorNow.Speed_4_20mA = 0.0;

                        SensorNow.Temperature_KIP_4_20mA = 0.0;
                        SensorNow.Temperature_NOx_4_20mA = 0.0;
                    }
                }
            }
            catch
            {
                if (!globalAlarms.Is_NotConnection.Value)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotConnection);
                    }
                    globalAlarms.Is_NotConnection.Value = true;
                }

                SensorNow.Date = DateTime.Now;

                SensorNow.CO_4_20mA = 0.0;
                SensorNow.CO2_4_20mA = 0.0;
                SensorNow.NO_4_20mA = 0.0;
                SensorNow.NO2_4_20mA = 0.0;
                SensorNow.NOx_4_20mA = 0.0;
                SensorNow.SO2_4_20mA = 0.0;
                SensorNow.Dust_4_20mA = 0.0;
                SensorNow.CH4_4_20mA = 0.0;
                SensorNow.H2S_4_20mA = 0.0;

                SensorNow.Rezerv_1_4_20mA = 0.0;
                SensorNow.Rezerv_2_4_20mA = 0.0;
                SensorNow.Rezerv_3_4_20mA = 0.0;
                SensorNow.Rezerv_4_4_20mA = 0.0;
                SensorNow.Rezerv_5_4_20mA = 0.0;

                SensorNow.O2_Wet_4_20mA = 0.0;
                SensorNow.O2_Dry_4_20mA = 0.0;
                SensorNow.H2O_4_20mA = 0.0;

                SensorNow.Pressure_4_20mA = 0.0;
                SensorNow.Temperature_4_20mA = 0.0;
                SensorNow.Speed_4_20mA = 0.0;

                SensorNow.Temperature_KIP_4_20mA = 0.0;
                SensorNow.Temperature_NOx_4_20mA = 0.0;
            }
        }



        //Проверяем датчики на обрыв
        public static void ChekSensorBreak() //Оптимизировать весь процесс с нуля! Есть проблемы!
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var globalAlarm_Services = new GlobalAlarm_Services(new ACCIDENT_LOG_Repository(db));

                //Обрыв датчиков
                if (SensorRange.CO.Is_Used)
                    if (SensorNow.CO_4_20mA < SensorRange.CO.mA.Min || SensorNow.CO_4_20mA > SensorRange.CO.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.CO.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.CO);

                            allAlarm.SensorAlarm.CO.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.CO.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.CO);

                            allAlarm.SensorAlarm.CO.Value = false;
                        }
                    }

                if (SensorRange.CO2.Is_Used)
                    if (SensorNow.CO2_4_20mA < SensorRange.CO2.mA.Min || SensorNow.CO2_4_20mA > SensorRange.CO2.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.CO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.CO2);

                            allAlarm.SensorAlarm.CO2.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.CO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.CO);

                            allAlarm.SensorAlarm.CO2.Value = false;
                        }
                    }

                if (SensorRange.NO.Is_Used)
                    if (SensorNow.NO_4_20mA < SensorRange.NO.mA.Min || SensorNow.NO_4_20mA > SensorRange.NO.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.NO.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.NO);

                            allAlarm.SensorAlarm.NO.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.NO.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.NO);

                            allAlarm.SensorAlarm.NO.Value = false;
                        }
                    }

                if (SensorRange.NO2.Is_Used)
                    if (SensorNow.NO2_4_20mA < SensorRange.NO2.mA.Min || SensorNow.NO2_4_20mA > SensorRange.NO2.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.NO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.NO2);

                            allAlarm.SensorAlarm.NO2.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.NO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.NO2);

                            allAlarm.SensorAlarm.NO2.Value = false;
                        }
                    }

                if (SensorRange.NOx.Is_Used)
                    if (SensorNow.NOx_4_20mA < SensorRange.NOx.mA.Min || SensorNow.NOx_4_20mA > SensorRange.NOx.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.NOx.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.NOx);

                            allAlarm.SensorAlarm.NOx.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.NOx.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.NOx);

                            allAlarm.SensorAlarm.NOx.Value = false;
                        }
                    }

                if (SensorRange.SO2.Is_Used)
                    if (SensorNow.SO2_4_20mA < SensorRange.SO2.mA.Min || SensorNow.SO2_4_20mA > SensorRange.SO2.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.SO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.SO2);

                            allAlarm.SensorAlarm.SO2.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.SO2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.SO2);

                            allAlarm.SensorAlarm.SO2.Value = false;
                        }
                    }

                if (SensorRange.Dust.Is_Used)
                    if (SensorNow.Dust_4_20mA < SensorRange.Dust.mA.Min || SensorNow.Dust_4_20mA > SensorRange.Dust.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Dust.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Dust);

                            allAlarm.SensorAlarm.Dust.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Dust.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Dust);

                            allAlarm.SensorAlarm.Dust.Value = false;
                        }
                    }

                if (SensorRange.CH4.Is_Used)
                    if (SensorNow.CH4_4_20mA < SensorRange.CH4.mA.Min || SensorNow.CH4_4_20mA > SensorRange.CH4.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.CH4.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.CH4);

                            allAlarm.SensorAlarm.CH4.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.CH4.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.CH4);

                            allAlarm.SensorAlarm.CH4.Value = false;
                        }
                    }

                if (SensorRange.H2S.Is_Used)
                    if (SensorNow.H2S_4_20mA < SensorRange.H2S.mA.Min || SensorNow.H2S_4_20mA > SensorRange.H2S.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.H2S.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.H2S);

                            allAlarm.SensorAlarm.H2S.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.H2S.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.H2S);

                            allAlarm.SensorAlarm.H2S.Value = false;
                        }
                    }

                if (SensorRange.Rezerv_1.Is_Used)
                    if (SensorNow.Rezerv_1_4_20mA < SensorRange.Rezerv_1.mA.Min || SensorNow.Rezerv_1_4_20mA > SensorRange.Rezerv_1.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Rezerv_1.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Rezerv_1);

                            allAlarm.SensorAlarm.Rezerv_1.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Rezerv_1.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Rezerv_1);

                            allAlarm.SensorAlarm.Rezerv_1.Value = false;
                        }
                    }

                if (SensorRange.Rezerv_2.Is_Used)
                    if (SensorNow.Rezerv_2_4_20mA < SensorRange.Rezerv_2.mA.Min || SensorNow.Rezerv_2_4_20mA > SensorRange.Rezerv_2.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Rezerv_2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Rezerv_2);

                            allAlarm.SensorAlarm.Rezerv_2.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Rezerv_2.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Rezerv_2);

                            allAlarm.SensorAlarm.Rezerv_2.Value = false;
                        }
                    }

                if (SensorRange.Rezerv_3.Is_Used)
                    if (SensorNow.Rezerv_3_4_20mA < SensorRange.Rezerv_3.mA.Min || SensorNow.Rezerv_3_4_20mA > SensorRange.Rezerv_3.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Rezerv_3.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Rezerv_3);

                            allAlarm.SensorAlarm.Rezerv_3.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Rezerv_3.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Rezerv_3);

                            allAlarm.SensorAlarm.Rezerv_3.Value = false;
                        }
                    }

                if (SensorRange.Rezerv_4.Is_Used)
                    if (SensorNow.Rezerv_4_4_20mA < SensorRange.Rezerv_4.mA.Min || SensorNow.Rezerv_4_4_20mA > SensorRange.Rezerv_4.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Rezerv_4.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Rezerv_4);

                            allAlarm.SensorAlarm.Rezerv_4.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Rezerv_4.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Rezerv_4);

                            allAlarm.SensorAlarm.Rezerv_4.Value = false;
                        }
                    }

                if (SensorRange.Rezerv_5.Is_Used)
                    if (SensorNow.Rezerv_5_4_20mA < SensorRange.Rezerv_5.mA.Min || SensorNow.Rezerv_5_4_20mA > SensorRange.Rezerv_5.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Rezerv_5.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Rezerv_5);

                            allAlarm.SensorAlarm.Rezerv_5.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Rezerv_5.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Rezerv_5);

                            allAlarm.SensorAlarm.Rezerv_5.Value = false;
                        }
                    }

                if (SensorRange.O2Wet.Is_Used)
                    if (SensorNow.O2_Wet_4_20mA < SensorRange.O2Wet.mA.Min || SensorNow.O2_Wet_4_20mA > SensorRange.O2Wet.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.O2_Wet.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.O2_Wet);

                            allAlarm.SensorAlarm.O2_Wet.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.O2_Wet.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.O2_Wet);

                            allAlarm.SensorAlarm.O2_Wet.Value = false;
                        }
                    }

                if (SensorRange.O2Dry.Is_Used)
                    if (SensorNow.O2_Dry_4_20mA < SensorRange.O2Dry.mA.Min || SensorNow.O2_Dry_4_20mA > SensorRange.O2Dry.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.O2_Dry.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.O2_Dry);

                            allAlarm.SensorAlarm.O2_Dry.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.O2_Dry.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.O2_Dry);

                            allAlarm.SensorAlarm.O2_Dry.Value = false;
                        }
                    }

                if (SensorRange.H2O.Is_Used)
                    if (SensorNow.H2O_4_20mA < SensorRange.H2O.mA.Min || SensorNow.H2O_4_20mA > SensorRange.H2O.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.H2O.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.H2O);

                            allAlarm.SensorAlarm.H2O.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.H2O.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.H2O);

                            allAlarm.SensorAlarm.H2O.Value = false;
                        }
                    }

                if (SensorRange.Pressure.Is_Used)
                    if (SensorNow.Pressure_4_20mA < SensorRange.Pressure.mA.Min || SensorNow.Pressure_4_20mA > SensorRange.Pressure.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Pressure.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Pressure);

                            allAlarm.SensorAlarm.Pressure.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Pressure.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Pressure);

                            allAlarm.SensorAlarm.Pressure.Value = false;
                        }
                    }

                if (SensorRange.Speed.Is_Used)
                    if (SensorNow.Speed_4_20mA < SensorRange.Speed.mA.Min || SensorNow.Speed_4_20mA > SensorRange.Speed.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Speed.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Speed);

                            allAlarm.SensorAlarm.Speed.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Speed.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Speed);

                            allAlarm.SensorAlarm.Speed.Value = false;
                        }
                    }

                if (SensorRange.Temperature.Is_Used)
                    if (SensorNow.Temperature_4_20mA < SensorRange.Temperature.mA.Min || SensorNow.Temperature_4_20mA > SensorRange.Temperature.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Temperature.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Temperature);

                            allAlarm.SensorAlarm.Temperature.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Temperature.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Temperature);

                            allAlarm.SensorAlarm.Temperature.Value = false;
                        }
                    }

                if (SensorRange.Temperature_KIP.Is_Used)
                    if (SensorNow.Temperature_KIP_4_20mA < SensorRange.Temperature_KIP.mA.Min || SensorNow.Temperature_KIP_4_20mA > SensorRange.Temperature_KIP.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Temperature_KIP.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Temperature_KIP);

                            allAlarm.SensorAlarm.Temperature_KIP.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Temperature_KIP.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Temperature_KIP);

                            allAlarm.SensorAlarm.Temperature_KIP.Value = false;
                        }
                    }

                if (SensorRange.Temperature_NOx.Is_Used)
                    if (SensorNow.Temperature_NOx_4_20mA < SensorRange.Temperature_NOx.mA.Min || SensorNow.Temperature_NOx_4_20mA > SensorRange.Temperature_NOx.mA.Max)
                    {
                        if (!allAlarm.SensorAlarm.Temperature_NOx.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, allAlarm.SensorAlarm.Temperature_NOx);

                            allAlarm.SensorAlarm.Temperature_NOx.Value = true;
                        }
                    }
                    else
                    {
                        if (allAlarm.SensorAlarm.Temperature_NOx.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, allAlarm.SensorAlarm.Temperature_NOx);

                            allAlarm.SensorAlarm.Temperature_NOx.Value = false;
                        }
                    }




                //Глобальные аварии и уведомления

                //Общая авария
                if ((allAlarm.SensorAlarm.CO.Value && allAlarm.SensorAlarm.CO.Is_Critical) ||
                    (allAlarm.SensorAlarm.CO2.Value && allAlarm.SensorAlarm.CO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO.Value && allAlarm.SensorAlarm.NO.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO2.Value && allAlarm.SensorAlarm.NO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NOx.Value && allAlarm.SensorAlarm.NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.SO2.Value && allAlarm.SensorAlarm.SO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Dust.Value && allAlarm.SensorAlarm.Dust.Is_Critical) ||
                    (allAlarm.SensorAlarm.CH4.Value && allAlarm.SensorAlarm.CH4.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2S.Value && allAlarm.SensorAlarm.H2S.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_1.Value && allAlarm.SensorAlarm.Rezerv_1.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_2.Value && allAlarm.SensorAlarm.Rezerv_2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_3.Value && allAlarm.SensorAlarm.Rezerv_3.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_4.Value && allAlarm.SensorAlarm.Rezerv_4.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_5.Value && allAlarm.SensorAlarm.Rezerv_5.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Wet.Value && allAlarm.SensorAlarm.O2_Wet.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Dry.Value && allAlarm.SensorAlarm.O2_Dry.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2O.Value && allAlarm.SensorAlarm.H2O.Is_Critical) ||
                    (allAlarm.SensorAlarm.Pressure.Value && allAlarm.SensorAlarm.Pressure.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature.Value && allAlarm.SensorAlarm.Temperature.Is_Critical) ||
                    (allAlarm.SensorAlarm.Speed.Value && allAlarm.SensorAlarm.Speed.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_KIP.Value && allAlarm.SensorAlarm.Temperature_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_NOx.Value && allAlarm.SensorAlarm.Temperature_NOx.Is_Critical))
                {
                    if (!globalAlarms.Is_Error.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Error);

                        globalAlarms.Is_Error.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Error.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Error);

                        globalAlarms.Is_Error.Value = false;
                    }
                }


                //Информационное сообщение
                if ((allAlarm.SensorAlarm.CO.Value && !allAlarm.SensorAlarm.CO.Is_Critical) ||
                    (allAlarm.SensorAlarm.CO2.Value && !allAlarm.SensorAlarm.CO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO.Value && !allAlarm.SensorAlarm.NO.Is_Critical) ||
                    (allAlarm.SensorAlarm.NO2.Value && !allAlarm.SensorAlarm.NO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.NOx.Value && !allAlarm.SensorAlarm.NOx.Is_Critical) ||
                    (allAlarm.SensorAlarm.SO2.Value && !allAlarm.SensorAlarm.SO2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Dust.Value && !allAlarm.SensorAlarm.Dust.Is_Critical) ||
                    (allAlarm.SensorAlarm.CH4.Value && !allAlarm.SensorAlarm.CH4.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2S.Value && !allAlarm.SensorAlarm.H2S.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_1.Value && !allAlarm.SensorAlarm.Rezerv_1.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_2.Value && !allAlarm.SensorAlarm.Rezerv_2.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_3.Value && !allAlarm.SensorAlarm.Rezerv_3.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_4.Value && !allAlarm.SensorAlarm.Rezerv_4.Is_Critical) ||
                    (allAlarm.SensorAlarm.Rezerv_5.Value && !allAlarm.SensorAlarm.Rezerv_5.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Wet.Value && !allAlarm.SensorAlarm.O2_Wet.Is_Critical) ||
                    (allAlarm.SensorAlarm.O2_Dry.Value && !allAlarm.SensorAlarm.O2_Dry.Is_Critical) ||
                    (allAlarm.SensorAlarm.H2O.Value && !allAlarm.SensorAlarm.H2O.Is_Critical) ||
                    (allAlarm.SensorAlarm.Pressure.Value && !allAlarm.SensorAlarm.Pressure.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature.Value && !allAlarm.SensorAlarm.Temperature.Is_Critical) ||
                    (allAlarm.SensorAlarm.Speed.Value && !allAlarm.SensorAlarm.Speed.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_KIP.Value && !allAlarm.SensorAlarm.Temperature_KIP.Is_Critical) ||
                    (allAlarm.SensorAlarm.Temperature_NOx.Value && !allAlarm.SensorAlarm.Temperature_NOx.Is_Critical))
                {
                    if (!globalAlarms.Is_Info.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Error);

                        globalAlarms.Is_Info.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Info.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Error);

                        globalAlarms.Is_Info.Value = false;
                    }
                }


                //Простой  //Условия простоя (на данный момент, есть авария - ппростой)
                if (globalAlarms.Is_Error.Value)
                {
                    if (!globalAlarms.Is_Stop.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Stop);

                        globalAlarms.Is_Stop.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Stop.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Stop);

                        globalAlarms.Is_Stop.Value = false;
                    }
                }




                //Нет выбросов  //Пока смотрим только по кислороду (сухому, если нет сухого по влажн)
                if (SensorRange.O2Dry.Is_Used)
                {
                    if (CurrentConcEmis.O2_Dry >= 20.0 && !globalAlarms.Is_Stop.Value && !allAlarm.SensorAlarm.O2_Dry.Value)
                    {
                        if (!globalAlarms.Is_NotProcess.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotProcess);

                            globalAlarms.Is_NotProcess.Value = true;
                        }
                    }
                    else
                    {
                        if (globalAlarms.Is_NotProcess.Value)
                        {
                            globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_NotProcess);

                            globalAlarms.Is_NotProcess.Value = false;
                        }
                    }
                }
                else
                {
                    if (SensorRange.O2Wet.Is_Used)
                    {
                        if (CurrentConcEmis.O2_Wet >= 20.0 && !globalAlarms.Is_Stop.Value && !allAlarm.SensorAlarm.O2_Wet.Value)
                        {
                            if (!globalAlarms.Is_NotProcess.Value)
                            {
                                globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_NotProcess);

                                globalAlarms.Is_NotProcess.Value = true;
                            }
                        }
                        else
                        {
                            if (globalAlarms.Is_NotProcess.Value)
                            {
                                globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_NotProcess);

                                globalAlarms.Is_NotProcess.Value = false;
                            }
                        }
                    }
                }



                //превышение  //Превышение ПДК
                if (CurrentConcEmis.CO_Conc > PDZ_Current.CO_Conc || CurrentConcEmis.CO_Emis > PDZ_Current.CO_Emis ||
                    CurrentConcEmis.CO2_Conc > PDZ_Current.CO2_Conc || CurrentConcEmis.CO2_Emis > PDZ_Current.CO2_Emis ||
                    CurrentConcEmis.NO_Conc > PDZ_Current.NO_Conc || CurrentConcEmis.NO_Emis > PDZ_Current.NO_Emis ||
                    CurrentConcEmis.NO2_Conc > PDZ_Current.NO2_Conc || CurrentConcEmis.NO2_Emis > PDZ_Current.NO2_Emis ||
                    CurrentConcEmis.NOx_Conc > PDZ_Current.NOx_Conc || CurrentConcEmis.NOx_Emis > PDZ_Current.NOx_Emis ||
                    CurrentConcEmis.Dust_Conc > PDZ_Current.Dust_Conc || CurrentConcEmis.Dust_Emis > PDZ_Current.Dust_Emis ||
                    CurrentConcEmis.CH4_Conc > PDZ_Current.CH4_Conc || CurrentConcEmis.CH4_Emis > PDZ_Current.CH4_Emis ||
                    CurrentConcEmis.H2S_Conc > PDZ_Current.H2S_Conc || CurrentConcEmis.H2S_Emis > PDZ_Current.H2S_Emis ||
                    CurrentConcEmis.Add_Conc_1 > PDZ_Current.Add_Conc_1 || CurrentConcEmis.Add_Emis_1 > PDZ_Current.Add_Emis_1 ||
                    CurrentConcEmis.Add_Conc_2 > PDZ_Current.Add_Conc_2 || CurrentConcEmis.Add_Emis_2 > PDZ_Current.Add_Emis_2 ||
                    CurrentConcEmis.Add_Conc_3 > PDZ_Current.Add_Conc_3 || CurrentConcEmis.Add_Emis_3 > PDZ_Current.Add_Emis_3 ||
                    CurrentConcEmis.Add_Conc_4 > PDZ_Current.Add_Conc_4 || CurrentConcEmis.Add_Emis_4 > PDZ_Current.Add_Emis_4 ||
                    CurrentConcEmis.Add_Conc_5 > PDZ_Current.Add_Conc_5 || CurrentConcEmis.Add_Emis_5 > PDZ_Current.Add_Emis_5)
                {

                    if (!globalAlarms.Is_Excess.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Excess);

                        globalAlarms.Is_Excess.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Excess.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Excess);

                        globalAlarms.Is_Excess.Value = false;
                    }
                }



                //Приближение 
                double approximation = 0.8; //Завести отделбную переменную"!
                if (CurrentConcEmis.CO_Conc > PDZ_Current.CO_Conc * approximation       && CurrentConcEmis.CO_Conc < PDZ_Current.CO_Conc        || CurrentConcEmis.CO_Emis > PDZ_Current.CO_Emis * approximation        && CurrentConcEmis.CO_Emis < PDZ_Current.CO_Emis ||
                    CurrentConcEmis.CO2_Conc > PDZ_Current.CO2_Conc * approximation     && CurrentConcEmis.CO2_Conc < PDZ_Current.CO2_Conc      || CurrentConcEmis.CO2_Emis > PDZ_Current.CO2_Emis * approximation      && CurrentConcEmis.CO2_Emis < PDZ_Current.CO2_Emis ||
                    CurrentConcEmis.NO_Conc > PDZ_Current.NO_Conc * approximation       && CurrentConcEmis.NO_Conc < PDZ_Current.NO_Conc        || CurrentConcEmis.NO_Emis > PDZ_Current.NO_Emis * approximation        && CurrentConcEmis.NO_Emis < PDZ_Current.NO_Emis ||
                    CurrentConcEmis.NO2_Conc > PDZ_Current.NO2_Conc * approximation     && CurrentConcEmis.NO2_Conc < PDZ_Current.NO2_Conc      || CurrentConcEmis.NO2_Emis > PDZ_Current.NO2_Emis * approximation      && CurrentConcEmis.NO2_Emis < PDZ_Current.NO2_Emis ||
                    CurrentConcEmis.NOx_Conc > PDZ_Current.NOx_Conc * approximation     && CurrentConcEmis.NOx_Conc < PDZ_Current.NOx_Conc      || CurrentConcEmis.NOx_Emis > PDZ_Current.NOx_Emis * approximation      && CurrentConcEmis.NOx_Emis < PDZ_Current.NOx_Emis ||
                    CurrentConcEmis.Dust_Conc > PDZ_Current.Dust_Conc * approximation   && CurrentConcEmis.Dust_Conc < PDZ_Current.Dust_Conc    || CurrentConcEmis.Dust_Emis > PDZ_Current.Dust_Emis * approximation    && CurrentConcEmis.Dust_Emis < PDZ_Current.Dust_Emis ||
                    CurrentConcEmis.CH4_Conc > PDZ_Current.CH4_Conc * approximation     && CurrentConcEmis.CH4_Conc < PDZ_Current.CH4_Conc      || CurrentConcEmis.CH4_Emis > PDZ_Current.CH4_Emis * approximation      && CurrentConcEmis.CH4_Emis < PDZ_Current.CH4_Emis ||
                    CurrentConcEmis.H2S_Conc > PDZ_Current.H2S_Conc * approximation     && CurrentConcEmis.H2S_Conc < PDZ_Current.H2S_Conc      || CurrentConcEmis.H2S_Emis > PDZ_Current.H2S_Emis * approximation      && CurrentConcEmis.H2S_Emis < PDZ_Current.H2S_Emis ||
                    CurrentConcEmis.Add_Conc_1 > PDZ_Current.Add_Conc_1 * approximation && CurrentConcEmis.Add_Conc_1 < PDZ_Current.Add_Conc_1  || CurrentConcEmis.Add_Emis_1 > PDZ_Current.Add_Emis_1 * approximation  && CurrentConcEmis.Add_Emis_1 < PDZ_Current.Add_Emis_1 ||
                    CurrentConcEmis.Add_Conc_2 > PDZ_Current.Add_Conc_2 * approximation && CurrentConcEmis.Add_Conc_2 < PDZ_Current.Add_Conc_2  || CurrentConcEmis.Add_Emis_2 > PDZ_Current.Add_Emis_2 * approximation  && CurrentConcEmis.Add_Emis_2 < PDZ_Current.Add_Emis_2 ||
                    CurrentConcEmis.Add_Conc_3 > PDZ_Current.Add_Conc_3 * approximation && CurrentConcEmis.Add_Conc_3 < PDZ_Current.Add_Conc_3  || CurrentConcEmis.Add_Emis_3 > PDZ_Current.Add_Emis_3 * approximation  && CurrentConcEmis.Add_Emis_3 < PDZ_Current.Add_Emis_3 ||
                    CurrentConcEmis.Add_Conc_4 > PDZ_Current.Add_Conc_4 * approximation && CurrentConcEmis.Add_Conc_4 < PDZ_Current.Add_Conc_4  || CurrentConcEmis.Add_Emis_4 > PDZ_Current.Add_Emis_4 * approximation  && CurrentConcEmis.Add_Emis_4 < PDZ_Current.Add_Emis_4 ||
                    CurrentConcEmis.Add_Conc_5 > PDZ_Current.Add_Conc_5 * approximation && CurrentConcEmis.Add_Conc_5 < PDZ_Current.Add_Conc_5  || CurrentConcEmis.Add_Emis_5 > PDZ_Current.Add_Emis_5 * approximation  && CurrentConcEmis.Add_Emis_5 < PDZ_Current.Add_Emis_5)
                {

                    if (!globalAlarms.Is_Approximation.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Approximation);

                        globalAlarms.Is_Approximation.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Approximation.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Approximation);

                        globalAlarms.Is_Approximation.Value = false;
                    }
                }



                //Техническое обслуживание
                if (false)
                {
                    if (!globalAlarms.Is_Maintenance.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Error);

                        globalAlarms.Is_Maintenance.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Maintenance.Value)
                    {
                        //globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Error);

                        globalAlarms.Is_Maintenance.Value = false;
                    }
                }



                //Дверь контейнера
                if (false)
                {
                    if (!globalAlarms.Is_OpenDoor.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_OpenDoor);

                        globalAlarms.Is_OpenDoor.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_OpenDoor.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_OpenDoor);

                        globalAlarms.Is_OpenDoor.Value = false;
                    }
                }




                //Сигнал пожара
                if (false)
                {
                    if (!globalAlarms.Is_Fire.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(true, false, globalAlarms.Is_Fire);

                        globalAlarms.Is_Fire.Value = true;
                    }
                }
                else
                {
                    if (globalAlarms.Is_Fire.Value)
                    {
                        globalAlarm_Services.AlarmLogBuider(false, true, globalAlarms.Is_Fire);

                        globalAlarms.Is_Fire.Value = false;
                    }
                }
            }
        }



        public static double ScaleRange(double value, Range_Model range)
        {
            double min = 4;  //4
            double max = 20;  //20

            if (value <= min)
                return range.Min;

            return Math.Round(range.Min + (value - min) / (max - min) * (range.Max - range.Min), 3);
        }


        //Симуляция 4-20
        public static double Simulation_4_20(double valueNow, bool O2 = false)
        {
            double min = 3.4;
            double max = 20.6;

            int rndMin = -250;
            int rndMax = 250;

            if(O2_Last < 1)
                O2_Last = 1;

            double newValue;

            if (O2)
                newValue = valueNow + rnd.Next(rndMin, rndMax) / 1000.0;
            else
                newValue = valueNow /** (GlobalStaticSettingsASK.SensorNow.O2_Wet_4_20mA / O2_Last)*/ + rnd.Next(rndMin, rndMax) / 10000.0;

            if (newValue < min)
            {
                newValue = newValue + rnd.Next(0, 100) / 10;
                //if (CO < 0.0)
                //    CO = 0.0;
            }
            if (newValue > max)
            {
                newValue = newValue - rnd.Next(0, 70)/10;
            }

            return Math.Round(newValue, 3);
        }

        public static void RunConvertSernsor()
        {
            //SensorScaledNow = new Sensor_4_20();

            SensorScaledNow.Date = DateTime.Now;

            SensorScaledNow.CO_4_20mA = ScaleRange(SensorNow.CO_4_20mA, SensorRange.CO);
            SensorScaledNow.CO2_4_20mA = ScaleRange(SensorNow.CO2_4_20mA, SensorRange.CO2);
            SensorScaledNow.NO_4_20mA = ScaleRange(SensorNow.NO_4_20mA, SensorRange.NO);
            SensorScaledNow.NO2_4_20mA = ScaleRange(SensorNow.NO2_4_20mA, SensorRange.NO2);
            SensorScaledNow.NOx_4_20mA = ScaleRange(SensorNow.NOx_4_20mA, SensorRange.NOx);
            SensorScaledNow.SO2_4_20mA = ScaleRange(SensorNow.SO2_4_20mA, SensorRange.SO2);
            SensorScaledNow.Dust_4_20mA = ScaleRange(SensorNow.Dust_4_20mA, SensorRange.Dust);
            SensorScaledNow.CH4_4_20mA = ScaleRange(SensorNow.CH4_4_20mA, SensorRange.CH4);
            SensorScaledNow.H2S_4_20mA = ScaleRange(SensorNow.H2S_4_20mA, SensorRange.H2S);
            SensorScaledNow.Rezerv_1_4_20mA = ScaleRange(SensorNow.Rezerv_1_4_20mA, SensorRange.Rezerv_1);
            SensorScaledNow.Rezerv_2_4_20mA = ScaleRange(SensorNow.Rezerv_2_4_20mA, SensorRange.Rezerv_2);
            SensorScaledNow.Rezerv_3_4_20mA = ScaleRange(SensorNow.Rezerv_3_4_20mA, SensorRange.Rezerv_3);
            SensorScaledNow.Rezerv_4_4_20mA = ScaleRange(SensorNow.Rezerv_4_4_20mA, SensorRange.Rezerv_4);
            SensorScaledNow.Rezerv_5_4_20mA = ScaleRange(SensorNow.Rezerv_5_4_20mA, SensorRange.Rezerv_5);

            SensorScaledNow.O2_Wet_4_20mA = ScaleRange(SensorNow.O2_Wet_4_20mA, SensorRange.O2Wet);
            SensorScaledNow.O2_Dry_4_20mA = ScaleRange(SensorNow.O2_Dry_4_20mA, SensorRange.O2Dry);
            SensorScaledNow.H2O_4_20mA = ScaleRange(SensorNow.H2O_4_20mA, SensorRange.H2O);

            SensorScaledNow.Pressure_4_20mA = ScaleRange(SensorNow.Pressure_4_20mA, SensorRange.Pressure);
            SensorScaledNow.Temperature_4_20mA = ScaleRange(SensorNow.Temperature_4_20mA, SensorRange.Temperature);
            SensorScaledNow.Speed_4_20mA = ScaleRange(SensorNow.Speed_4_20mA, SensorRange.Speed);

            SensorScaledNow.Temperature_KIP_4_20mA = ScaleRange(SensorNow.Temperature_KIP_4_20mA, SensorRange.Temperature_KIP);
            SensorScaledNow.Temperature_NOx_4_20mA = ScaleRange(SensorNow.Temperature_NOx_4_20mA, SensorRange.Temperature_NOx);

        }



        public static void Normalization_ConcEmis()
        {
            Calculation_Services calculat = new Calculation_Services();

            //CurrentConcEmis = new Array20M();

            CurrentConcEmis.Date = DateTime.Now;


            CurrentConcEmis.O2_Wet = SensorScaledNow.O2_Wet_4_20mA;
            CurrentConcEmis.O2_Dry = SensorScaledNow.O2_Dry_4_20mA;
            CurrentConcEmis.H2O = SensorScaledNow.H2O_4_20mA;

            CurrentConcEmis.Pressure = SensorScaledNow.Pressure_4_20mA;
            CurrentConcEmis.Temperature = SensorScaledNow.Temperature_4_20mA;
            CurrentConcEmis.Speed = SensorScaledNow.Speed_4_20mA;
            CurrentConcEmis.Flow = 0.0;

            CurrentConcEmis.Temperature_KIP = SensorScaledNow.Temperature_KIP_4_20mA;
            CurrentConcEmis.Temperature_NOx = SensorScaledNow.Temperature_NOx_4_20mA;


            if (SensorRange.CO.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CO_4_20mA, SensorRange.CO.Is_ppm, 1.14);
                CurrentConcEmis.CO_Conc = cal.C;
                CurrentConcEmis.CO_Emis = cal.M;
            }

            if (SensorRange.CO2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CO2_4_20mA, SensorRange.CO2.Is_ppm, 1.98);
                CurrentConcEmis.CO2_Conc = cal.C;
                CurrentConcEmis.CO2_Emis = cal.M;
            }

            //Эксклюзивный расчёт для NO, NO2, NOx
            if (SensorRange.NO.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.NO_4_20mA, SensorRange.NO.Is_ppm, 1.34);
                CurrentConcEmis.NOx_Conc = cal.C_NOx;
                CurrentConcEmis.NOx_Emis = cal.M_NOx;
                CurrentConcEmis.NO_Conc = cal.C_NO;
                CurrentConcEmis.NO_Emis = cal.M_NO;
                CurrentConcEmis.NO2_Conc = cal.C_NO2;
                CurrentConcEmis.NO2_Emis = cal.M_NO2;
            }

            if (SensorRange.SO2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.SO2_4_20mA, SensorRange.SO2.Is_ppm, 2.92);
                CurrentConcEmis.SO2_Conc = cal.C;
                CurrentConcEmis.SO2_Emis = cal.M;
            }

            if (SensorRange.Dust.Is_Used || !(CalculationSetting.TypeDust == TypeDustConc.None)) //!!!Проверить!!!
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Dust_4_20mA, SensorRange.Dust.Is_ppm, 1);
                CurrentConcEmis.Dust_Conc = cal.C_Dust;
                CurrentConcEmis.Dust_Emis = cal.M_Dust;
            }

            if (SensorRange.CH4.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.CH4_4_20mA, SensorRange.CH4.Is_ppm, 0.65);
                CurrentConcEmis.CH4_Conc = cal.C;
                CurrentConcEmis.CH4_Emis = cal.M;
            }

            if (SensorRange.H2S.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.H2S_4_20mA, SensorRange.H2S.Is_ppm, 1.36);
                CurrentConcEmis.H2S_Conc = cal.C;
                CurrentConcEmis.H2S_Emis = cal.M;
            }

            if (SensorRange.Rezerv_1.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_1_4_20mA, SensorRange.Rezerv_1.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_1 = cal.C;
                CurrentConcEmis.Add_Emis_1 = cal.M;
            }

            if (SensorRange.Rezerv_2.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_2_4_20mA, SensorRange.Rezerv_2.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_2 = cal.C;
                CurrentConcEmis.Add_Emis_2 = cal.M;
            }

            if (SensorRange.Rezerv_3.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_3_4_20mA, SensorRange.Rezerv_3.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_3 = cal.C;
                CurrentConcEmis.Add_Emis_3 = cal.M;
            }

            if (SensorRange.Rezerv_4.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_4_4_20mA, SensorRange.Rezerv_4.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_4 = cal.C;
                CurrentConcEmis.Add_Emis_4 = cal.M;
            }

            if (SensorRange.Rezerv_5.Is_Used)
            {
                var cal = calculat.Count(CalculationSetting, SensorScaledNow.Rezerv_5_4_20mA, SensorRange.Rezerv_5.Is_ppm, 1.0);
                CurrentConcEmis.Add_Conc_5 = cal.C;
                CurrentConcEmis.Add_Emis_5 = cal.M;
            }
        }



        public static void GetCurrentPDZ()
        {
            if (PDZ.Is_Active == 1)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_1_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_1_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_1_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_1_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_1_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_1_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_1_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_1_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_1_H2S_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_1_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_1_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_1_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_1_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_1_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_1_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_1_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_1_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_1_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_1_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_1_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_1_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_1_H2S_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_1_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_1_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_1_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_1_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_1_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_1_Add_Emis_5;
            }
            if (PDZ.Is_Active == 2)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_2_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_2_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_2_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_2_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_2_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_2_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_2_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_2_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_2_H2S_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_2_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_2_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_2_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_2_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_2_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_2_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_2_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_2_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_2_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_2_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_2_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_2_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_2_H2S_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_2_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_2_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_2_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_2_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_2_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_2_Add_Emis_5;
            }
            if (PDZ.Is_Active == 3)
            {
                PDZ_Current.CO_Conc = PDZ.PDZ_3_CO_Conc;
                PDZ_Current.CO2_Conc = PDZ.PDZ_3_CO2_Conc;
                PDZ_Current.NO_Conc = PDZ.PDZ_3_NO_Conc;
                PDZ_Current.NO2_Conc = PDZ.PDZ_3_NO2_Conc;
                PDZ_Current.NOx_Conc = PDZ.PDZ_3_NOx_Conc;
                PDZ_Current.SO2_Conc = PDZ.PDZ_3_SO2_Conc;
                PDZ_Current.Dust_Conc = PDZ.PDZ_3_Dust_Conc;
                PDZ_Current.CH4_Conc = PDZ.PDZ_3_CH4_Conc;
                PDZ_Current.H2S_Conc = PDZ.PDZ_3_H2S_Conc;
                PDZ_Current.Add_Conc_1 = PDZ.PDZ_3_Add_Conc_1;
                PDZ_Current.Add_Conc_2 = PDZ.PDZ_3_Add_Conc_2;
                PDZ_Current.Add_Conc_3 = PDZ.PDZ_3_Add_Conc_3;
                PDZ_Current.Add_Conc_4 = PDZ.PDZ_3_Add_Conc_4;
                PDZ_Current.Add_Conc_5 = PDZ.PDZ_3_Add_Conc_5;

                PDZ_Current.CO_Emis = PDZ.PDZ_3_CO_Emis;
                PDZ_Current.CO2_Emis = PDZ.PDZ_3_CO2_Emis;
                PDZ_Current.NO_Emis = PDZ.PDZ_3_NO_Emis;
                PDZ_Current.NO2_Emis = PDZ.PDZ_3_NO2_Emis;
                PDZ_Current.NOx_Emis = PDZ.PDZ_3_NOx_Emis;
                PDZ_Current.SO2_Emis = PDZ.PDZ_3_SO2_Emis;
                PDZ_Current.CH4_Emis = PDZ.PDZ_3_CH4_Emis;
                PDZ_Current.H2S_Emis = PDZ.PDZ_3_H2S_Emis;
                PDZ_Current.Dust_Emis = PDZ.PDZ_3_Dust_Emis;
                PDZ_Current.Add_Emis_1 = PDZ.PDZ_3_Add_Emis_1;
                PDZ_Current.Add_Emis_2 = PDZ.PDZ_3_Add_Emis_2;
                PDZ_Current.Add_Emis_3 = PDZ.PDZ_3_Add_Emis_3;
                PDZ_Current.Add_Emis_4 = PDZ.PDZ_3_Add_Emis_4;
                PDZ_Current.Add_Emis_5 = PDZ.PDZ_3_Add_Emis_5;
            }
            GetCurrentStringPDZ();
        }



        public static void GetCurrentStringPDZ()
        {
            if (PDZ_Current.CO_Conc != 0.0 && PDZ_Current.CO_Conc != 9999999.0)
                PDZ_Current_String.CO_Conc = PDZ_Current.CO_Conc.ToString();
            else
                PDZ_Current_String.CO_Conc = "-/-";

            if (PDZ_Current.CO2_Conc != 0.0 && PDZ_Current.CO2_Conc != 9999999.0)
                PDZ_Current_String.CO2_Conc = PDZ_Current.CO2_Conc.ToString();
            else
                PDZ_Current_String.CO2_Conc = "-/-";

            if (PDZ_Current.NO_Conc != 0.0 && PDZ_Current.NO_Conc != 9999999.0)
                PDZ_Current_String.NO_Conc = PDZ_Current.NO_Conc.ToString();
            else
                PDZ_Current_String.NO_Conc = "-/-";

            if (PDZ_Current.NO2_Conc != 0.0 && PDZ_Current.NO2_Conc != 9999999.0)
                PDZ_Current_String.NO2_Conc = PDZ_Current.NO2_Conc.ToString();
            else
                PDZ_Current_String.NO2_Conc = "-/-";

            if (PDZ_Current.NOx_Conc != 0.0 && PDZ_Current.NOx_Conc != 9999999.0)
                PDZ_Current_String.NOx_Conc = PDZ_Current.NOx_Conc.ToString();
            else
                PDZ_Current_String.NOx_Conc = "-/-";

            if (PDZ_Current.SO2_Conc != 0.0 && PDZ_Current.SO2_Conc != 9999999.0)
                PDZ_Current_String.SO2_Conc = PDZ_Current.SO2_Conc.ToString();
            else
                PDZ_Current_String.SO2_Conc = "-/-";

            if (PDZ_Current.Dust_Conc != 0.0 && PDZ_Current.Dust_Conc != 9999999.0)
                PDZ_Current_String.Dust_Conc = PDZ_Current.Dust_Conc.ToString();
            else
                PDZ_Current_String.Dust_Conc = "-/-";

            if (PDZ_Current.CH4_Conc != 0.0 && PDZ_Current.CH4_Conc != 9999999.0)
                PDZ_Current_String.CH4_Conc = PDZ_Current.CH4_Conc.ToString();
            else
                PDZ_Current_String.CH4_Conc = "-/-";

            if (PDZ_Current.H2S_Conc != 0.0 && PDZ_Current.H2S_Conc != 9999999.0)
                PDZ_Current_String.H2S_Conc = PDZ_Current.H2S_Conc.ToString();
            else
                PDZ_Current_String.H2S_Conc = "-/-";

            if (PDZ_Current.Add_Conc_1 != 0.0 && PDZ_Current.Add_Conc_1 != 9999999.0)
                PDZ_Current_String.Add_Conc_1 = PDZ_Current.Add_Conc_1.ToString();
            else
                PDZ_Current_String.Add_Conc_1 = "-/-";

            if (PDZ_Current.Add_Conc_2 != 0.0 && PDZ_Current.Add_Conc_2 != 9999999.0)
                PDZ_Current_String.Add_Conc_2 = PDZ_Current.Add_Conc_2.ToString();
            else
                PDZ_Current_String.Add_Conc_2 = "-/-";

            if (PDZ_Current.Add_Conc_3 != 0.0 && PDZ_Current.Add_Conc_3 != 9999999.0)
                PDZ_Current_String.Add_Conc_3 = PDZ_Current.Add_Conc_3.ToString();
            else
                PDZ_Current_String.Add_Conc_3 = "-/-";

            if (PDZ_Current.Add_Conc_4 != 0.0 && PDZ_Current.Add_Conc_4 != 9999999.0)
                PDZ_Current_String.Add_Conc_4 = PDZ_Current.Add_Conc_4.ToString();
            else
                PDZ_Current_String.Add_Conc_4 = "-/-";

            if (PDZ_Current.Add_Conc_5 != 0.0 && PDZ_Current.Add_Conc_5 != 9999999.0)
                PDZ_Current_String.Add_Conc_5 = PDZ_Current.Add_Conc_5.ToString();
            else
                PDZ_Current_String.Add_Conc_5 = "-/-";


            if (PDZ_Current.CO_Emis != 0.0 && PDZ_Current.CO_Emis != 9999999.0)
                PDZ_Current_String.CO_Emis = PDZ_Current.CO_Emis.ToString();
            else
                PDZ_Current_String.CO_Emis = "-/-";

            if (PDZ_Current.CO2_Emis != 0.0 && PDZ_Current.CO2_Emis != 9999999.0)
                PDZ_Current_String.CO2_Emis = PDZ_Current.CO2_Emis.ToString();
            else
                PDZ_Current_String.CO2_Emis = "-/-";

            if (PDZ_Current.NO_Emis != 0.0 && PDZ_Current.NO_Emis != 9999999.0)
                PDZ_Current_String.NO_Emis = PDZ_Current.NO_Emis.ToString();
            else
                PDZ_Current_String.NO_Emis = "-/-";

            if (PDZ_Current.NO2_Emis != 0.0 && PDZ_Current.NO2_Emis != 9999999.0)
                PDZ_Current_String.NO2_Emis = PDZ_Current.NO2_Emis.ToString();
            else
                PDZ_Current_String.NO2_Emis = "-/-";

            if (PDZ_Current.NOx_Emis != 0.0 && PDZ_Current.NOx_Emis != 9999999.0)
                PDZ_Current_String.NOx_Emis = PDZ_Current.NOx_Emis.ToString();
            else
                PDZ_Current_String.NOx_Emis = "-/-";

            if (PDZ_Current.SO2_Emis != 0.0 && PDZ_Current.SO2_Emis != 9999999.0)
                PDZ_Current_String.SO2_Emis = PDZ_Current.SO2_Emis.ToString();
            else
                PDZ_Current_String.SO2_Emis = "-/-";

            if (PDZ_Current.CH4_Emis != 0.0 && PDZ_Current.CH4_Emis != 9999999.0)
                PDZ_Current_String.CH4_Emis = PDZ_Current.CH4_Emis.ToString();
            else
                PDZ_Current_String.CH4_Emis = "-/-";

            if (PDZ_Current.H2S_Emis != 0.0 && PDZ_Current.H2S_Emis != 9999999.0)
                PDZ_Current_String.H2S_Emis = PDZ_Current.H2S_Emis.ToString();
            else
                PDZ_Current_String.H2S_Emis = "-/-";

            if (PDZ_Current.Dust_Emis != 0.0 && PDZ_Current.Dust_Emis != 9999999.0)
                PDZ_Current_String.Dust_Emis = PDZ_Current.Dust_Emis.ToString();
            else
                PDZ_Current_String.Dust_Emis = "-/-";

            if (PDZ_Current.Add_Emis_1 != 0.0 && PDZ_Current.Add_Emis_1 != 9999999.0)
                PDZ_Current_String.Add_Emis_1 = PDZ_Current.Add_Emis_1.ToString();
            else
                PDZ_Current_String.Add_Emis_1 = "-/-";

            if (PDZ_Current.Add_Emis_2 != 0.0 && PDZ_Current.Add_Emis_2 != 9999999.0)
                PDZ_Current_String.Add_Emis_2 = PDZ_Current.Add_Emis_2.ToString();
            else
                PDZ_Current_String.Add_Emis_2 = "-/-";

            if (PDZ_Current.Add_Emis_3 != 0.0 && PDZ_Current.Add_Emis_3 != 9999999.0)
                PDZ_Current_String.Add_Emis_3 = PDZ_Current.Add_Emis_3.ToString();
            else
                PDZ_Current_String.Add_Emis_3 = "-/-";

            if (PDZ_Current.Add_Emis_4 != 0.0 && PDZ_Current.Add_Emis_4 != 9999999.0)
                PDZ_Current_String.Add_Emis_4 = PDZ_Current.Add_Emis_4.ToString();
            else
                PDZ_Current_String.Add_Emis_4 = "-/-";

            if (PDZ_Current.Add_Emis_5 != 0.0 && PDZ_Current.Add_Emis_5 != 9999999.0)
                PDZ_Current_String.Add_Emis_5 = PDZ_Current.Add_Emis_5.ToString();
            else
                PDZ_Current_String.Add_Emis_5 = "-/-";
        }
    }
}
