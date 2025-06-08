using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PD_Store.Helper;
using PD_Store.Repositories.Auth;
using PD_Store.ViewModels.Auth;

namespace PD_Store.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }
        //Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestVM model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(model, returnUrl ?? "");

                if (result.Status == Contants.StatusCodeSuccessed)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    ViewBag.RedirectUrl = returnUrl ?? "/";
                    return RedirectToAction("Index", "Home");
                }
                //Login fail
                ModelState.AddModelError("", result.Message ?? Contants.DefaultMessageError);
                ViewBag.ErrorMessage = result.Message ?? "Đăng ký thất bại!";
            }

            return View(model);
        }

        //Login
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestVM login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(login, returnUrl ?? "");
                if (result.Status == Contants.StatusCodeSuccessed)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    var redirectUrl = returnUrl ?? "/";
                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    TempData["RedirectUrl"] = redirectUrl;
                    // return RedirectToAction("Index", login);
                    return RedirectToAction("Index", "Home");
                }
                //Login fail
                ModelState.AddModelError("", result.Message ?? Contants.DefaultMessageError);
                ViewBag.ErrorMessage = result.Message ?? "Đăng nhập thất bại!";
            }
            return View("Index", login);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}