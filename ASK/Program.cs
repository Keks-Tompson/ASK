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
            



            //// Grab the Scheduler instance from the Factory
            //StdSchedulerFactory factory = new StdSchedulerFactory();
            //IScheduler scheduler = await factory.GetScheduler();

            //// and start it off
            //await scheduler.Start();

            //// define the job and tie it to our HelloJob class
            //IJobDetail job = JobBuilder.Create<Writer20M>()
            //    .WithIdentity("job1", "group1")
            //    .Build();

            //// Trigger the job to run now, and then repeat every 10 seconds
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(30)
            //        .RepeatForever())
            //    .Build();

            //// Tell quartz to schedule the job using our trigger
            //await scheduler.ScheduleJob(job, trigger);

            //// some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(10));

            // and last shut down the scheduler when you are ready to close your program
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
