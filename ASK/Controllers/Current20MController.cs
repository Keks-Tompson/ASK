using Microsoft.AspNetCore.Mvc;

namespace ASK.Controllers
{
    public class Current20MController : Controller
    {
        public IActionResult CurrentTable20M()
        {
            return View();
        }
    }
}
