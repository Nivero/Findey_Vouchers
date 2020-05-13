using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace FindeyVouchers.Cms.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (string.IsNullOrWhiteSpace(user.CompanyName) || string.IsNullOrWhiteSpace(user.StripeAccountId))
                return RedirectToAction("Index", "Home");

            var applicationUser = await _context.Users.FindAsync(user.Id);
            if (applicationUser == null) return NotFound();

            return View(applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string id,
            ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id) return NotFound();

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
                    user.BusinessType = applicationUser.BusinessType;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Email = applicationUser.Email;
                    user.NormalizedEmail = applicationUser.Email.ToUpper();
                    user.Website = applicationUser.Website;
                    user.Description = applicationUser.Description;


                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);
        }

        public async Task<IActionResult> ResetPassword()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    await DoResetPassword(user);
                    ModelState.AddModelError(string.Empty, "Er is een email verstuurd.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(user.Id)) return NotFound();
                }

                return View(nameof(Index), user);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task DoResetPassword(ApplicationUser user)
        {
            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new {area = "Identity", code},
                Request.Scheme);

            var msg = _mailService.GetPasswordForgetEmail(user.CompanyName, callbackUrl);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Je wachtwoord opnieuw instellen",
                "test");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}