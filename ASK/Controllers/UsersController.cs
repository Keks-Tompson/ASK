using ASK.BLL.Helper.Setting;
using ASK.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASK.Controllers
{
    public class UsersController : Controller
    {
        public static IList<UserViewModel> StaticUsers { get; } = new List<UserViewModel>
    {
        new UserViewModel()
        {
            UserName = "Simatek",
            UserPasword = "Simatek2020",
            UserRole = "Administrator"
        },
        new UserViewModel()
        {
            UserName = "Ecolog",
            UserPasword = "Ecolog2020",
            UserRole = "Ecolog"
        },
        new UserViewModel()
        {
            UserName = "Operator",
            UserPasword = "Operator2020",
            UserRole = "Operator"
        }
    };



        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToRoute(new { controller = "Settings", action = "Index" });
            else
                return View(new LoginViewModel());
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute(new { controller = "CurrentValue", action = "Index" });
        }


        [HttpPost]
        //[AllowAnonymous] - Allow anonymous users
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                //return RedirectToAction("Index", "Users");

                //ModelState.AddModelError("Username", "Некорректное название книги");

                return View(loginViewModel);
            }

            var user = AuthenticateUser(loginViewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError("Username", "Пользователь с таким именем не существует!");

                return View(loginViewModel);
            }

            if (user.UserPasword != loginViewModel.UserPassword)
            {
                ModelState.AddModelError("Username", "Неверный пароль!");

                return View(loginViewModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginViewModel.UserName),
                //new Claim("AnyTestValueIWantToAdd", "123124"),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToRoute(new { controller = "Settings", action = "Index" });
        }

        private UserViewModel? AuthenticateUser(string username)
        {
            return UsersController.StaticUsers.FirstOrDefault(model => model.UserName.Equals(username));
        }



    }
}
