using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FindeyVouchers.Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WeatherForecast> InitiatePayment()
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_iZnEwjRXBzBdmTUdjLWDV8Xn00zsgY41iV";

            var service = new PaymentIntentService();
            var createOptions = new PaymentIntentCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                Amount = 2000,
                Currency = "eur",
                ApplicationFeeAmount = 123,
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = "{{CONNECTED_STRIPE_ACCOUNT_ID}}",
                },
            };
            service.Create(createOptions);
        }
    }
}