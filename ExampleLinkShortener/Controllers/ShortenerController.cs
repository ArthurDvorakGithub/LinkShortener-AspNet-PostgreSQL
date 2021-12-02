using System.Security.Claims;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
namespace ExampleLinkShortener.Controllers
{
    //[ApiController]
    //[Route("shortener")]
    public class ShortenerController : Controller
    {
        private readonly IShortenerService _shortenerService;

        public ShortenerController(IShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }

        [HttpPost("shortify/{projectId}")]
        
        public async Task<IActionResult> Shortify(UserLink model)
        {

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _shortenerService.Shortify(model.ProjectId, model.LinkName, model.Link, userId); // string projectId,

                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

                return Ok(new { url = $"{baseUrl}/u/{result}" });

                return RedirectToAction("PanelUser","Index");

            }
            
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