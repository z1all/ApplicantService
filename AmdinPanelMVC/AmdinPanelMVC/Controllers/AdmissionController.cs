using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.Models;

namespace AmdinPanelMVC.Controllers
{
    [RequiredAuthorize]
    public class AdmissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAdmission([FromBody] AdmissionsFilterViewModel filter)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return ViewComponent("Admissions", filter);
        }
    }
}
