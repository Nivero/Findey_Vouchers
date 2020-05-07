using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text.Json;
using FindeyVouchers.Interfaces;
using FindeyVouchers.Website.Models;
using Microsoft.AspNetCore.Authorization;

namespace FindeyVouchers.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public PaymentController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpPost]
        [Route("intent")]
        public IActionResult InitiatePaymentIntent([FromBody] JsonElement body)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_iZnEwjRXBzBdmTUdjLWDV8Xn00zsgY41iV";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var response = JsonSerializer.Deserialize<PaymentIntentRequest>(body.ToString(), options);
            var user = _merchantService.GetMerchantInfo(response.CompanyName);
            if (user == null)
            {
                return NotFound("Merchant not found");
            }

            var createOptions = new PaymentIntentCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "ideal",
                    "card"
                },
                Amount = response.Amount,
                Currency = "eur",
                ApplicationFeeAmount = 1,
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = user.StripeAccountId,
                },
            };
            var service = new PaymentIntentService();
            var intent = service.Create(createOptions);
            return Ok(new {client_secret = intent.ClientSecret});
        }
    }
}