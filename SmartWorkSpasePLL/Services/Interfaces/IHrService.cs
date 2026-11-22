using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IHrService
    {
        Task<List<Notification>> GetAllNotification(string userId);
        Task<Notification> GetNotificationById(int id, string userId);
    }
}
