
using bookify_data;
using bookify_data.Data;
using bookify_data.Helper;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using bookify_data.Repositories;

namespace bookify_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection("AWS"));

            builder.Services.AddControllers();
			builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.Configure<OpenAIConfig>(builder.Configuration.GetSection("OpenAI"));

			// Load appsettings.json configuration
			builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			builder.Configuration.AddJsonFile("appsettings.json"); 
			AppSettings.Configure(builder.Configuration);
			string connectionString = builder.Configuration.GetConnectionString("BookifyDb");

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("Database connection string 'BookifyDb' is missing in appsettings.json.");
			}

			builder.Services.AddDbContext<BookifyDbContext>(options =>
				options.UseSqlServer(connectionString, sqlOptions =>
				{
					sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
					sqlOptions.CommandTimeout(30);
				})
	   );
			builder.Services
					.AddRepository()
					.AddServices();

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
			builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRoleService, RoleService>();
			builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<INoteService, NoteService>();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<INewsService, NewsService>();
			builder.Services.AddScoped<IBookRepository, BookRepository>();
			builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddSingleton<AmazonS3Service>(); // Lưu trữ ảnh trên AWS S3
            builder.Services.AddScoped<IBookshelfRepository, BookshelfRepository>();
            builder.Services.AddScoped<IBookshelfService, BookshelfService>();
			builder.Services.AddScoped<IBookshelfDetailRepository, BookshelfDetailRepository>();
			builder.Services.AddScoped<IBookshelfDetailService, BookshelfDetailService>();
			builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
			builder.Services.AddScoped<IAuthorService, AuthorService>();
			builder.Services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
			builder.Services.AddScoped<IBookAuthorService, BookAuthorService>();
			builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
			builder.Services.AddScoped<IWishlistService, WishlistService>();
			builder.Services.AddScoped<IWishlistDetailRepository, WishlistDetailRepository>();
			builder.Services.AddScoped<IWishlistDetailService, WishlistDetailService>();


            #region configure jwt authentication
            builder.Services.AddHttpContextAccessor();

			// services.addi<IdentityUser>();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
		   .AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Cookies["authToken"];
						if (!string.IsNullOrEmpty(accessToken))
						{
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					}
				};
			}).AddCookie(options =>
			{
				options.LoginPath = "/account/login";
				options.LogoutPath = "/account/logout";
				options.SlidingExpiration = true;
			});
			#endregion
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookify", Version = "v1.0" });

				// Add security definitions for JWT Bearer and Cookie-based authentication
				//c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				//{
				//    In = ParameterLocation.Header,
				//    Description = "Please enter a valid JWT token",
				//    Name = "Authorization",
				//    Type = SecuritySchemeType.Http,
				//    BearerFormat = "JWT",
				//    Scheme = "Bearer"
				//});
				//c.OperationFilter<SwaggerFileOperationFilter>();
				c.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Cookie,
					Description = "Please enter a valid cookie",
					Name = "authToken",
					Type = SecuritySchemeType.ApiKey
				});
				// Add security requirements for both JWT and Cookie
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new string[] { }
			},
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Cookie"
					}
				},
				new string[] { }
			}
		});

				
			});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") 
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
           /* if (app.Environment.IsDevelopment())
            {*/
                app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookify API V1");
					//    c.RoutePrefix = string.Empty;
					c.InjectJavascript("/swagger/custom-swagger.js");
				});
			/*}*/
			app.UseAuthentication();
			app.UseAuthorization();
            //app.UseCors("AllowSpecificOrigins");



            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
