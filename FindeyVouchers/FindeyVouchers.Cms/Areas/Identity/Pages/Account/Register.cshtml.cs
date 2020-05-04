using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace FindeyVouchers.Cms.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, $"Findey Vouchers account voor {user.CompanyName}",
                        GetVerificationEmailContent(user.CompanyName, callbackUrl));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        
        private string GetVerificationEmailContent(string username, string url)
        {
            return $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\"width=\"600\">\n    <tr>\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je een account wil registreren.\nVerifieer je e-mailadres hieronder. \n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">\n                <a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\">\n                    <div class=\"cta\" style=\"width: 200px;padding: 1em;background: #40e376;border-radius: 25px;margin: 0 auto;\">\n                        Verifieer je e-mailadres\n                    </div>\n                </a>\n            </div>\n\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Na het verifiëren van je e-mailadres kan je beginnen\n met het aanbieden van je eigen unieke vouchers!</div>\n             <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Als deze e-mail niet is aangevraagd door jou,\ndan mag je de e-mail negeren.\n</div>\n        </td>\n    </tr>\n    <tr>\n        <td style=\"text-align: center;  border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" >\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n            <div>Hulp nodig?</div>\n            <div>Stuur ons een e-mail via</div>\n            <div>info@findey.co</div>\n          </div>\n        </td>\n    </tr>\n</table>\n\n</html>";
        }
    }
}
