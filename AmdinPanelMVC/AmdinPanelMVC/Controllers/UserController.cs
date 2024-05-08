using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Helpers;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using Common.Models.Models;
using Common.API.Helpers;

namespace AmdinPanelMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _userService;

        // 1. +++ Отрефачить сделанное (Разделить на несколько проектов)
        // 2. +++ Отрефачить RPC
        // 3. +++ Добавить запрос на получение профиля и выход
        // 4. Сверстать хедер и профиль.

        public UserController(IAuthService userService)
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
            ExecutionResult<TokensResponseDTO> response = await _userService.LoginAsync(new()
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

            TokensResponseDTO loginResponse = response.Result!;
            HttpContext.Response.Cookies.SetTokens(loginResponse.JwtToken, loginResponse.RefreshToken);

            return Redirect("User/Profile");
        }

        [JwtAuthorize] 
        public async Task<IActionResult> Profile()
        {
            if(HttpContext.TryGetUserId(out Guid managerId) && HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI))
            {

                //ExecutionResult<ManagerDTO> result = await _userService.GetManagerProfileAsync(managerId);
                //ExecutionResult result = await _userService.LogoutAsync(accessTokenJTI);


            }


            return View("Login");
        }
    }
}
