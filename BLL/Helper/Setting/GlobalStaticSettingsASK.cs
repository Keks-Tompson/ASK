
using ASK.BLL.Helper.Chart;
using ASK.BLL.Services;
using ASK.DAL;
using ASK.Models;
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
        //Chart
        public static Chart_CurrentValue ChartCurrent { get; set; } = new Chart_CurrentValue();
        public static List<Chart_CurrentValue> ChartList { get; set; } = new List<Chart_CurrentValue>();
        public static int CounterChart { get; set; } = 0;



        public static SettingOptionsJSON SettingOptions { get; set; }
        public static VisibilityOptions20MJSON VisibilityOptions20M { get; set; }
        public static PdzJSON PDZ { get; set; }                                     //Все 3 вида топлива ПДЗ 
        public static PDZ_Active PDZ_Current { get; set; } = new PDZ_Active();      //Текущие ПДЗ в double
        public static PDZ_String_Active PDZ_Current_String { get; set; } = new PDZ_String_Active();          //Текущие ПДЗ в string (-/-)


        public static SensorRangeJSON SensorRange { get; set; }


        public static List<Sensor_4_20> Sensor_4_20s = new List<Sensor_4_20>();     //Будем хранить значение 4-20мА 
        public static Sensor_4_20 SensorNow { get; set; } = new Sensor_4_20();                        //Будем хранить текущее значения датчиков (4-20)

        public static Sensor_4_20 SensorScaledNow { get; set; } = new Sensor_4_20();//Будет хранить текущие значения прямых показаний датчиков 

        public static List<Array20M> Array20Ms { get; set; } = new List<Array20M>();//Текущие значения формируемой 20 минутки
        public static Array20M CurrentConcEmis { get; set; } = new Array20M();      //Текущие значение концентрацйи и выбросов после приведения



        //Цвета 
        public static Color ColorExcess { get; set; } = Color.FromArgb(247, 213, 213); //Цвет превышения
        //public static Color ColorExcess { get; set; } = Color.FromArgb(0, 14, 14, 3); //Цвет превышения

        public static Color ColorHeader1 { get; set; } = Color.FromArgb(255, 230, 168); //Цвет заголовка 1
        public static Color ColorHeader2 { get; set; } = Color.FromArgb(182, 242, 250); //Цвет заголовка 2



        public static bool stopGetSernsorNow = false; //Если идёт запись в БД 20М тормозит поток обновления данных, пока запись не запишется; 

        //всё для пинга ПЛК 
        static Ping ping = new Ping();
        static PingReply connected;




        //Нет связи с плк
        public static bool isNotConnection = false;


        public static void SaveSettingOptionsJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SettingOptionsJSON.json");
            file.WriteLine(JsonSerializer.Serialize(SettingOptions, typeof(SettingOptionsJSON)));
            file.Close();
        }

        public static void SaveVisibilityOptions20MJSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\VisibilityOptions20MJSON.json");
            file.WriteLine(JsonSerializer.Serialize(VisibilityOptions20M, typeof(VisibilityOptions20MJSON)));
            file.Close();
        }

        public static void SavePdz_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\PdzJSON.json");
            file.WriteLine(JsonSerializer.Serialize(PDZ, typeof(PdzJSON)));
            file.Close();
            GetCurrentPDZ();
        }


        public static void SaveSensorRange_JSON()
        {
            StreamWriter file = File.CreateText("SaveSetting_JSON\\SensorRangeJSON.JSON");
            file.WriteLine(JsonSerializer.Serialize(SensorRange, typeof(SensorRangeJSON)));
            file.Close();
        }




        static GlobalStaticSettingsASK()
        {
            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SettingOptionsJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SettingOptionsJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SettingOptionsJSON.json");
                SettingOptions = JsonSerializer.Deserialize<SettingOptionsJSON>(data);
            }
            else //не существует
            {
                SettingOptions = new SettingOptionsJSON();

                SaveSettingOptionsJSON();
            }






            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  VisibilityOptions20MJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\VisibilityOptions20MJSON.json")) //Если файл существует
            {
                string data = File.ReadAllText("SaveSetting_JSON\\VisibilityOptions20MJSON.json");
                VisibilityOptions20M = JsonSerializer.Deserialize<VisibilityOptions20MJSON>(data);
            }
            else //не существует
            {
                VisibilityOptions20M = new VisibilityOptions20MJSON();

                SaveVisibilityOptions20MJSON();
            }


            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  PdzJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\PdzJSON.json"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\PdzJSON.json");
                PDZ = JsonSerializer.Deserialize<PdzJSON>(data);
            }
            else
            {
                PDZ = new PdzJSON();
                SavePdz_JSON();

            }


            //-------------------------------------------------------------------------------------------------------------------------------------------
            //                                                  SensorRangeJSON
            //-------------------------------------------------------------------------------------------------------------------------------------------
            if (File.Exists("SaveSetting_JSON\\SensorRangeJSON.JSON"))
            {
                string data = File.ReadAllText("SaveSetting_JSON\\SensorRangeJSON.JSON");
                SensorRange = JsonSerializer.Deserialize<SensorRangeJSON>(data);
            }
            else
            {
                SensorRange = new SensorRangeJSON();
                SaveSensorRange_JSON();
            }

            GetCurrentPDZ();

            ChartList.Add(new Chart_CurrentValue());

        }

        public static async Task Add_20M_Async()
        {


            stopGetSernsorNow = true; //стопим запись данных в Array20Ms

            await Task.Delay(TimeSpan.FromSeconds(1));  //Выжидаем, что бы поток который прошёл успел остановится

            await Task.Run(() => Add_20M());

            stopGetSernsorNow = false; //Рарешаем работу записи
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

                    new20M.Mode_ASK = random.Next(0, 3);
                    new20M.PDZ_Fuel = random.Next(0, 1);

                }
                catch
                {

                }


                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        AVG_20_MINUTES_Service avg_20_M_Service = new AVG_20_MINUTES_Service(db);
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
                    //GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M = GlobalStaticSettingsASK.VisibilityOptions20M.data_add_20M.AddMinutes(20);
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
                    SENSOR_4_20_10sec_Service Sensors_4_20db = new SENSOR_4_20_10sec_Service(db);
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

            Sensor_4_20s.Add((Sensor_4_20)SensorNow.Clone());

            if (Sensor_4_20s.Count > 999)
                Sensor_4_20s.RemoveAt(0);

            if (!stopGetSernsorNow)
                Array20Ms.Add((Array20M)CurrentConcEmis.Clone());

            if (CounterChart > 8)
            {
                CounterChart = 0;

                ChartCurrent.Getsimulation();
                if (ChartList.Count > 699)
                    ChartList.RemoveAt(0);

                ChartList.Add((Chart_CurrentValue)ChartCurrent.Clone());

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        SENSOR_4_20_10sec_Service Sensors_4_20db = new SENSOR_4_20_10sec_Service(db);
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
                PDZ_Service pdz_Service = new PDZ_Service(db);

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
                connected = ping.Send(IpAdres, 900); //Проверяем соедение с таймингом 0.5 сек

                ushort[] registers; //Будующий масиив считываемых WORD из ПЛК

                if (connected.Status == IPStatus.Success)
                {

                    isNotConnection = true;

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
                    isNotConnection = false;

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
            catch
            {
                isNotConnection = false;

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


        public static double ScaleRange(double value, double minScale, double maxScale)
        {
            double min = 4.0;
            double max = 20.0;



            if (value <= min)
                return minScale;

            return Math.Round(minScale + (value - min) / (max - min) * (maxScale - minScale), 3);
        }

        public static void RunConvertSernsor()
        {
            //SensorScaledNow = new Sensor_4_20();

            SensorScaledNow.Date = DateTime.Now;

            SensorScaledNow.CO_4_20mA = ScaleRange(SensorNow.CO_4_20mA, SensorRange.Min_CO, SensorRange.Max_CO);
            SensorScaledNow.CO2_4_20mA = ScaleRange(SensorNow.CO2_4_20mA, SensorRange.Min_CO2, SensorRange.Max_CO2);
            SensorScaledNow.NO_4_20mA = ScaleRange(SensorNow.NO_4_20mA, SensorRange.Min_NO, SensorRange.Max_NO);
            SensorScaledNow.NO2_4_20mA = ScaleRange(SensorNow.NO2_4_20mA, SensorRange.Min_NO2, SensorRange.Max_NO2);
            SensorScaledNow.NOx_4_20mA = ScaleRange(SensorNow.NOx_4_20mA, SensorRange.Min_NOx, SensorRange.Max_NOx);
            SensorScaledNow.SO2_4_20mA = ScaleRange(SensorNow.SO2_4_20mA, SensorRange.Min_SO2, SensorRange.Max_SO2);
            SensorScaledNow.Dust_4_20mA = ScaleRange(SensorNow.Dust_4_20mA, SensorRange.Min_Dust, SensorRange.Max_Dust);
            SensorScaledNow.CH4_4_20mA = ScaleRange(SensorNow.CH4_4_20mA, SensorRange.Min_CH4, SensorRange.Max_CH4);
            SensorScaledNow.H2S_4_20mA = ScaleRange(SensorNow.H2S_4_20mA, SensorRange.Min_H2S, SensorRange.Max_H2S);
            SensorScaledNow.Rezerv_1_4_20mA = ScaleRange(SensorNow.Rezerv_1_4_20mA, SensorRange.Min_Rezerv_1, SensorRange.Max_Rezerv_1);
            SensorScaledNow.Rezerv_2_4_20mA = ScaleRange(SensorNow.Rezerv_2_4_20mA, SensorRange.Min_Rezerv_2, SensorRange.Max_Rezerv_2);
            SensorScaledNow.Rezerv_3_4_20mA = ScaleRange(SensorNow.Rezerv_3_4_20mA, SensorRange.Min_Rezerv_3, SensorRange.Max_Rezerv_3);
            SensorScaledNow.Rezerv_4_4_20mA = ScaleRange(SensorNow.Rezerv_4_4_20mA, SensorRange.Min_Rezerv_4, SensorRange.Max_Rezerv_4);
            SensorScaledNow.Rezerv_5_4_20mA = ScaleRange(SensorNow.Rezerv_5_4_20mA, SensorRange.Min_Rezerv_5, SensorRange.Max_Rezerv_5);

            SensorScaledNow.O2_Wet_4_20mA = ScaleRange(SensorNow.O2_Wet_4_20mA, SensorRange.Min_O2Wet, SensorRange.Max_O2Wet);
            SensorScaledNow.O2_Dry_4_20mA = ScaleRange(SensorNow.O2_Dry_4_20mA, SensorRange.Min_O2Dry, SensorRange.Max_O2Dry);
            SensorScaledNow.H2O_4_20mA = ScaleRange(SensorNow.H2O_4_20mA, SensorRange.Min_H2O, SensorRange.Max_H2O);

            SensorScaledNow.Pressure_4_20mA = ScaleRange(SensorNow.Pressure_4_20mA, SensorRange.Min_Pressure, SensorRange.Max_Pressure);
            SensorScaledNow.Temperature_4_20mA = ScaleRange(SensorNow.Temperature_4_20mA, SensorRange.Min_Temperature, SensorRange.Max_Temperature);
            SensorScaledNow.Speed_4_20mA = ScaleRange(SensorNow.Speed_4_20mA, SensorRange.Min_Speed, SensorRange.Max_Speed);

            SensorScaledNow.Temperature_KIP_4_20mA = ScaleRange(SensorNow.Temperature_KIP_4_20mA, SensorRange.Min_Temperature_KIP, SensorRange.Max_Temperature_KIP);
            SensorScaledNow.Temperature_NOx_4_20mA = ScaleRange(SensorNow.Temperature_NOx_4_20mA, SensorRange.Min_Temperature_NOx, SensorRange.Max_Temperature_NOx);
        }

        public static void Normalization_ConcEmis()
        {
            //CurrentConcEmis = new Array20M();

            CurrentConcEmis.Date = DateTime.Now;

            CurrentConcEmis.CO_Conc = 0.0;
            CurrentConcEmis.CO2_Conc = 0.0;
            CurrentConcEmis.NO_Conc = 0.0;
            CurrentConcEmis.NO2_Conc = 0.0;
            CurrentConcEmis.NOx_Conc = 0.0;
            CurrentConcEmis.SO2_Conc = 0.0;
            CurrentConcEmis.Dust_Conc = 0.0;
            CurrentConcEmis.CH4_Conc = 0.0;
            CurrentConcEmis.H2S_Conc = 0.0;
            CurrentConcEmis.Add_Conc_1 = 0.0;
            CurrentConcEmis.Add_Conc_2 = 0.0;
            CurrentConcEmis.Add_Conc_3 = 0.0;
            CurrentConcEmis.Add_Conc_4 = 0.0;
            CurrentConcEmis.Add_Conc_5 = 0.0;

            CurrentConcEmis.CO_Emis = 0.0;
            CurrentConcEmis.CO2_Emis = 0.0;
            CurrentConcEmis.NO_Emis = 0.0;
            CurrentConcEmis.NO2_Emis = 0.0;
            CurrentConcEmis.NOx_Emis = 0.0;
            CurrentConcEmis.SO2_Emis = 0.0;
            CurrentConcEmis.Dust_Emis = 0.0;
            CurrentConcEmis.CH4_Emis = 0.0;
            CurrentConcEmis.H2S_Emis = 0.0;
            CurrentConcEmis.Add_Emis_1 = 0.0;
            CurrentConcEmis.Add_Emis_2 = 0.0;
            CurrentConcEmis.Add_Emis_3 = 0.0;
            CurrentConcEmis.Add_Emis_4 = 0.0;
            CurrentConcEmis.Add_Emis_5 = 0.0;

            CurrentConcEmis.O2_Wet = SensorScaledNow.O2_Wet_4_20mA;
            CurrentConcEmis.O2_Dry = SensorScaledNow.O2_Dry_4_20mA;
            CurrentConcEmis.H2O = SensorScaledNow.H2O_4_20mA;

            CurrentConcEmis.Pressure = SensorScaledNow.Pressure_4_20mA;
            CurrentConcEmis.Temperature = SensorScaledNow.Temperature_4_20mA;
            CurrentConcEmis.Speed = SensorScaledNow.Speed_4_20mA;
            CurrentConcEmis.Flow = 0.0;

            CurrentConcEmis.Temperature_KIP = SensorScaledNow.Temperature_KIP_4_20mA;
            CurrentConcEmis.Temperature_NOx = SensorScaledNow.Temperature_NOx_4_20mA;
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
