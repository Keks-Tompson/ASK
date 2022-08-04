using Microsoft.AspNetCore.Mvc;

namespace ASK.Controllers
{
    public class Charts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
