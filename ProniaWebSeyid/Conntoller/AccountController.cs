using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.ViewModels.AppUserViewModels;
using System.Threading.Tasks;

namespace ProniaWebSeyid.Conntoller
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existingUser is { })
            {
                ModelState.AddModelError("UserName", "This username is already exist");
                return View(vm);
            }
            existingUser = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (existingUser is { })
            {
                ModelState.AddModelError(nameof(vm.EmailAddress), "This email is already exist");
                return View(vm);
            }
            AppUser appUser = new()
            {
                FullName = vm.FirstName + " " + vm.LastName,
                UserName = vm.UserName,
                Email = vm.EmailAddress
            };
            var result = await _userManager.CreateAsync(appUser, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }

            await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (user is null)
            {
                ModelState.AddModelError("", "Email ya da password  Sehdi");
                return View(vm);
            }
            var loginResult = await _userManager.CheckPasswordAsync(user, vm.Password);
            if (!loginResult)
            {
                ModelState.AddModelError("", "Email ya da password  Sehdi");
                return View(vm);
            }
            await _signInManager.SignInAsync(user, vm.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
