
using ASK.DAL.Interfaces;
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.DAL.Repository
{
    public class AVG_20_MINUTES_Repository : IAVG_20_MINUTES
    {
        readonly ApplicationDbContext db;

        public AVG_20_MINUTES_Repository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public bool Create_AVG_20_MINUTES(AVG_20_MINUTES new_AVG_20_MINUTES)
        {
            try
            {
                db.AVG_20_MINUTE.Add(new AVG_20_MINUTES()
                {
                    Date = new_AVG_20_MINUTES.Date,

                    Conc_CO = new_AVG_20_MINUTES.Conc_CO,
                    Conc_CO2 = new_AVG_20_MINUTES.Conc_CO2,
                    Conc_NO = new_AVG_20_MINUTES.Conc_NO,
                    Conc_NO2 = new_AVG_20_MINUTES.Conc_NO2,
                    Conc_NOx = new_AVG_20_MINUTES.Conc_NOx,
                    Conc_SO2 = new_AVG_20_MINUTES.Conc_SO2,
                    Conc_Dust = new_AVG_20_MINUTES.Conc_Dust,
                    Conc_CH4 = new_AVG_20_MINUTES.Conc_CH4,
                    Conc_H2S = new_AVG_20_MINUTES.Conc_H2S,
                    Conc_D1 = new_AVG_20_MINUTES.Conc_D1,
                    Conc_D2 = new_AVG_20_MINUTES.Conc_D2,
                    Conc_D3 = new_AVG_20_MINUTES.Conc_D3,
                    Conc_D4 = new_AVG_20_MINUTES.Conc_D4,
                    Conc_D5 = new_AVG_20_MINUTES.Conc_D5,

                    Emis_CO = new_AVG_20_MINUTES.Emis_CO,
                    Emis_CO2 = new_AVG_20_MINUTES.Emis_CO2,
                    Emis_NO = new_AVG_20_MINUTES.Emis_NO,
                    Emis_NO2 = new_AVG_20_MINUTES.Emis_NO2,
                    Emis_NOx = new_AVG_20_MINUTES.Emis_NOx,
                    Emis_SO2 = new_AVG_20_MINUTES.Emis_SO2,
                    Emis_CH4 = new_AVG_20_MINUTES.Emis_CH4,
                    Emis_H2S = new_AVG_20_MINUTES.Emis_H2S,
                    Emis_Dust = new_AVG_20_MINUTES.Emis_Dust,
                    Emis_D1 = new_AVG_20_MINUTES.Emis_D1,
                    Emis_D2 = new_AVG_20_MINUTES.Emis_D2,
                    Emis_D3 = new_AVG_20_MINUTES.Emis_D3,
                    Emis_D4 = new_AVG_20_MINUTES.Emis_D4,
                    Emis_D5 = new_AVG_20_MINUTES.Emis_D5,

                    O2_Wet = new_AVG_20_MINUTES.O2_Wet,
                    O2_Dry = new_AVG_20_MINUTES.O2_Dry,
                    H2O = new_AVG_20_MINUTES.H2O,

                    Pressure = new_AVG_20_MINUTES.Pressure,
                    Temperature = new_AVG_20_MINUTES.Temperature,
                    Speed = new_AVG_20_MINUTES.Speed,
                    Flow = new_AVG_20_MINUTES.Flow,
                    Temperature_KIP = new_AVG_20_MINUTES.Temperature_KIP,
                    Temperature_NOx = new_AVG_20_MINUTES.Temperature_NOx,

                    Mode_ASK = new_AVG_20_MINUTES.Mode_ASK,
                    PDZ_Fuel = new_AVG_20_MINUTES.PDZ_Fuel,
                });

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<AVG_20_MINUTES> Get_All_AVG_20_MINMUTES()
        {
            var dbAVG_20_MINUTES = db.AVG_20_MINUTE.ToList();
            return dbAVG_20_MINUTES.Select(s => s).ToList();
        }

        public List<AVG_20_MINUTES> Get_DayAll_AVG_20_MINUTES(DateTime date)
        {
            var dbAVG_20_MINUTES = db.AVG_20_MINUTE.ToList();
            return dbAVG_20_MINUTES.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Date.Month && w.Date.Day == date.Day).Select(s => s).ToList();
        }
    }
}
