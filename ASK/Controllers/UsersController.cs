using ASK.BLL.Helper.Setting;
using Microsoft.AspNetCore.Mvc;

namespace ASK.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            if (Accaunt.isValid)
                return RedirectToRoute(new { controller = "Settings", action = "Index" });
            else
                return View();
        }

        

        public IActionResult Reboot()
        {
            Accaunt.isValid = false;
            return RedirectToRoute(new { controller = Accaunt.CurrentPage, action = "Index" });
        }



        [HttpPost]
        public IActionResult Index(string login, string password)
        {
            if (login == "Simatek" && password == "Simatek2020")
            {
                Accaunt.isValid = true;
                //return View("~/Views/CurrentValue/Index.cshtml");
                return RedirectToRoute(new { controller = "Settings", action = "Index" });
            }
            else
            {
                return View();
            }
        }



       



    }
}
