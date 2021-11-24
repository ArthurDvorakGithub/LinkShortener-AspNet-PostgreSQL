using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Models;
using ExampleLinkShortener.Services;
using ExampleLinkShortener.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ExampleLinkShortener.Controllers
{
    
    public class ShortenerController : Controller
    {
        private readonly IShortenerService _shortenerService;

        public ShortenerController(IShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }


        [HttpPost("shortify")]
        
        public async Task<IActionResult> Shortify(ShortenerLinkViewModel model)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var result = await _shortenerService.Shortify(model.Link, userId);

            //var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            //return Ok(new { url = $"{baseUrl}/u/{result}" });

            if(ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _shortenerService.Shortify(model.Link, userId); // linkCode вернуло

                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

                return Ok(new { url = $"{baseUrl}/u/{result}" });

            }

            //return RedirectToAction("UserPanel", "Index");
            return View(model);

            
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