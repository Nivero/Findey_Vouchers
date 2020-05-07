using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace FindeyVouchers.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public MerchantService(ApplicationDbContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public ApplicationUser GetMerchantInfo(string merchantName)
        {
            if (merchantName != null)
            {
                var merchant =
                    _context.Users.FirstOrDefault(x => x.NormalizedCompanyName.Equals(merchantName.ToLower()));
                return merchant ?? null;
            }

            return null;
        }
        public string GetPasswordForgetEmailContent(string username, string url)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\"width=\"600\">\n    <tr>\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je jouw wachtwoord wilde resetten. Klik op onderstaande knop om je wachtwoord opnieuw te installeren:\n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">\n                <a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\">\n                    <div class=\"cta\" style=\"width: 200px;padding: 1em;background: #2a3bf5;border-radius: 25px;margin: 0 auto;\">\n                        Reset wachtwoord\n                    </div>\n                </a>\n            </div>\n\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Mocht je het wachtwoord resetten per ongeluk hebben aangevraagd, dan mag je de e-mail negeren. Indien je de aanvraag niet zelf hebt ingediend of ondersteuning nodig heb, contacteer ons dan.</div>\n        </td>\n    </tr>\n    <tr>\n         <td style=\"text-align: center;  border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" >\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n            <div>Hulp nodig?</div>\n            <div>Stuur ons een e-mail via</div>\n            <div>info@findey.co</div>\n          </div>\n        </td>\n    </tr>\n</table>\n\n</html>";
        }
    }
}