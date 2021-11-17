using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleLinkShortener.Controllers
{
    public class PanelAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
