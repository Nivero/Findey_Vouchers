using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FindeyVouchers.Website.Models;

namespace FindeyVouchers.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVoucherService _voucherService;

        public HomeController(ILogger<HomeController> logger, IVoucherService voucherService)
        {
            _logger = logger;
            _voucherService = voucherService;
        }

        public IActionResult Index()
        {
            var code = _voucherService.GenerateVoucherCode(12);
            var bitmap = _voucherService.GenerateQrCodeFromString(code);
            var image = (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[]));
            var model = new HomeViewModel
            {
                QrCode = image
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}