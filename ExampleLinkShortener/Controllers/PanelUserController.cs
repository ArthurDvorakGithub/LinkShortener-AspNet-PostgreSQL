using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleLinkShortener.Controllers
{
    public class PanelUserController : Controller
    {
        private readonly IShortenerService _shortenerService;

        public PanelUserController(IShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }
        public IActionResult Edit(string id)
        {
            var entity = id == default ? new UserLink() : _shortenerService.GetUserLinkById(id);
            return View(entity);
        }

        public IActionResult Index()
        {
            return View(_shortenerService.GetAllUserLinks());
        }

        [HttpPost]
        public IActionResult Edit(UserLink model)
        {
            if (ModelState.IsValid)
            {
                _shortenerService.SaveUserLink(model);
                return RedirectToAction("Index", "PanelUser");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            _shortenerService.DeleteUserLink(id);
            return RedirectToAction("Index", "PanelUser");
        }
    }
}
