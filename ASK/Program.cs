using ASK.Workers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASK
{
    public class Program
    {
        public static async Task Main(string[] args)
        {




            // —оздаЄм экземпл€р планировщика с фабрики
            StdSchedulerFactory factory = new StdSchedulerFactory();

            IScheduler scheduler = await factory.GetScheduler();


            // «апускаем его
            await scheduler.Start();


            // определить задание и прив€зать его к нашему классу (созданному)
            IJobDetail job_Writer20M = JobBuilder.Create<Writer20M>()
                .WithIdentity("Job_Writer20M", "Group_1")
                .Build();

            IJobDetail job_ReaderConcEmisParam = JobBuilder.Create<ReaderConcEmisParam>()
                .WithIdentity("Job_ReaderConcEmisParam", "Group_2")
                .Build();

            IJobDetail job_WriterPDZ = JobBuilder.Create<WriterPDZ>()
                .WithIdentity("Job_WriterPDZ", "Group_3")
                .Build();

            // «апустите задание дл€ запуска сейчас, а затем повтор€йте каждые 10 секунд.
            ITrigger trigger_Writer20M = TriggerBuilder.Create()
                .WithIdentity("Trigger_Writer20M", "Group_1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            ITrigger trigger_ReaderConcEmisParam = TriggerBuilder.Create()
                .WithIdentity("Trigger_ReaderConcEmisParam", "Group_2")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(1)
                    .RepeatForever())
                .Build();



            ITrigger trigger_WriterPDZ = TriggerBuilder.Create()
                .WithIdentity("Trigger_WriterPDZ", "Group_3")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();

            // —кажите кварцу, чтобы запланировать задание, использу€ наш триггер


            await scheduler.ScheduleJob(job_ReaderConcEmisParam, trigger_ReaderConcEmisParam);

            await scheduler.ScheduleJob(job_Writer20M, trigger_Writer20M);

            await scheduler.ScheduleJob(job_WriterPDZ, trigger_WriterPDZ);

            // some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(10));

            //and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();



            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                  
                });


        


        
    }
}
