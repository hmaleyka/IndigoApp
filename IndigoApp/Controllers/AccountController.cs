using IndigoApp.Models;
using IndigoApp.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IndigoApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
         
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(Index), "Home");
            
        }
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM loginvm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginvm.EmailOrUsername);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginvm.EmailOrUsername);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username-Email or Password is incorrect");
                    return View();
                }
            }
            var result = _signInManager.CheckPasswordSignInAsync(user, loginvm.Password, true).Result;
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Try it after few seconds");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username-Email or password is wrong");
                return View();
            }

            await _signInManager.SignInAsync(user, loginvm.RememberMe);

         

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
