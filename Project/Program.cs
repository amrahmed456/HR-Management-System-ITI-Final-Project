using FinalProject.Data;
using FinalProject.Repository;
using FinalProject.Repository.interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Hangfire;
using System.Configuration;
using FinalProject.Controllers.Attendance;
using Microsoft.AspNetCore.Identity;
using FinalProject.Models;
using FinalProject.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //Add Connection String
            builder.Services.AddDbContext<HRContext>(options=>
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("CS"))
            );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<HRContext>()
                            .AddDefaultUI()
                            .AddDefaultTokenProviders();

            //        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //	options.LoginPath = "/Identity/Account/Login";
            //                options.LogoutPath = "/Identity/Account/Login"; // set the logout path to the desired URL
            //            });

            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
           
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            builder.Services.AddHangfire(config =>
				config.UseSqlServerStorage(builder.Configuration.GetConnectionString("CS"))
			);

			//Add Services For DI
			builder.Services.AddScoped<IVacationRepository,VacationService>();
			builder.Services.AddScoped<IGeneralSettingsRepository, GeneralSettingsService>();
            builder.Services.AddScoped<IDepartmentRepository,DepartmentService>();
			builder.Services.AddScoped<IEmployeeAttendanceRepository, EmployeeAttendanceRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeService>();
            builder.Services.AddScoped<IAttendanceReportRepository, AttendanceReportService>();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
			
			app.UseAuthorization();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            var attendanceCronJobs = new AttendanceCronJobs(app.Services);
            attendanceCronJobs.ScheduleJobs();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.MapRazorPages();


            app.Run();
        }
    }
}