using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FindeyVouchers.Services
{
    public class MailService : IMailService
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public MailService(IConfiguration configuration)
        {
            var config = configuration;
            _apiKey = config.GetValue<string>("SendGridKey");
            _senderEmail = config.GetValue<string>("EmailSenderAddress");
            _senderName = config.GetValue<string>("EmailSenderName");
        }

        public Task<Response> SendVoucherMail(Customer customer, ApplicationUser user, CustomerVoucher voucher)
        {
            var subject = $"Je voucher van {user.CompanyName}";

            string content = $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\" width=\"600\">\n    <tr  style=\" font-weight: bold;\">\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr style=\" font-weight: bold;\">\n        <td>\n            </div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\"><img src=\"https://findeystorage.blob.core.windows.net/voucher-images/20200501T115138921.png\" style=\"width: 150px; height: 150px;\"></div>\n\n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {customer.FirstName}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">\n                Gefeliciteerd met je voucher(s)! De voucher kan je verzilveren voor {voucher.VoucherMerchant.Name} (t.w.v. €{voucher.VoucherMerchant.Price}) door de barcode te laten scannen.\n            </div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\"><img src=\"https://findeystorage.blob.core.windows.net/voucher-images/20200501T115138921.png\" style=\"width: 150px; height: 150px;\"></div>\n\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td style=\" font-weight: bold;\">\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Veel plezier met je voucher en\ngraag tot snel! \n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td style=\"text-align: center; border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em;\">\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\"><span class=\"text-red\" style=\"color: #f54266;\">Findey</span> Vouchers & {user.CompanyName}</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n                <div>Hulp nodig?</div>\n                <div>Stuur ons een e-mail via</div>\n                <div>info@findey.co</div>\n            </div>\n        </td>\n    </tr>\n</table>\n\n</html>";

            return SendMail(customer.Email, subject, content);
        }


        private async Task<Response> SendMail(string receiverAddress, string subject, string body)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(receiverAddress);


            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            return await client.SendEmailAsync(msg);
        }
    }
}