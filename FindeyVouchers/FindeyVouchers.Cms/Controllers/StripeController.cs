using System;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;

namespace FindeyVouchers.Cms.Controllers
{
    public class StripeController : Controller
    {
        private readonly StripeClient _client;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StripeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            // Set your secret key: remember to switch to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            this._client = new StripeClient("sk_test_iZnEwjRXBzBdmTUdjLWDV8Xn00zsgY41iV");
        }

        [HttpGet("/connect/oauth")]
        public async Task<IActionResult> HandleOAuthRedirect(
            [FromQuery] string state,
            [FromQuery] string code
        )
        {
            var service = new OAuthTokenService(_client);

            // Assert the state matches the state you provided in the OAuth link (optional).
            if (!StateMatches(state).Result)
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
            await SaveAccountId(connectedAccountId);

            // Render some HTML or redirect to a different page.
            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> StateMatches(string stateParameter)
        {
            // Load the same state value that you randomly generated for your OAuth link.
            var user = await _userManager.GetUserAsync(User);

            var savedState = _context.StripeSecret.FirstOrDefault(x => x.Email.Equals(user.Email));
            
            return savedState != null && savedState.Secret == stateParameter;
        }

        private async Task SaveAccountId(string id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var appUser = _context.Users.FirstOrDefault(x => x.Email.Equals(user.Email));
                if (appUser != null)
                {
                    appUser.StripeAccountId = id;
                    appUser.PhoneNumberConfirmed = true;
                }
                else
                {
                    Log.Error("Error retrieving app user: {0}", user.Id);
                }

                await _context.SaveChangesAsync();
                Log.Information($"Connected stripe account ID: {id}");
            }
            catch (Exception e)
            {
                Log.Error("Error storing stripe account Id: {0}", e);
            }
        }
    }
}