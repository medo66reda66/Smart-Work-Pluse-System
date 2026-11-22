
using Microsoft.EntityFrameworkCore;

using Scalar;
using Scalar.AspNetCore;

using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Entities.DAL.Entities;
using SmartWorkSpaseADL.Repositories.Classes;
using SmartWorkSpaseADL.Repositories.Interfaces;

using SmartWorkSpasePLL.Services.Classes;
using SmartWorkSpasePLL.Services.Interfaces;

namespace SmartWorkSpasePL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // OpenAPI and Scalar
            builder.Services.AddOpenApi();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            // Company Settings
            builder.Services.AddScoped<
                IRepository<CompanySetting>,
                Repository<CompanySetting>
            >();

            builder.Services.AddScoped<
                ICompanySettingService,
                CompanySettingService
            >();

            // Overtime Repositories
            builder.Services.AddScoped<
                IRepository<OvertimeSession>,
                Repository<OvertimeSession>
            >();

            builder.Services.AddScoped<
                IRepository<Project>,
                Repository<Project>
            >();

            // Overtime Service
            builder.Services.AddScoped<
                IOvertimeSessionService,
                OvertimeSessionService
            >();

            var app = builder.Build();

            // Development
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
