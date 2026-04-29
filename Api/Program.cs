using Api.Middlewares;
using Application.Hubs;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddSwaggerGen();

            // 2. Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. Infrastructure
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicineDrawerRepository, MedicineDrawerRepository>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

            // 4. Application 
            builder.Services.AddValidatorsFromAssembly(typeof(PatientDtoValidator).Assembly);
            builder.Services.AddSignalR();

            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedicineDrawerService, MedicineDrawerService>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

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

            app.UseAuthorization();
            app.MapHub<RobotHub>("/robot-hub");
            app.MapControllers();

            app.Run();
        }
    }
}