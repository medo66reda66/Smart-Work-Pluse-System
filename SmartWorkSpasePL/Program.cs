using Microsoft.EntityFrameworkCore;
using Scalar;
using Scalar.AspNetCore;
using SmartWorkSpaseADL.Data.Context;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

           
            builder.Services.AddScoped<IRepository<CompanySetting>, Repository<CompanySetting>>();
            builder.Services.AddScoped<ICompanySettingService, CompanySettingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
