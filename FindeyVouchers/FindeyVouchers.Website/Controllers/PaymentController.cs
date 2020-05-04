using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text.Json;

namespace FindeyVouchers.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        [Route("intent")]
        public IActionResult InitiatePaymentIntent([FromBody] object company)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_iZnEwjRXBzBdmTUdjLWDV8Xn00zsgY41iV";

            var service = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "ideal", 
                    "card"
                },
                Amount = 1000,
                Currency = "eur",
                ApplicationFeeAmount = 1,
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = "acct_1Gdc8VBSzed9ODwd",
                },
            };
            var intent = service.Create(createOptions);
            return Ok(new {client_secret = intent.ClientSecret});
        }
    }
}