using ASK.Workers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            //// ??????? ????????? ???????????? ? ???????
            //StdSchedulerFactory factory = new StdSchedulerFactory();
            //IScheduler scheduler = await factory.GetScheduler();

            //// ????????? ???
            //await scheduler.Start();

            //// ?????????? ??????? ? ????????? ??? ? ?????? ?????? (??????????)
            //IJobDetail job_Writer20M = JobBuilder.Create<Writer20M>()
            //    .WithIdentity("Job_Writer20M", "Group_1")
            //    .Build();

            //IJobDetail job_ReaderConcEmisParam = JobBuilder.Create<ReaderConcEmisParam>()
            //    .WithIdentity("Job_ReaderConcEmisParam", "Group_2")
            //    .Build();

            //IJobDetail job_WriterPDZ = JobBuilder.Create<WriterPDZ>()
            //    .WithIdentity("Job_WriterPDZ", "Group_3")
            //    .Build();

            //IJobDetail job_DeleteOld_4_20m = JobBuilder.Create<DeleteOld_4_20m>()
            //    .WithIdentity("Job_DeleteOld_4_20m", "Group_4")
            //    .Build();


            ////www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html#example-cron-expressions
            //ITrigger trigger_Writer20M = TriggerBuilder.Create()
            //   .WithIdentity("Trigger_Writer20M", "Group_1")
            //   .WithCronSchedule("0 19,39,59 * * * ?")  //Cron quartz   
            //   .ForJob("Job_Writer20M", "Group_1")
            //   .Build();

            //// ????????? ??????? ??? ??????? ??????, ? ????? ?????????? ?????? 10 ??????.
            //ITrigger trigger_ReaderConcEmisParam = TriggerBuilder.Create()
            //    .WithIdentity("Trigger_ReaderConcEmisParam", "Group_2")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(1)
            //        .RepeatForever())
            //    .Build();

            //ITrigger trigger_WriterPDZ = TriggerBuilder.Create()
            //    .WithIdentity("Trigger_WriterPDZ", "Group_3")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInHours(1)
            //        .RepeatForever())
            //    .Build();

            //ITrigger trigger_DeleteOld_4_20m = TriggerBuilder.Create()
            //   .WithIdentity("Trigger_DeleteOld_4_20m", "Group_4")
            //   .StartNow()
            //   .WithSimpleSchedule(x => x
            //       .WithIntervalInHours(24)
            //       .RepeatForever())
            //   .Build();

            //// ??????? ??????, ????? ????????????? ???????, ????????? ??? ???????
            //await scheduler.ScheduleJob(job_ReaderConcEmisParam, trigger_ReaderConcEmisParam);

            //await scheduler.ScheduleJob(job_Writer20M, trigger_Writer20M);

            //await scheduler.ScheduleJob(job_WriterPDZ, trigger_WriterPDZ);

            //await scheduler.ScheduleJob(job_DeleteOld_4_20m, trigger_DeleteOld_4_20m);


            // some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(10));

            //and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();



            //???????? ???? ??????? ? DI ??? ???
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    DataScheduler.Start(serviceProvider);
                }
                catch (Exception)
                {
                    throw;
                }
            }


            CreateHostBuilder(args).Build().Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                  
                });



        //????? ??? ?????? DI Quartc.Net
        public static IWebHost BuildWebHost(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>()
              .Build();
    }
}
