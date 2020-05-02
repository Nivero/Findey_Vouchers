using FindeyVouchers.Interfaces;
using FindeyVouchers.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace FindeyVouchers.Website.Controllers
{
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        public IActionResult Info([FromBody] object name)
        {
            var merchant = _merchantService.GetMerchantInfo((string)name);
            if (merchant != null)
            {
                return Ok(new MerchantResponse
                {
                    Name = merchant.CompanyName,
                    Email = merchant.Email,
                    PhoneNumber = merchant.PhoneNumber,
                    Website = "tent"
                });
            }
            else
            {
                return NotFound();
            }
        }
    }
}