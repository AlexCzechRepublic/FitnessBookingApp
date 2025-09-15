using FitnessBookingApp.Data;
using FitnessBookingApp.Services;
using Microsoft.EntityFrameworkCore;

namespace FitnessBookingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<DataService>();
            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(
                 builder.Configuration.GetConnectionString("DefaultConnection"),
                 sqlOptions => sqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 5,             // počet pokusů
                     maxRetryDelay: TimeSpan.FromSeconds(10),  // prodleva mezi pokusy
                     errorNumbersToAdd: null       // můžeš specifikovat SQL error kódy
                 )
             )
         );


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

            app.UseSession(); // ⚡ musí být před UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
