using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProniaWebSeyid.Contexts;

namespace ProniaWebSeyid
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
