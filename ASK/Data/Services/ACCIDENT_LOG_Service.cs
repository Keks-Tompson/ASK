using ASK.Data.Interfaces;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.Data.Services
{
    public class ACCIDENT_LOG_Service : IACCIDENT_LOG
    {
        readonly ApplicationDbContext db;

        public ACCIDENT_LOG_Service(ApplicationDbContext _db)
        {
            db = _db;
        }


        public bool Create_ACCIDENT_LOG(ACCIDENT_LOG accident_log)
        {
            try 
            {
                db.ACCIDENT_LOG.Add(new ACCIDENT_LOG()
                {
                     id_accident = accident_log.id_accident,
                     Date_Begin = accident_log.Date_Begin,
                     Time_End = accident_log.Time_End,
                     Is_Active = accident_log.Is_Active
                });
                db.SaveChanges();
                return true;
            } 
            catch
            {
                return false;
            }
        }

        public List<ACCIDENT_LOG> Get_All_ACCIDENT_LOG()
        {
            return db.ACCIDENT_LOG.Select(s => s).ToList();
        }

        public List<ACCIDENT_LOG> Get_All_ACCIDENT_LOG_Active()
        {
            return db.ACCIDENT_LOG.Where(w=> w.Is_Active == true).ToList();
        }

        public List<ACCIDENT_LOG> Get_DayAll_ACCIDENT_LOG(DateTime date)
        {
            return db.ACCIDENT_LOG.Where(w => w.Date_Begin.Year == date.Year && w.Date_Begin.Month == date.Month && w.Date_Begin.Day == date.Day).ToList();
        }
    }
}
