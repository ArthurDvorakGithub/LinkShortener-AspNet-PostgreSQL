using System;
using System.Threading.Tasks;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleLinkShortener.Controllers
{
    [ApiController]
    [Route("shortener")]
    public class ShortenerController : ControllerBase
    {
        private readonly IShortenerService _shortenerService;

        public ShortenerController(IShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }

        [HttpPost]
        public async Task<IActionResult> Shortify(string url)
        {
            var userId = Guid.NewGuid().ToString();//HttpContext.User.Identity;
            var result = await _shortenerService.Shortify(url, userId);

            return Ok(new {url = result});
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToLink(string encodedUrl)
        {
            var userId = Guid.NewGuid().ToString();//HttpContext.User.Identity;
            var link = await _shortenerService.GetLink(encodedUrl, userId);

            if (string.IsNullOrWhiteSpace(link))
            {
                return BadRequest();
            }

            return Redirect(link);
        }
    }
}