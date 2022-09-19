using ASK.DAL.Interfaces;
using ASK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASK.DAL.Repository
{
    public class SENSOR_4_20_10sec_Repository : ISENSOR_4_20_10sec
    {
        readonly ApplicationDbContext db;

        public SENSOR_4_20_10sec_Repository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public bool Create_SENSOR_4_20_10sec(SENSOR_4_20_10sec newSernsors)
        {
            try
            {
                db.SENSOR_4_20_10sec.Add(newSernsors);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteOld(DateTime date)
        {
            var deleteLists = db.SENSOR_4_20_10sec.Where(w=> w.Date <= date).ToList();
            if (deleteLists != null)
                foreach (var deleteList in deleteLists)
                {
                    db.SENSOR_4_20_10sec.Remove(deleteList);
                }
            db.SaveChanges();
        }

        public List<SENSOR_4_20_10sec> Get_All_SENSOR_4_20_10sec()
        {
            return db.SENSOR_4_20_10sec.ToList();
        }

        public List<SENSOR_4_20_10sec> Get_Range_SENSOR_4_20_10sec(DateTime date1, DateTime date2) //Чуть позже
        {
            throw new NotImplementedException();
        }
    }
}
