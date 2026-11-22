using SmartWorkSpaseADL.Entities.DAL.Entities;
using SmartWorkSpaseADL.Repositories.Classes;
using SmartWorkSpaseADL.Repositories.Interfaces;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkSpasePLL.Services.Classes
{
    public class CompanySettingService : ICompanySettingService
    {
        private readonly IRepository<CompanySetting> _companySettingRepository;

        public CompanySettingService(IRepository<CompanySetting> companySettingRepository)
        {
            _companySettingRepository = companySettingRepository;
        }

        public async Task<List<CompanySetting>> GetCompanySettingAsync()
        {
            return await _companySettingRepository.GetAllAsync();
        }
        public async Task<CompanySetting> GetCompanySettingByIdAsync(int id)
        {
            return await _companySettingRepository.GetByIdAsync(x => x.Id == id);
        }
        public async Task AddCompanySettingAsync(CompanySettingRequest companySettingRequest)
        {
            var companySetting = new CompanySetting
            {
                WorkStartTime = companySettingRequest.WorkStartTime,
                WorkEndTime = companySettingRequest.WorkEndTime,
                BlueWindowMinutes = companySettingRequest.BlueWindowMinutes,
                GreenCardsPerEarlyArrival = companySettingRequest.GreenCardsPerEarlyArrival,
                BlueCardsPerLateArrival = companySettingRequest.BlueCardsPerLateArrival,
                BlackCardsPerLateArrival = companySettingRequest.BlackCardsPerLateArrival,
                BlackCardsPerAbsence = companySettingRequest.BlackCardsPerAbsence,
                MonthlyApprovedLeaveDays = companySettingRequest.MonthlyApprovedLeaveDays,
                GreenForReward = companySettingRequest.GreenForReward,
                BlueForFine = companySettingRequest.BlueForFine,
                BlackForFine = companySettingRequest.BlackForFine,
                RewardAmount = companySettingRequest.RewardAmount,
                BlueFineAmount = companySettingRequest.BlueFineAmount,
                BlackFineAmount = companySettingRequest.BlackFineAmount,
                RequireFinePaymentBeforeCheckIn = companySettingRequest.RequireFinePaymentBeforeCheckIn,
                EnableOvertime = companySettingRequest.EnableOvertime,
                OvertimeMultiplier = companySettingRequest.OvertimeMultiplier,
                HolidayOvertimeMultiplier = companySettingRequest.HolidayOvertimeMultiplier,
                OvertimeBaseRatePerHour = companySettingRequest.OvertimeBaseRatePerHour,
                WorkingHoursPerDay = companySettingRequest.WorkingHoursPerDay,
                MaxOvertimeMinutes = companySettingRequest.MaxOvertimeMinutes
            };

            _companySettingRepository.Add(companySetting);
            await _companySettingRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateCompanySettingAsync(int id, CompanySettingRequest companySettingRequest)
        {
            var existingCompanySetting = await _companySettingRepository.GetByIdAsync(x => x.Id == id);
            if (existingCompanySetting != null)
            {
                existingCompanySetting.WorkStartTime = companySettingRequest.WorkStartTime;
                existingCompanySetting.WorkEndTime = companySettingRequest.WorkEndTime;
                existingCompanySetting.BlueWindowMinutes = companySettingRequest.BlueWindowMinutes;
                existingCompanySetting.GreenCardsPerEarlyArrival = companySettingRequest.GreenCardsPerEarlyArrival;
                existingCompanySetting.BlueCardsPerLateArrival = companySettingRequest.BlueCardsPerLateArrival;
                existingCompanySetting.BlackCardsPerLateArrival = companySettingRequest.BlackCardsPerLateArrival;
                existingCompanySetting.BlackCardsPerAbsence = companySettingRequest.BlackCardsPerAbsence;
                existingCompanySetting.MonthlyApprovedLeaveDays = companySettingRequest.MonthlyApprovedLeaveDays;
                existingCompanySetting.GreenForReward = companySettingRequest.GreenForReward;
                existingCompanySetting.BlueForFine = companySettingRequest.BlueForFine;
                existingCompanySetting.BlackForFine = companySettingRequest.BlackForFine;
                existingCompanySetting.RewardAmount = companySettingRequest.RewardAmount;
                existingCompanySetting.BlueFineAmount = companySettingRequest.BlueFineAmount;
                existingCompanySetting.BlackFineAmount = companySettingRequest.BlackFineAmount;
                existingCompanySetting.RequireFinePaymentBeforeCheckIn = companySettingRequest.RequireFinePaymentBeforeCheckIn;
                existingCompanySetting.EnableOvertime = companySettingRequest.EnableOvertime;
                existingCompanySetting.OvertimeMultiplier = companySettingRequest.OvertimeMultiplier;
                existingCompanySetting.HolidayOvertimeMultiplier = companySettingRequest.HolidayOvertimeMultiplier;
                existingCompanySetting.OvertimeBaseRatePerHour = companySettingRequest.OvertimeBaseRatePerHour;
                existingCompanySetting.WorkingHoursPerDay = companySettingRequest.WorkingHoursPerDay;
                existingCompanySetting.MaxOvertimeMinutes = companySettingRequest.MaxOvertimeMinutes;
                _companySettingRepository.Update(existingCompanySetting);
                await _companySettingRepository.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteCompanySettingAsync(int id)
        {
            var existingCompanySetting = await _companySettingRepository.GetByIdAsync(x => x.Id == id);
            if (existingCompanySetting != null)
            {
                _companySettingRepository.Delete(existingCompanySetting);
                await _companySettingRepository.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
