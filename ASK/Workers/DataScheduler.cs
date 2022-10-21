using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;

namespace ASK.Workers
{
    public class DataScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            //// Создаём экземпляр планировщика с фабрики
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            await scheduler.Start();

            //IJobDetail ReaderConcEmisParam = JobBuilder.Create<ReaderConcEmisParam>().Build();
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("MailingTrigger", "default")
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //    .WithIntervalInMinutes(1)
            //    .RepeatForever())
            //    .Build();

            //await scheduler.ScheduleJob(ReaderConcEmisParam, trigger);



            //// Создаём экземпляр планировщика с фабрики
            //StdSchedulerFactory factory = new StdSchedulerFactory();
            //IScheduler scheduler = await factory.GetScheduler();

            //// Запускаем его
            //await scheduler.Start();

            // определить задание и привязать его к нашему классу (созданному)
            IJobDetail job_Writer20M = JobBuilder.Create<Writer20M>()
                .WithIdentity("Job_Writer20M", "Group_1")
                .Build();

            IJobDetail job_ReaderConcEmisParam = JobBuilder.Create<ReaderConcEmisParam>()
                .WithIdentity("Job_ReaderConcEmisParam", "Group_2")
                .Build();

            IJobDetail job_WriterPDZ = JobBuilder.Create<WriterPDZ>()
                .WithIdentity("Job_WriterPDZ", "Group_3")
                .Build();

            IJobDetail job_DeleteOld_4_20m = JobBuilder.Create<DeleteOld_4_20m>()
                .WithIdentity("Job_DeleteOld_4_20m", "Group_4")
                .Build();


            //www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html#example-cron-expressions
            ITrigger trigger_Writer20M = TriggerBuilder.Create()
               .WithIdentity("Trigger_Writer20M", "Group_1")
               .WithCronSchedule("0 19,39,59 * * * ?")  //Cron quartz   
               .ForJob("Job_Writer20M", "Group_1")
               .Build();

            // Запустите задание для запуска сейчас, а затем повторяйте каждые 10 секунд.
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

            ITrigger trigger_DeleteOld_4_20m = TriggerBuilder.Create()
               .WithIdentity("Trigger_DeleteOld_4_20m", "Group_4")
               .StartNow()
               .WithSimpleSchedule(x => x
                   .WithIntervalInHours(24)
                   .RepeatForever())
               .Build();


            //Говорим кварцу, чтобы запланировал задание, используя наш триггер
            await scheduler.ScheduleJob(job_ReaderConcEmisParam, trigger_ReaderConcEmisParam);
            await scheduler.ScheduleJob(job_Writer20M, trigger_Writer20M);
            await scheduler.ScheduleJob(job_WriterPDZ, trigger_WriterPDZ);
            await scheduler.ScheduleJob(job_DeleteOld_4_20m, trigger_DeleteOld_4_20m);
        }
    }
}
