
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Application.Validators;
using Core.Exceptions;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api
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

            // Infrastructure
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicineDrawerRepository, MedicineDrawerRepository>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();



            // Application
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(PatientDtoValidator).Assembly);
            builder.Services.AddSignalR();

            builder.Services.AddScoped<IMedicineDrawerService, MedicineDrawerService>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            builder.Services.AddScoped<IPatientService, PatientService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            //API
            builder.Services.AddScoped<IPatientService, PatientService>();



            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
            var app = builder.Build();

            app.UseCors("AllowAll");

            //Global Exception Handling
            app.UseExceptionHandler(opt =>
            {
                opt.Run(async context =>
                {
                    var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionDetails?.Error;

                    if (exception is NotFoundException)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsJsonAsync(new { error = exception.Message });
                    }
                    else
                    {
                        // For unexpected errors
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(new { error = "An unexpected server error occurred." });
                    }
                });
            });

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
