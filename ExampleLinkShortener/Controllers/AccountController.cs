using ExampleLinkShortener.Models;
using ExampleLinkShortener.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExampleLinkShortener.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        // login with facebook

        [HttpPost]
        public IActionResult ExternalLogin(string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            string provider = "Facebook";
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [TempData]
        public string ErrorMessage { get; set; }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(ExternalLogin));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(ExternalLogin));
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
                //_logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                //return RedirectToAction(nameof(returnUrl));
            }
            else
            {
                string email = string.Empty;
                string firstName = string.Empty;
                string lastName = string.Empty;
                string profileImage = string.Empty;
                //get google login user infromation like that.
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                    firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                    lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                }
                //this function is get facebook profile images
                var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                profileImage = $"https://graph.facebook.com/{identifier}/picture?type=large";
                var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var result2 = await _userManager.CreateAsync((User)user);
                if (result2.Succeeded)
                {
                    result2 = await _userManager.AddLoginAsync((User)user, info);
                    if (result2.Succeeded)
                    {
                        //do somethng here
                    }
                }
                return View("Login");
            }
        }
        //public IActionResult GoogleLogInSuccess()
        //{
        //    return View();
        //}
    }
}
