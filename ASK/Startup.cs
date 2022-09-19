using ASK.BLL.Interfaces;
using ASK.BLL.Services;
using ASK.DAL;
using ASK.DAL.Interfaces;
using ASK.DAL.Repository;
using ASK.Workers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASK
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            //DI
            services.AddScoped<IAVG_20_MINUTES, AVG_20_MINUTES_Repository>(); //!
            services.AddScoped<IPDZ, PDZ_Repository>(); //!
            services.AddScoped<IReportDay, ReportDay_Services>();
            services.AddScoped<IReportMonth, ReportMonth_Services>();
            services.AddScoped<IACCIDENT_LOG, ACCIDENT_LOG_Repository>();
            services.AddScoped<IACCIDENT_LIST, ACCIDENT_LIST_Repository>();
            services.AddScoped<IAlarmLog, AlarmLog_Services>();
            services.AddScoped<IColorSensorParametrError, ColorSensorParametrError_Services>();
            services.AddScoped<ICurrentPage, CurrentPage_Services>();
            services.AddScoped<IExcelReport, ExcelReport_Services>();

            services.AddRazorPages();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                //options.AccessDeniedPath = new PathString("/Authentication");
                options.LoginPath = new PathString("/Users");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = false;
                //options.AccessDeniedPath = "/Authentication/Forbidden/";
            });

            //services.AddSingleton<MyMiddleware, MyMiddleware>(); ////middleware

            


            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //services.AddTransient<JobFactory>();
            //services.AddScoped<DataJob>();
            //services.AddScoped<IWriter20M, Writer20M>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Users");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseMiddleware<MyMiddleware>();

            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    await context.Response.WriteAsync("Hello World");
            //    //await midleware.WriteAsync(context);
            //});


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            



        }

        public static async Task Method()
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

            IJobDetail job_DeleteOld_4_20m = JobBuilder.Create<DeleteOld_4_20m>()
                .WithIdentity("Job_DeleteOld_4_20m", "Group_4")
                .Build();





            //www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html#example-cron-expressions
            ITrigger trigger_Writer20M = TriggerBuilder.Create()
               .WithIdentity("Trigger_Writer20M", "Group_1")
               .WithCronSchedule("0 19,39,59 * * * ?")  //Cron quartz   
               .ForJob("Job_Writer20M", "Group_1")
               .Build();


            // «апустите задание дл€ запуска сейчас, а затем повтор€йте каждые 10 секунд.
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

            // —кажите кварцу, чтобы запланировать задание, использу€ наш триггер


            await scheduler.ScheduleJob(job_ReaderConcEmisParam, trigger_ReaderConcEmisParam);

            await scheduler.ScheduleJob(job_Writer20M, trigger_Writer20M);

            await scheduler.ScheduleJob(job_WriterPDZ, trigger_WriterPDZ);

            await scheduler.ScheduleJob(job_DeleteOld_4_20m, trigger_DeleteOld_4_20m);


            // some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(10));

            //and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();
        }

    }
}
