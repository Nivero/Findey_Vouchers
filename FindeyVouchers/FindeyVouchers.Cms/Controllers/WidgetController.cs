using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindeyVouchers.Cms.Controllers
{
    [Authorize]
    public class WidgetController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}