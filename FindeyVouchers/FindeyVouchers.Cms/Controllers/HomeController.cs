using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FindeyVouchers.Cms.Models;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FindeyVouchers.Cms.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IVoucherService _voucherService;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IConfiguration configuration, IVoucherService voucherService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _voucherService = voucherService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var secret = _voucherService.GenerateVoucherCode(15);

            // Store value so we can later verify it
            if (_context.StripeSecret.Any(x => x.Email.Equals(user.Email)))
            {
                _context.StripeSecret.Update(new StripeSecret
                {
                    Secret = secret,
                    Email = user.Email
                });
            }
            else
            {
                _context.StripeSecret.Add(new StripeSecret
                {
                    Secret = secret,
                    Email = user.Email
                });
            }

            await _context.SaveChangesAsync();
            var model = new HomeViewModel
            {
                Email = user.Email.ToLower(),
                ClientId = _configuration.GetValue<string>("StripeClientId"),
                StateValue = secret,
                OnboardingComplete = !String.IsNullOrWhiteSpace(user.StripeAccountId)
            };

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        private string HashString(string secret)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: secret,
                salt: new byte[42],
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}