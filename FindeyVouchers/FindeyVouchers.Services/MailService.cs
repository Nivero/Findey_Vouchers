using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private readonly string _imageUrl;

        public MailService(IConfiguration configuration)
        {
            var config = configuration;
            _apiKey = config.GetValue<string>("SendGridKey");
            _senderEmail = config.GetValue<string>("EmailSenderAddress");
            _senderName = config.GetValue<string>("EmailSenderName");
            _senderName = config.GetValue<string>("VoucherImageContainerUrl");
        }

        // public Task<Response> SendVoucherMail(List<CustomerVoucher> vouchers)
        // {
        //     var subject = $"Je vouchers van {vouchers.First().MerchantVoucher.Merchant.CompanyName}";
        //
        //     string content = GetVoucherEmail(vouchers);
        //     return SendMail(vouchers.First().Customer.Email, subject, content);
        // }


        public async Task<Response> SendMail(string receiverAddress, string subject, string body)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(receiverAddress);


            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public string GetPasswordForgetEmail(string username, string url)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\"width=\"600\"style=\"font-family: Open Sans\">\n    <tr>\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je jouw wachtwoord wilde resetten. Klik op onderstaande knop om je wachtwoord opnieuw te installeren:\n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">\n                <a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\">\n                    <div class=\"cta\" style=\"width: 200px;padding: 1em;background: #2a3bf5;border-radius: 25px;margin: 0 auto;\">\n                        Reset wachtwoord\n                    </div>\n                </a>\n            </div>\n\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Mocht je het wachtwoord resetten per ongeluk hebben aangevraagd, dan mag je de e-mail negeren. Indien je de aanvraag niet zelf hebt ingediend of ondersteuning nodig heb, contacteer ons dan.</div>\n        </td>\n    </tr>\n    <tr>\n         <td style=\"text-align: center;  border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" >\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n            <div>Hulp nodig?</div>\n            <div>Stuur ons een e-mail via</div>\n            <div>info@findey.nl</div>\n          </div>\n        </td>\n    </tr>\n</table>\n\n</html>";
        }

        public string GetVerificationEmail(string username, string url)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n</head>\n<table align=\"center\"width=\"600\" style=\"font-family: Open Sans\">\n    <tr>\n        <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n                <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n                <div>Vouchers</div>\n            </div>\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je een account wil registreren.\nVerifieer je e-mailadres hieronder. \n            </div>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">\n                <a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\">\n                    <div class=\"cta\" style=\"width: 200px;padding: 1em;background: #40e376;border-radius: 25px;margin: 0 auto;\">\n                        Verifieer je e-mailadres\n                    </div>\n                </a>\n            </div>\n\n        </td>\n    </tr>\n    <tr>\n        <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Na het verifiëren van je e-mailadres kan je beginnen\n met het aanbieden van je eigen unieke vouchers!</div>\n             <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Als deze e-mail niet is aangevraagd door jou,\ndan mag je de e-mail negeren.\n</div>\n        </td>\n    </tr>\n    <tr>\n        <td style=\"text-align: center;  border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" >\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div>\n            <div class=\"text-grey\" style=\"color: grey;\">\n            <div>Hulp nodig?</div>\n            <div>Stuur ons een e-mail via</div>\n            <div>info@findey.nl</div>\n          </div>\n        </td>\n    </tr>\n</table>\n\n</html>";
        }

        // TODO Fix params.
        // Also multiple vouchers.
        public string GetVoucherSoldNotificationEmail()
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n   <head>\n      <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n      <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n      <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n   </head>\n   <table align=\"center\" width=\"600\" style=\"font-family: Open Sans\">\n      <tr  style=\" font-weight: bold;\">\n         <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n               <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n               <div>Vouchers</div>\n            </div>\n         </td>\n      </tr>\n      <tr>\n         <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Beste, {{user.CompanyName}}</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">\n               Er zijn vouchers verkocht! Hieronder volgt een overzicht van de bestelling!\n            </div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">\n               De bestelling bevat:\n            </div>\n            <table width=\"600\" style=\"margin-bottom: 1em;\">\n               <tr style=\"display: inline-block; border-bottom: 1px solid grey; width:600px; padding-bottom: 10px;\">\n                  <td style=\"display:inline-block;\">\n                     <img src=\"https://findeystorage.blob.core.windows.net/vouchers-images/default-images/Black.png\" style=\"width: 200px; height: 150px;\">\n                  </td>\n                  <td style=\"display: inline-block; vertical-align: top; margin:5px;\">\n                     <div style=\"font-weight: bold\">{{vouchers.Name}}</div>\n                     <div style=\"font-weight: bold\">€ {{vouchers.Price}}</div>\n                     <div>Aantal: <span style=\"font-weight: bold\">€ {{amount}}</span> </div>\n                  </td>\n               </tr>\n            </table>\n         </td>\n      </tr>\n      <tr>\n         <td style=\"text-align: center; border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em;\">\n            <div class=\"font-bold mb-2\">Totale omzet: €{{totalAmount}}</div>\n            <div class=\"font-bold mb-2\">Aantal verkocht: {{totalCount}}</div>\n         </td>\n      </tr>\n      <tr>\n         <td style=\"text-align: center; margin-bottom: 2em;\">\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\"><span class=\"text-red\" style=\"color: #f54266;\">Findey</span> Vouchers</div>\n            <div class=\"text-grey\" style=\"color: grey; margin-bottom: 2em;\">\n               <div>Hulp nodig?</div>\n               <div>Stuur ons een e-mail via</div>\n               <div>info@findey.co</div>\n            </div>\n            <div style=\"font-weight: bold;\">Let op! U bent verantwoordelijk dat de vouchers verzilverd worden voor de juiste producten en/of diensten.</div>\n         </td>\n      </tr>\n   </table>\n</html>";
        }

        // TODO fix params.
        // Also mupltiple vouchers.
        public string GetVoucherHtml(CustomerVoucher voucher, Bitmap bmp)
        {
            StringBuilder htmlVoucher = new StringBuilder();

            ImageConverter converter = new ImageConverter();
            var bytes = (byte[]) converter.ConvertTo(bmp, typeof(byte[]));
            var qrcode = Convert.ToBase64String(bytes);
            var voucherImageUrl = _imageUrl + voucher.MerchantVoucher.Image;
            htmlVoucher.Append(
                $"<div style=\"align-content: center; justify-content: center; text-align: center; border-bottom: 1px solid grey;margin-bottom:\">\n                  <div> <img src=\"{voucherImageUrl}\" width=\"300\" style=\"display: block; margin: auto;\"> </div>\n                  <div style=\"margin: 1em;\"> {voucher.MerchantVoucher.Name} t.w.v. € {voucher.MerchantVoucher.Price} </div>\n                  <div> <img src=\"data:image/jpeg;base64,{qrcode}\" width=\"150\" style=\"display: block; margin: auto;\"> </div>\n               </div>");

            return htmlVoucher.ToString();
        }

        public string GetVoucherHtmlBody(string companyName, string htmlVouchers)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n   <head>\n      <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n      <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n      <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">\n   </head>\n   <table align=\"center\" width=\"600\" style=\" font-family: Open Sans\">\n      <tr style=\" font-weight: bold;\">\n         <td>\n            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">\n               <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>\n               <div>Vouchers</div>\n            </div>\n         </td>\n      </tr>\n      <tr>\n         <td>\n            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Gefeliciteerd met je vouchers(s)!</div>\n            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">               Je kan de vouchers(s) verzilveren door ze te laten scannen bij {companyName}!            </div>\n            <div>\n                  {htmlVouchers}\n            </div>\n         </td>\n      </tr>\n      <tr>\n         <td style=\"text-align: center; margin-bottom: 2em;\">\n            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\"><span class=\"text-red\" style=\"color: #f54266;\">Findey</span> Vouchers & {companyName}</div>\n            <div class=\"text-grey\" style=\"color: grey; margin-bottom: 2em;\">\n               <div>Hulp nodig?</div>\n               <div>Stuur ons een e-mail via</div>\n               <div>info@findey.co</div>\n            </div>\n         </td>\n      </tr>\n   </table>\n</html>";
        }
    }
}