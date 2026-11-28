
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar;
using Scalar.AspNetCore;

using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Entities.DAL.Entities;
using SmartWorkSpaseADL.Repositories.Classes;
using SmartWorkSpaseADL.Repositories.Interfaces;

using SmartWorkSpasePLL.Services.Classes;
using SmartWorkSpasePLL.Services.Interfaces;
using SmartWorkSpasePLL.Utilise;
using System;
using System.Text;
using System.Threading.Tasks;
namespace SmartWorkSpasePL
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped< IOvertimeSessionService, OvertimeSessionService >();

            // Development
            builder.Services.AddIdentity<Applicationuser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.User.RequireUniqueEmail = false;
                option.SignIn.RequireConfirmedEmail = false;
            })
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();


            builder.Services.AddScoped<IRepository<CompanySetting>, Repository<CompanySetting>>();
            builder.Services.AddScoped<IRepository<Notification>, Repository<Notification>>();
            builder.Services.AddScoped<ICompanySettingService, CompanySettingService>();
            builder.Services.AddScoped<IApplicationuserAuthServise, ApplicationuserAuthServise>();
            builder.Services.AddScoped<IFinanceService, FinanceService>();
            builder.Services.AddScoped<IHrService, HrService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IDBIntializer, DBIntializer>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IAttendanceRecordAndCardService, AttendanceRecordAndCardService>();
            builder.Services.AddScoped<IRepository<Card>, Repository<Card>>();
            builder.Services.AddScoped<IRepository<AttendanceRecord>, Repository<AttendanceRecord>>();
            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();


            builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = "https://localhost:7295",
                   ValidAudience = "https://localhost:7295",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Nf7$Pq19!sD@84LmZ#xT2wQvR%k9Hp36"))
               };
           });

            builder.Services.AddHangfire(config =>
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(
                        builder.Configuration.GetConnectionString("DefaultConnection")
                    )
            );

            builder.Services.AddHangfireServer();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBIntializer>();
            await service!.Intialize();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<IAttendanceRecordAndCardService>(
                "Daily-Checkin",
                service => service.RunDailyCheckin(),
                "30 16 * * 0-4",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                        "Egypt Standard Time"
                    )
                }
            );

            app.MapControllers();

            app.Run();
        }
    }
}
