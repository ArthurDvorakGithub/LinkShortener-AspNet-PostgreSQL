using System.Collections.Generic;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ExampleLinkShortener.Controllers
{
    //[Authorize(Roles = "User")]
    public class PanelUserController : Controller
    {
        private readonly IShortenerService _shortenerService;
        private readonly IProjectService _projectService;

        public PanelUserController(IShortenerService shortenerService, IProjectService projectService)
        {
            _shortenerService = shortenerService;
            _projectService = projectService;
        }
        
        // Links

        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userLinks = _shortenerService.GetLinksByUserId(userId);

            foreach (var userLink in userLinks)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

                userLink.LinkCode = $"{baseUrl}/u/{userLink.LinkCode}";
            }

            return View(userLinks);
        }

        public IActionResult EditLink(string id)
        {
            var entity = id == default ? new UserLink() : _shortenerService.GetUserLinkById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult EditLink(UserLink model)
        {
            if (ModelState.IsValid)
            {
                _shortenerService.SaveUserLink(model);
                return RedirectToAction("Index", "PanelUser");
            }
            return View(model);
        }

        public IActionResult EditProject(string id)
        {
            var entity = id == default ? new Project() : _projectService.GetProjectById(id);
            return View(entity);
        }

        [HttpPost]
        public IActionResult EditProject(Project model)
        {
            if (ModelState.IsValid)
            {
                _projectService.SaveProject(model);
                return RedirectToAction("Index", "PanelUser");
            }
            return View(model);
        }

        //Shortener

        [HttpGet]
        public IActionResult Shortify(string id)
        {
            
            return View(new UserLink { ProjectId = id});
        }

        [HttpPost]
        //[HttpPost("shortify/{projectId}")]

        public async Task<IActionResult> Shortify(UserLink model)
        {

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _shortenerService.Shortify(model.ProjectId, model.LinkName,model.Link, userId);

                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

                model.LinkCode = $"{baseUrl}/u/{result}" ;

                if(model.ProjectId == null)
                {
                    return RedirectToAction("Index", "PanelUser");
                }
                else
                {
                    return RedirectToAction("GetProjects", "PanelUser");
                }
                

            }
            
            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            _shortenerService.DeleteUserLink(id);
            return RedirectToAction("Index", "PanelUser");
        }

        // Projects

        public async Task<IActionResult> GetProjects()
        { 
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await _projectService.GetAll(userId);

            return View(projects);
        }

        public async Task<IActionResult> GetProjectLinksById(Project model)
        {
            var projectLinks = await _projectService.GetById(model.Id);
            var links = new List<UserLink>();
            
            foreach (var userLink in projectLinks.Links)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                userLink.LinkCode = $"{baseUrl}/u/{userLink.LinkCode}";
                
                links.Add(userLink);
            }

            projectLinks.Links = links;

            return View(projectLinks);
        }

        public IActionResult DeleteProjectsPanel(Project model)
        {

            _projectService.DeleteProject(model.Id);
            
            return RedirectToAction("GetProjects", "PanelUser");
        }

        public IActionResult CreateProject() => View();


        [HttpPost]
        public async Task<IActionResult> CreateProject(Project model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _projectService.Create(model);
                
                return RedirectToAction("GetProjects", "PanelUser");

            }
            return View(model);
        }

    }
}
