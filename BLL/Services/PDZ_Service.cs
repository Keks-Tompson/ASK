
using ASK.BLL.Interfaces;
using ASK.DAL;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.BLL.Services
{
    public class PDZ_Service : IPDZ_Service
    {
        readonly ApplicationDbContext db;

        public PDZ_Service(ApplicationDbContext _db)
        {
            db = _db;
        }

        public bool Create_PDZ(PDZ new_pdz)
        {
            try
            {
                db.PDZ.Add(new PDZ()
                {
                    Date = new_pdz.Date,

                    CO_Conc = new_pdz.CO_Conc,
                    CO2_Conc = new_pdz.CO2_Conc,
                    NO_Conc = new_pdz.NO_Conc,
                    NO2_Conc = new_pdz.NO2_Conc,
                    NOx_Conc = new_pdz.NOx_Conc,
                    SO2_Conc = new_pdz.SO2_Conc,
                    Dust_Conc = new_pdz.Dust_Conc,
                    CH4_Conc = new_pdz.CH4_Conc,
                    H2S_Conc = new_pdz.H2S_Conc,
                    Add_Conc_1 = new_pdz.Add_Conc_1,
                    Add_Conc_2 = new_pdz.Add_Conc_2,
                    Add_Conc_3 = new_pdz.Add_Conc_3,
                    Add_Conc_4 = new_pdz.Add_Conc_4,
                    Add_Conc_5 = new_pdz.Add_Conc_5,

                    CO_Emis = new_pdz.CO_Emis,
                    CO2_Emis = new_pdz.CO2_Emis,
                    NO_Emis = new_pdz.NO_Emis,
                    NO2_Emis = new_pdz.NO2_Emis,
                    NOx_Emis = new_pdz.NOx_Emis,
                    SO2_Emis = new_pdz.SO2_Emis,
                    CH4_Emis = new_pdz.CH4_Emis,
                    H2S_Emis = new_pdz.H2S_Emis,
                    Dust_Emis = new_pdz.Dust_Emis,
                    Add_Emis_1 = new_pdz.Add_Emis_1,
                    Add_Emis_2 = new_pdz.Add_Emis_2,
                    Add_Emis_3 = new_pdz.Add_Emis_3,
                    Add_Emis_4 = new_pdz.Add_Emis_4,
                    Add_Emis_5 = new_pdz.Add_Emis_5,

                    NumberPDZ = new_pdz.NumberPDZ,
                    Current = new_pdz.Current
                });

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<PDZ> Get_All_PDZ()
        {
            var dbPDZ = db.PDZ.ToList();
            return dbPDZ.Select(s => s).ToList();
        }

        public List<PDZ> Get_DayAll_PDZ(DateTime date)
        {
            var dbPDZ = db.PDZ.ToList();
            return dbPDZ.Where(w => w.Date.Year == date.Year && w.Date.Month == date.Date.Month && w.Date.Day == date.Day).Select(s => s).ToList();
        }

        public PDZ FisrsPDZMonth(DateTime date)
        {
            var dbPDZ = db.PDZ.ToList();
            return dbPDZ.First(a => a.Date.Year == date.Year && a.Date.Month == date.Month && a.Current == true);
        }

        public bool FindPDZDay()
        {
            var dbPDZ = db.PDZ.ToList();
            return dbPDZ.Any(a => a.Date.Year == DateTime.Now.Year && a.Date.Month == DateTime.Now.Month && a.Date.Day == DateTime.Now.Day);
        }
    }
}
