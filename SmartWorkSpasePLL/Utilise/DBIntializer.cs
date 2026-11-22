
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using SmartWorkSpaseADL.Data.Context;
using SmartWorkSpaseADL.Entities;
using System.IO;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Utilise
{
    public class DBIntializer : IDBIntializer
    {
        private readonly AppDbContext _Context;
        private readonly ILogger<DBIntializer> _logger;
        private readonly UserManager<Applicationuser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public DBIntializer(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<Applicationuser> userManager, ILogger<DBIntializer> logger)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
            _logger = logger;
        }

        public async Task Intialize()
        {
            try
            {
                if (_Context.Database.GetPendingMigrations().Any())
                {
                    _Context.Database.Migrate();
                }
                if(!_RoleManager.Roles.Any())
                {
                    _RoleManager.CreateAsync(new(DS.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.MANAGER_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.HR_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.EMPLOYEE_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.FINANCE_ROLE)).GetAwaiter().GetResult();

                   var result = await _UserManager.CreateAsync(new Applicationuser
                    {
                        UserName="Admin",
                        Email="Admin123@Hoda.com",
                        FullName="Mahmoud Reda Zahra",
                        PhoneNumber="01120811023",
                        EmailConfirmed = true,
                        IsActive = true,
                        Department = "Admin",
                        City = "Cairo",
                        Gavernate = "Cairo",
                        ManagerId = null,
                       gender = Gender.Male,
                   },"admin123#");

                    if (!result.Succeeded) 
                    {
                        foreach (var error in result.Errors)
                            Console.WriteLine(error.Description);
                    }

                    var user = await _UserManager.FindByEmailAsync("Admin123@Hoda.com");

                   await _UserManager.AddToRoleAsync(user!,DS.ADMIN_ROLE);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Erorr{ex.Message}");
            }
        }


    }
}
