using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.IO;
using ASK.Models;

namespace ASK.DAL
{
    public class ApplicationDbContext : DbContext //IdentityDbContext
    {
        //Изменить БД в ручную!!! тут и в appsettings.json
        #region Указание БД в ручную - НУЖНО ИСПРАВИТЬ НА АВТОМАТИЧЕСКОЕ ФОРМИРОВАНИЕ!!!!!

        public static DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        public static Microsoft.EntityFrameworkCore.DbContextOptions<ASK.DAL.ApplicationDbContext> options = optionsBuilder
                .UseSqlServer(@"Server=MNS1-212N;Database=ASK_SIMATEK;Trusted_Connection=True;MultipleActiveResultSets=true")
                //.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ASK_SIMATEK;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

        #endregion


        public ApplicationDbContext() ///Для создания экземпляра БД без указания OPTIONS
             : base(options)
        {
            //Database.EnsureCreated();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  //Для миграций и обновления БД
            : base(options)
        { }

        public DbSet<AVG_20_MINUTES> AVG_20_MINUTE { get; set; }
        public DbSet<PDZ> PDZ { get; set; }
        public DbSet<ACCIDENT_LIST> ACCIDENT_LIST { get; set; }
        public DbSet<ACCIDENT_LOG> ACCIDENT_LOG { get; set; }
        public DbSet<SENSOR_4_20_10sec> SENSOR_4_20_10sec { get; set; }


    }
}




