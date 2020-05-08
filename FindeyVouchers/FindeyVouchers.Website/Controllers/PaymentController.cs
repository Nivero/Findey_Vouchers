using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text.Json;
using FindeyVouchers.Domain.EfModels;
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
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        private readonly IVoucherService _voucherService;

        public PaymentController(IMerchantService merchantService, ICustomerService customerService,
            IPaymentService paymentService, IVoucherService voucherService)
        {
            _merchantService = merchantService;
            _customerService = customerService;
            _paymentService = paymentService;
            _voucherService = voucherService;
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

        [HttpPost]
        [Route("success")]
        public IActionResult FinishOrder([FromBody] JsonElement body)
        {
            // Save user
            // Wait for payment complete
            // Connect payment to user
            // Generate emails
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var response = JsonSerializer.Deserialize<PaymentSuccessRequest>(body.ToString(), options);
            var customer = _customerService.CreateCustomer(response.Customer);
            var paymentId = _paymentService.CreatePayment(new Payment
            {
                Amount = response.Amount,
                Status = response.PaymentStatus,
                StripeId = response.PaymentId,
                Created = new DateTime().AddSeconds(response.Created)
            });
            
            return Ok();
        }
    }
}