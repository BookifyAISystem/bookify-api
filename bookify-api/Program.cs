
using bookify_data;
using bookify_data.Data;
using bookify_service;
using Microsoft.EntityFrameworkCore;

namespace bookify_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Load appsettings.json configuration
            builder.Services.AddDbContext<BookifyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookifyDb"),b => b.MigrationsAssembly("bookify-data")));
			builder.Services
					.AddRepository() 
					.AddServices();
			builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			builder.Configuration.AddJsonFile("appsettings.json");
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
