using Microsoft.AspNetCore.Mvc;

namespace ASK.Controllers
{
    public class CurrentValueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
