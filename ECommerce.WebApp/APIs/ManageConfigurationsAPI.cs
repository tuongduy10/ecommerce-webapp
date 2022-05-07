using ECommerce.Application.Services.Bank;
using ECommerce.Application.Services.Bank.Dtos;
using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageConfigurationsAPI : ControllerBase
    {
        private IConfigurationService _configurationService;
        private IFooterService _footerService;
        private IBankService _bankService;
        public ManageConfigurationsAPI(IConfigurationService configurationService, IFooterService footerService, IBankService bankService)
        {
            _configurationService = configurationService;
            _footerService = footerService;
            _bankService = bankService;
        }

        [HttpPost("UpdateBlog")]
        public async Task<IActionResult> UpdateBlog(BlogUpdateRequest request)
        {
            var result = await _footerService.UpdateBlog(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("UpdateSocial")]
        public async Task<IActionResult> UpdateSocial(SocialUpdateRequest request)
        {
            var result = await _footerService.UpdateSocial(request);
            if (!result.isSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpPost("DeleteBank")]
        public async Task<IActionResult> DeleteBank([FromBody] int bankId)
        {
            var result = await _bankService.deleteBank(bankId);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("UpdateBank")]
        public async Task<IActionResult> UpdateBank([FromBody] BankUpdateRequest request)
        {
            var result = await _bankService.updateBank(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("AddBank")]
        public async Task<IActionResult> AddBank([FromBody] BankAddRequest request)
        {
            var result = await _bankService.addBank(request);
            if (result.isSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
