using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FindeyVouchers.Cms.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required] [EmailAddress] public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new {area = "Identity", code},
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    GetPasswordForgetEmailContent(user.CompanyName, code));

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        private string GetPasswordForgetEmailContent(string username, string url)
        {
            return $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\"width=\"600\">\n    <tr>\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je jouw wachtwoord wilde resetten. Klik op onderstaande knop om je wachtwoord opnieuw te installeren:\n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">\n                <a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\">\n                    <div class=\"cta\" style=\"width: 200px;padding: 1em;background: #2a3bf5;border-radius: 25px;margin: 0 auto;\">\n                        Reset wachtwoord\n                    </div>\n                </a>\n            </div>\n\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Mocht je het wachtwoord resetten per ongeluk hebben aangevraagd, dan mag je de e-mail negeren. Indien je de aanvraag niet zelf hebt ingediend of ondersteuning nodig heb, contacteer ons dan.</div>\n        </td>\n    </tr>\n    <tr>\n        <td style=\"text-align: center\" >\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n            <div>Hulp nodig?</div>\n            <div>Stuur ons een e-mail via</div>\n            <div>info@findey.co</div>\n          </div>\n        </td>\n    </tr>\n</table>\n\n</html>";
        }
    }
}