using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaWebSeyid.Abstraction;
using ProniaWebSeyid.ViewModels.AppUserViewModels;
using System.Threading.Tasks;

namespace ProniaWebSeyid.Conntoller
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager,IEmailService _emailService) : Controller
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


            await SendConfirmationEmailAsync(appUser);
            TempData["SuccessMessage"] = "Tesdiq olundu.Get Confirm ele maili!";

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            TempData["ErrorMessage"] = "Xetta";
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
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Confirm ele maili");
                await SendConfirmationEmailAsync(user);
                return View(vm);
            }
            await _signInManager.SignInAsync(user, vm.IsRemember);
            if(!string.IsNullOrWhiteSpace(vm.ReturnUrl))
            {
                return Redirect(vm.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        private async Task SendConfirmationEmailAsync(AppUser appUser)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            //string url = $"https://localhost:7224/Account/ConfirmEmail?token{token}&appUserId={appUser.Id}";
            string? url = Url.Action("ConfirmEmail", "Account", new { token, appUserId = appUser.Id }, Request.Scheme);
            string EmailBody = @$"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Email Confirmation</title>
    <style>
        body {{
            font-family: Arial, Helvetica, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #4f46e5;
            color: #ffffff;
            padding: 20px;
            text-align: center;
        }}
        .content {{
            padding: 30px;
            color: #333333;
            line-height: 1.6;
        }}
        .btn {{
            display: inline-block;
            margin-top: 20px;
            padding: 12px 24px;
            background-color: #4f46e5;
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 6px;
            font-weight: bold;
        }}
        .footer {{
            background-color: #f1f1f1;
            padding: 15px;
            text-align: center;
            font-size: 12px;
            color: #666666;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Email Confirmation</h2>
        </div>

        <div class=""content"">
            <p>Salam,{appUser.FullName}</p>

            <p>
                Hesabınızı aktivləşdirmək üçün aşağıdakı düyməyə klik edin:
            </p>

            <p style=""text-align: center;"">
                <a href=""{url}"" class=""btn"">
                    Confirm Email
                </a>
            </p>

            <p>
                Əgər bu əməliyyatı siz etməmisinizsə,{url}
            </p>

            <p>
                Hörmətlə,<br />
                <strong>Pronia</strong>
            </p>
        </div>

        <div class=""footer"">
            © 2026 Pronia. Bütün hüquqlar qorunur.
        </div>
    </div>
</body>
</html>
";
            await _emailService.SendEmailAsync(appUser.Email!, "Confirm your Email", EmailBody);
        }
        public async Task<IActionResult> ConfirmEmail(string token,string appUserId)
        {
            var user = await _userManager.FindByIdAsync(appUserId);
            if (user is null)
            {
                return NotFound();
            }
            var result =  _userManager.ConfirmEmailAsync(user, token);
            if (!result.Result.Succeeded)
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
    }
}
