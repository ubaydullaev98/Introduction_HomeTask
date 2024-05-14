using Introduction_HomeTask.Configurations;
using Introduction_HomeTask.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace Introduction_HomeTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ProductOptions>(builder.Configuration.GetSection(ProductOptions.ConfigName));

            var logPath = Path.Combine(Environment.CurrentDirectory, "log/log_.log");
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Logging.AddSerilog(logger);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<NorthwindContext>();
            

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
