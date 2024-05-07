using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using AmdinPanelMVC.Filters;

namespace AmdinPanelMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            HttpContext.Response.Cookies.SetTokens("asd", "sdf");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel login)
        {
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(login);
        }

        [JwtAuthorize] 
        public IActionResult Profile()
        {
            return View("Login");
        }
    }
}
