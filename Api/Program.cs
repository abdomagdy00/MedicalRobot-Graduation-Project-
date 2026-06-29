using Api.Middlewares;
using Application.Hubs;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using Core.Interfaces;
using Core.Services;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Basics
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Medical Robot API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the token here like this: Bearer {your_token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            // 2. Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. Infrastructure
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicineDrawerRepository, MedicineDrawerRepository>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<IRobotSettingRepository, RobotSettingRepository>();
            builder.Services.AddIdentityServices(builder.Configuration);


            // 4. Application 
            builder.Services.AddValidatorsFromAssembly(typeof(PatientDtoValidator).Assembly);
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<RobotConnectionTracker>();

            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedicineDrawerService, MedicineDrawerService>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            builder.Services.AddScoped<IRobotService, RobotService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            //5.API
            builder.Services.AddSingleton<FaceEnrollmentSessionManager>();

            // 5.  CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("RobotPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("RobotPolicy");

            // app.UseHttpsRedirection(); 
            app.UseWebSockets();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            //app.MapHub<RobotHub>("/robot-hub").RequireAuthorization();
            app.MapHub<RobotHub>("/robot-hub");

            app.Run();
        }
    }
}