﻿using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;
using AmdinPanelMVC.Models;
using AmdinPanelMVC.Services.Interfaces;
using AmdinPanelMVC.Helpers;
using AmdinPanelMVC.Filters;
using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Mappers;
using Common.Models.Models;
using Common.API.Helpers;
using Common.Models.DTOs.Admission;

namespace AmdinPanelMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService _userService;

        public UserController(IAuthService userService)
        {
            _userService = userService;
        }

        [RequiredUnauthorize]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredUnauthorize]
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

            return Redirect("Profile");
        }

        [HttpGet]
        [RequiredAuthorize]
        public async Task<IActionResult> Profile()
        {
            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/Error");
            }

            ExecutionResult<ManagerProfileDTO> result = await _userService.GetManagerProfileAsync(managerId);
            if (!result.IsSuccess)
            {
                return Redirect("/Error");
            }

            return View(result.Result!.ToProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredAuthorize]
        public async Task<IActionResult> Email(ProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", profile);
            }

            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/Error");
            }

            ExecutionResult response = await _userService.ChangeEmailAsync(managerId, profile.Email);
            if (!response.IsSuccess)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("Email", error.Value[0]);
                }
                return View("Profile", profile);
            }

            return Redirect("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredAuthorize]
        public async Task<IActionResult> FullName(ProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", profile);
            }

            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/Error");
            }

            ExecutionResult response = await _userService.ChangeFullNameAsync(managerId, profile.FullName);
            if (!response.IsSuccess)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("FullName", error.Value[0]);
                }
                return View("Profile", profile);
            }

            return Redirect("Profile");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredAuthorize]
        public async Task<IActionResult> ChangePassword(ProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", profile);
            }

            if (!HttpContext.TryGetUserId(out Guid managerId))
            {
                return Redirect("/Error");
            }

            ExecutionResult response = await _userService.ChangePasswordAsync(managerId, 
                new() { CurrentPassword = profile.CurrentPassword!, NewPassword = profile.NewPassword!});
            if (!response.IsSuccess)
            {
                foreach (var error in response.Errors)
                {
                    if (error.Key.Contains("PasswordMismatch")) 
                    { 
                        ModelState.AddModelError("CurrentPassword", "Неверный текущий пароль");
                    }
                    else if (error.Key.Contains(""))
                    {
                        ModelState.AddModelError("NewPassword", error.Value[0]);
                    }
                }
                return View("Profile", profile);
            }

            return Redirect("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequiredAuthorize]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.TryGetAccessTokenJTI(out Guid accessTokenJTI))
            {
                ExecutionResult result = await _userService.LogoutAsync(accessTokenJTI);

                if (result.IsSuccess)
                {
                    HttpContext.Response.Cookies.RemoveTokens();
                    return Redirect("Login");
                }

                if (result.Errors.TryGetKey("LogoutFail", out _))
                {
                    HttpContext.Response.Cookies.RemoveTokens();
                }
            }

            StringValues from = HttpContext.Request.Headers["Referer"];
            return Redirect("/InternalServer");
        }

    }
}
