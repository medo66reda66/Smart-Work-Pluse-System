using SmartWorkSpaseADL.Entities.DAL.Entities;
using SmartWorkSpasePLL.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Interfaces
{
    public interface ICompanySettingService
    {
        Task<List<CompanySetting>> GetCompanySettingAsync();

        Task<CompanySetting> GetCompanySettingByIdAsync(int id);

        Task AddCompanySettingAsync(CompanySettingRequest companySettingRequest);

        Task<bool> UpdateCompanySettingAsync(int id, CompanySettingRequest companySettingRequest);

        Task<bool> DeleteCompanySettingAsync(int id);
    }
}
