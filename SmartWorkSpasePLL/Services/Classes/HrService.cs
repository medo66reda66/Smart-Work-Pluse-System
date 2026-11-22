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
    public class HrService : IHrService
    {
        private readonly IRepository<Notification> _notificationRepository;

        public HrService(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
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
    }
}
