using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
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
                    return RedirectToAction("Index", "Home");
                }
                //Login fail
                ModelState.AddModelError("", result.Message ?? Contants.DefaultMessageError);
            }
            return View(login);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}