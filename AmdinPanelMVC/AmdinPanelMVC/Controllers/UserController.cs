using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Models;

namespace AmdinPanelMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel login)
        {
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(login);
        }
    }
}
