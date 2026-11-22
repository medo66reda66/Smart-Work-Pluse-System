using SmartWorkSpaseADL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<Notification>> GetAllNotification(string userId);
        Task<Notification> GetNotificationById(int id, string userId);
        Task<bool> AddBasicSalaryToHRANDFinance(string userId, decimal basicSalary);
    }
}
