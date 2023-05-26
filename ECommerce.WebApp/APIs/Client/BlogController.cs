using ECommerce.Application.Constants;
using ECommerce.Application.Services.Configurations;
using ECommerce.Application.Services.Configurations.Dtos.Footer;
using ECommerce.WebApp.Models.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs.Client
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IFooterService _footerService;
        public BlogController(
            IFooterService footerService
        )
        {
            _footerService = footerService;
        }
        [HttpPost("get-blog")]
        public async Task<IActionResult> GetBlog(BlogDto request)
        {
            var res = await _footerService.GetBlog(request);
            if (res.isSucceed)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}
