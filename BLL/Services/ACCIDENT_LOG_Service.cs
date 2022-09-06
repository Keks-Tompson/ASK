
using ASK.BLL.Interfaces;
using ASK.DAL;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.BLL.Services
{
    public class ACCIDENT_LOG_Service : IACCIDENT_LOG
    {
        readonly ApplicationDbContext db;

        public ACCIDENT_LOG_Service(ApplicationDbContext _db)
        {
            db = _db;
        }
    


        public void StratSystem_ACCIDENT_LOG() //Уведомление чо система запущена + очистка старых уведомлений
        {
            //var accident = db.ACCIDENT_LOG.Where(f => f.Is_Active == true).ToList();
            foreach (var mas in db.ACCIDENT_LOG)
            {
                if (mas.Is_Active)
                {
                    mas.Is_Active = false;
                    mas.Time_End = DateTime.Now;
                }
            }

            db.ACCIDENT_LOG.Add(new ACCIDENT_LOG()
            {
                id_accident = 11, //Система запущена/перезапущена
                Date_Begin = DateTime.Now,
                Time_End = DateTime.Now,
                Is_Active = false
            });

            db.SaveChanges();
        }


        public bool Begin_ACCIDENT_LOG(int id_Accident) //Записываем событие и дату начала события
        {
            var accident = db.ACCIDENT_LOG.FirstOrDefault(f => f.Is_Active == true && f.id_accident == id_Accident);

            if (accident == null)
            {

                var dateNow = DateTime.Now;
                try
                {
                    db.ACCIDENT_LOG.Add(new ACCIDENT_LOG()
                    {
                        id_accident = id_Accident,
                        Date_Begin = dateNow,
                        Time_End = dateNow,
                        Is_Active = true
                    });
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void End_ACCIDENT_LOG(int id_Accident) //Записываем дату окончания события
        {
            var accident = db.ACCIDENT_LOG.FirstOrDefault(f => f.Is_Active == true && f.id_accident == id_Accident);

            if (accident != null)
            {
                accident.Is_Active = false;
                accident.Time_End = DateTime.Now;
                db.ACCIDENT_LOG.Update(accident);
                db.SaveChanges();
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
