using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WeatherForecastEtain.Core.Interfaces;
using WeatherForecastEtain.MVC.Models;

namespace WeatherForecastEtain.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel LoginDetails, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await accountService.Login(LoginDetails.Username, LoginDetails.Password);
                
                if(user == null) {
                    ModelState.AddModelError("", "Error logging in, please try again!");
                    return View(); 
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));

                foreach (var role in user.Roles.Split(",")) //would use something betteer than a string array
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}