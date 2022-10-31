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
            services.AddScoped<ICalculation, Calculation_Services>();

            //DI для работы в Quartz.Net
            services.AddTransient<JobFactory>();
            services.AddScoped<ReaderConcEmisParam>();
            services.AddScoped<Writer20M>();
            services.AddScoped<DeleteOld_4_20m>();
            services.AddScoped<WriterPDZ>();

            services.AddRazorPages();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                //options.AccessDeniedPath = new PathString("/Authentication");
                options.LoginPath = new PathString("/Users");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = false;
                //options.AccessDeniedPath = "/Authentication/Forbidden/";
            });

            //services.AddSingleton<MyMiddleware, MyMiddleware>(); ////middleware
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
    }
}
