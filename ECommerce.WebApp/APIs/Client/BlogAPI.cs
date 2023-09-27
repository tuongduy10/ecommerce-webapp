using ECommerce.Application.Constants;
using ECommerce.Application.BaseServices.Configurations;
using ECommerce.Application.BaseServices.Configurations.Dtos.Footer;
using ECommerce.WebApp.Models.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.WebApp.APIs
{
    [Route("api/blog")]
    [ApiController]
    public class BlogAPI : ControllerBase
    {
        private readonly IFooterService _footerService;
        public BlogAPI(
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
