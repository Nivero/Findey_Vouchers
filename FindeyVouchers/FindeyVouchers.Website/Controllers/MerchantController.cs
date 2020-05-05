using FindeyVouchers.Interfaces;
using FindeyVouchers.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindeyVouchers.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;
        private readonly IVoucherService _voucherService;

        public MerchantController(IMerchantService merchantService, IVoucherService voucherService)
        {
            _merchantService = merchantService;
            _voucherService = voucherService;
        }

        [HttpGet("{name}")]
        public IActionResult Info([FromRoute] string name)
        {
            var response = _voucherService.RetrieveMerchantVouchers(name);
            if (response != null)
            {
                return Ok(response);
            }

            return NotFound();
        }
    }
}