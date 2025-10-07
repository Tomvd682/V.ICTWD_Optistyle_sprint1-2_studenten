using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class BrillenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
