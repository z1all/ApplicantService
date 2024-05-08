using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.DTOs;
using Common.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            ExecutionResult<LoginResponseDTO> response = await _userService.LoginAsync(new()
            {
                Email = login.Email,
                Password = login.Password,
            });

            if (!response.IsSuccess)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error.Value[0]);
                }

                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, response.Result!.FullName)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            return Redirect("Login");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View("Login");
        }
    }
}
