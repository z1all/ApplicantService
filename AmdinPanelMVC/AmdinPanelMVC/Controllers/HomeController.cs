using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Filters;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        { 
            return View();
        }

        public IActionResult NotFoundError()
        {
            return View("NotFound");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
