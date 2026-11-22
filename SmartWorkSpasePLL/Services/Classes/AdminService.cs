using Microsoft.AspNetCore.Identity;
using SmartWorkSpaseADL.Entities;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly UserManager<Applicationuser> _userManager;

        public AdminService(IRepository<Notification> notificationRepository, UserManager<Applicationuser> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public async Task<List<Notification>> GetAllNotification(string userId)
        {
            var AllNotifications = await _notificationRepository.GetAllAsync(n => n.ApplicationUserId == userId);
            return AllNotifications.ToList();
        }
        public async Task<Notification> GetNotificationById(int id, string userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(n => n.Id == id && n.ApplicationUserId == userId);
            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
            return notification;
        }
        public async Task<bool> AddBasicSalaryToHRANDFinance(string userId, decimal basicSalary)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.BasicSalary = basicSalary;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
