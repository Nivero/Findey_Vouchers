using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindeyVouchers.Cms.Models;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FindeyVouchers.Cms.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoucherService _voucherService;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IConfiguration configuration, IVoucherService voucherService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var secret = _voucherService.GenerateVoucherCode(15);

            // Store value so we can later verify it
            if (_context.StripeSecret.Any(x => x.Email.Equals(user.Email)))
                _context.StripeSecret.Update(new StripeSecret
                {
                    Secret = secret,
                    Email = user.Email
                });
            else
                _context.StripeSecret.Add(new StripeSecret
                {
                    Secret = secret,
                    Email = user.Email
                });

            await _context.SaveChangesAsync();
            var model = new HomeViewModel
            {
                Email = user.Email.ToLower(),
                AccountComplete = !string.IsNullOrWhiteSpace(user.CompanyName),
                StripeComplete = !string.IsNullOrWhiteSpace(user.StripeAccountId)
            };
            if (!string.IsNullOrWhiteSpace(user.CompanyName)) model.StripeUrl = GenerateStripeUrl(user, secret);


            return View(model);
        }

        public async Task<IActionResult> OnBoarding()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var applicationUser = await _context.Users.FindAsync(user.Id);
            if (applicationUser == null) return NotFound();

            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnBoarding(string id,
            ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id) return NotFound();

            if (_context.Users.Any(x => x.CompanyName.Equals(applicationUser.CompanyName)))
                ModelState.AddModelError(string.Empty,
                    "Bedrijfsnaam is al in gebruik. Neem contact op met de beheerder.");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    user.FirstName = applicationUser.FirstName;
                    user.LastName = applicationUser.LastName;
                    user.DateOfBirth = applicationUser.DateOfBirth;
                    user.Address = applicationUser.Address;
                    user.ZipCode = applicationUser.ZipCode;
                    user.City = applicationUser.City;
                    user.CompanyName = applicationUser.CompanyName;
                    user.NormalizedCompanyName = applicationUser.CompanyName.Trim().ToLower();
                    user.BusinessType = applicationUser.BusinessType;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Email = applicationUser.Email;
                    user.Website = applicationUser.Website;
                    user.NormalizedEmail = applicationUser.Email.ToUpper();
                    user.Description = applicationUser.Description;

                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private void SetCookie(string key, string value, int? expireTime)
        {
            var option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(10);

            Response.Cookies.Append(key, value, option);
        }

        private string GenerateStripeUrl(ApplicationUser user, string secret)
        {
            var businessType = "";
            if (user.BusinessType == BusinessType.Corporation)
                businessType = "company";
            else
                businessType = "individual";

            var stripeUrl = new StringBuilder("https://connect.stripe.com/express/oauth/authorize");
            stripeUrl.Append($"?client_id={_configuration.GetValue<string>("StripeClientId")}");
            stripeUrl.Append($"&state={secret}");
            stripeUrl.Append("&suggested_capabilities[]=transfers");
            stripeUrl.Append("&suggested_capabilities[]=transfers");
            stripeUrl.Append($"&stripe_user[email]={user.Email}");
            stripeUrl.Append($"&stripe_user[url]={user.Website}");
            stripeUrl.Append($"&stripe_user[country]={user.Country}");
            stripeUrl.Append($"&stripe_user[phone_number]={user.PhoneNumber.Substring(1)}");
            stripeUrl.Append($"&stripe_user[business_name]={user.CompanyName}");
            stripeUrl.Append($"&stripe_user[business_type]={businessType}");
            stripeUrl.Append($"&stripe_user[first_name]={user.FirstName}");
            stripeUrl.Append($"&stripe_user[last_name]={user.LastName}");
            stripeUrl.Append($"&stripe_user[dob_day]={user.DateOfBirth.Day}");
            stripeUrl.Append($"&stripe_user[dob_month]={user.DateOfBirth.Month}");
            stripeUrl.Append($"&stripe_user[dob_year]={user.DateOfBirth.Year}");
            stripeUrl.Append($"&stripe_user[street_address]={user.Address}");
            stripeUrl.Append($"&stripe_user[city]={user.City}");
            stripeUrl.Append($"&stripe_user[zip]={user.ZipCode}");
            stripeUrl.Append("&stripe_user[currency]=EUR");
            return stripeUrl.ToString();
        }
    }
}