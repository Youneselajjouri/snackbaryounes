using Microsoft.EntityFrameworkCore;
using snackbaryounes.Models;
using Microsoft.OpenApi.Models;
using snackbaryounes.Controllers.API;
using System.Text.Json.Serialization;

namespace snackbaryounes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Voeg services toe aan de container.
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<MenuItemsAPIController>();


            builder.Services.AddControllersWithViews()
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
             });

            var test = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SnackbarDatabaseContext>(options =>
                options.UseSqlServer(test));

            // Voeg Swagger services toe
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Snackbar API", Version = "v1" });
            });

            var app = builder.Build();

            // Configureer de HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Schakel Swagger in tijdens ontwikkeling
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Snackbar API v1");
                    c.RoutePrefix = "swagger"; // Zorg dat Swagger UI direct beschikbaar is
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // API routes configureren
            app.MapControllers();

            // Standaard route configureren
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
