using Microsoft.AspNetCore.Identity;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Services.Interfaces;
using SmartWorkSpasePLL.Utilise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class FinanceService : IFinanceService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly UserManager<Applicationuser> _userManager;

        public FinanceService(IRepository<Notification> notificationRepository, UserManager<Applicationuser> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }
        public async Task<List<Notification>> GetAllNotification(string userId)
        {
            var AllNotifications =await _notificationRepository.GetAllAsync(n => n.ApplicationUserId == userId);
            return AllNotifications.ToList();
        }
        public async Task<Notification> GetNotificationById(int id ,string userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(n => n.Id == id && n.ApplicationUserId == userId);
            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> AddBasicSalaryToUser(string userId,string financeUserId, decimal basicSalary)
        {
            var user =await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.BasicSalary = basicSalary;
            var result =await _userManager.UpdateAsync(user);
            var HRUser = await _userManager.GetUsersInRoleAsync(DS.HR_ROLE);
            if (HRUser == null)
            {
                return false;
            }
            foreach (var hruser in HRUser)
            {
                var notification = new Notification
                {
                    ApplicationUserId = hruser.Id,
                    Type = "Finance",
                    Message = $"Your basic salary has been updated to {basicSalary:C}.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    RelatedUserId = financeUserId

                };
                _notificationRepository.Add(notification);
            }
        
            await _notificationRepository.SaveChangesAsync();
            return result.Succeeded;
        }
    }
}
