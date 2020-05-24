using Microsoft.AspNetCore.Mvc;

namespace FindeyVouchers.Cms.Controllers
{
    public class WidgetController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}