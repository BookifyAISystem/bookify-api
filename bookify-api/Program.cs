using bookify_api.Repositories.Implementations;
using bookify_api.Repositories;
using bookify_api.Services.Implementations;
using bookify_api.Services;
using bookify_data.Data;
using Microsoft.EntityFrameworkCore;

namespace bookify_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ??c c?u h�nh t? appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // C?u h�nh DbContext v?i SQL Server
            builder.Services.AddDbContext<BookifyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookifyDb"),
                    sqlOptions => sqlOptions.MigrationsAssembly("bookify-data")));

            // ??ng k� c�c repository v� service
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IBookService, BookService>();

            // ??ng k� c�c d?ch v? kh�c (Swagger, Controllers)
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Sau khi ??ng k� t?t c? d?ch v?, m?i g?i builder.Build()
            var app = builder.Build();

            // C?u h�nh pipeline cho ?ng d?ng
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
