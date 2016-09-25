using EWiki.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWiki.Api.Controllers
{
    [Authorize]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
                if (result.Succeeded)
                {
                    return Json(new { success = true, responseText = "Login successed!" });
                }
                else
                {
                    return Json(new { success = false, responseText = "Login failed!" });
                }
            }

            return Json(new { success = false, responseText = "Invalid data" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("LogOff")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Json(new { success = true, responseText = "Logout successfully!" });
        }
    }
}
