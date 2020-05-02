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

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet("{name}")]
        public IActionResult Info([FromRoute] string name)
        {
            var merchant = _merchantService.GetMerchantInfo(name);

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
                    }
                };
                return Ok(response);
            }

            return NotFound();
        }
    }
}