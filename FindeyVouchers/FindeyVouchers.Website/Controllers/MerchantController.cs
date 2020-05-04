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
            var merchant = _merchantService.GetMerchantInfo(name);
            var vouchers = _voucherService.RetrieveMerchantVouchers(name);
            if (merchant != null)
            {
                var response = new VoucherPageResponse
                {
                    Merchant = new Merchant
                    {
                        Name = merchant.CompanyName,
                        Email = merchant.Email,
                        PhoneNumber = merchant.PhoneNumber,
                        Website = merchant.Website
                    },
                    Vouchers = vouchers
                    
                };
                return Ok(response);
            }

            return NotFound();
        }
    }
}