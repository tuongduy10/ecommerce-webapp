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
        public ManageConfigurationsAPI(IConfigurationService configurationService, IFooterService footerService)
        {
            _configurationService = configurationService;
            _footerService = footerService;
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
    }
}
