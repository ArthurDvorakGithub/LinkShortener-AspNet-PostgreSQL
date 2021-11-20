using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ExampleLinkShortener.Models;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExampleLinkShortener.Controllers
{
    [ApiController]
    //[Route("shortener")]
    public class ShortenerController : ControllerBase
    {
        private readonly IShortenerService _shortenerService;

        public ShortenerController(IShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }

        [HttpPost("shortify")]
        
        public async Task<IActionResult> Shortify(string url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _shortenerService.Shortify(url, userId);

            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            return Ok(new { url = $"{baseUrl}/u/{result}" });
        }

        [HttpGet("u/{encodedUrl}")]
        public async Task<IActionResult> RedirectToLink(string encodedUrl)
        {
            var link = await _shortenerService.GetLink(encodedUrl);

            if (string.IsNullOrWhiteSpace(link))
            {
                return BadRequest();
            }

            return Redirect(link);
        }

        //Управление ссылками в кабинете

      
    }
}