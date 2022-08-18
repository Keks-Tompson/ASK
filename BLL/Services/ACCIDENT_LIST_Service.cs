using ASK.BLL.Interfaces;
using ASK.DAL;
using ASK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASK.BLL.Services
{
    public class ACCIDENT_LIST_Service : IACCIDENT_LIST
    {
        readonly ApplicationDbContext db;

        public ACCIDENT_LIST_Service(ApplicationDbContext _db)
        {
            db = _db;
        }

        public bool Create_ACCIDENT_LIST(ACCIDENT_LIST accident_list)
        {
            try
            {
                db.ACCIDENT_LIST.Add(new ACCIDENT_LIST()
                {
                    Accident = accident_list.Accident,
                    is_Error = accident_list.is_Error
                });
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ACCIDENT_LIST> Get_All_ACCIDENT_LIST()
        {
            return db.ACCIDENT_LIST.Select(s => s).ToList();
        }

    
    }
}
