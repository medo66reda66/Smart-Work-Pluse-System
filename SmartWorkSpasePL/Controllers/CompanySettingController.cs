using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWorkSpasePLL.Dtos.Request;
using SmartWorkSpasePLL.Services.Interfaces;

namespace SmartWorkSpasePL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySettingController : ControllerBase
    {
        private readonly ICompanySettingService _companySettingService;

        public CompanySettingController(ICompanySettingService companySettingService)
        {
            _companySettingService = companySettingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanySetting()
        {
            var companySettings = await _companySettingService.GetCompanySettingAsync();
            if (companySettings == null || !companySettings.Any())
            {
                return NotFound();
            }
            return Ok(companySettings);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetCompanySettingById(int id)
        {
            var companySetting = await _companySettingService.GetCompanySettingByIdAsync(id);
            if (companySetting == null)
            {
                return NotFound();
            }
            return Ok(companySetting);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanySetting([FromBody] CompanySettingRequest companySettingRequest)
        {
            await _companySettingService.AddCompanySettingAsync(companySettingRequest);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanySetting(int id, [FromBody] CompanySettingRequest companySettingRequest)
        {
            var result = await _companySettingService.UpdateCompanySettingAsync(id, companySettingRequest);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanySetting(int id)
        {
            var result = await _companySettingService.DeleteCompanySettingAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
