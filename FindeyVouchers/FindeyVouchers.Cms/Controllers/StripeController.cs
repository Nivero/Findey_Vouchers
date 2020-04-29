using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Stripe;

namespace FindeyVouchers.Cms.Controllers
{
    public class StripeController : Controller
    {
        private readonly StripeClient client;

        public StripeController(
        )
        {
            // Set your secret key: remember to switch to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            this.client = new StripeClient("sk_test_iZnEwjRXBzBdmTUdjLWDV8Xn00zsgY41iV");
        }

        [HttpGet("/connect/oauth")]
        public IActionResult HandleOAuthRedirect(
            [FromQuery] string state,
            [FromQuery] string code
        )
        {
            var service = new OAuthTokenService(client);

            // Assert the state matches the state you provided in the OAuth link (optional).
            if (!StateMatches(state))
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    Json(new {Error = String.Format("Incorrect state parameter: {0}", state)})
                );
            }

            // Send the authorization code to Stripe's API.
            var options = new OAuthTokenCreateOptions
            {
                GrantType = "authorization_code",
                Code = code,
            };

            OAuthToken response = null;

            try
            {
                response = service.Create(options);
            }
            catch (StripeException e)
            {
                if (e.StripeError != null && e.StripeError.Error == "invalid_grant")
                {
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        Json(new {Error = String.Format("Invalid authorization code: {0}", code)})
                    );
                }
                else
                {
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        Json(new {Error = "An unknown error occurred."})
                    );
                }
            }

            var connectedAccountId = response.StripeUserId;
            SaveAccountId(connectedAccountId);

            // Render some HTML or redirect to a different page.
            return new OkObjectResult(Json(new {Success = true}));
        }

        private bool StateMatches(string stateParameter)
        {
            // Load the same state value that you randomly generated for your OAuth link.
            var savedState = "{{ STATE }}";

            return savedState == stateParameter;
        }

        private void SaveAccountId(string id)
        {
            // Save the connected account ID from the response to your database.
            Log.Information($"Connected account ID: {id}");
        }
    }
}