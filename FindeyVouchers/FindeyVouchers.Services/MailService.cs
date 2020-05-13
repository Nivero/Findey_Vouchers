using System.IO;
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
        private readonly string _imageUrl;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public MailService(IConfiguration configuration)
        {
            var config = configuration;
            _apiKey = config.GetValue<string>("SendGridKey");
            _senderEmail = config.GetValue<string>("EmailSenderAddress");
            _senderName = config.GetValue<string>("EmailSenderName");
            _imageUrl = config.GetValue<string>("VoucherImageContainerUrl");
        }

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
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"></head><table align=\"center\"width=\"600\"style=\"font-family: Open Sans\"><tr><td><div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\"><div class=\"text-red\" style=\"color: #f54266;\">Findey</div><div>Vouchers</div></div></td></tr><tr><td><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div><div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je jouw wachtwoord wilde resetten. Klik op onderstaande knop om je wachtwoord opnieuw te installeren:</div><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\"><a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\"><div class=\"cta\" style=\"width: 200px;padding: 1em;background: #2a3bf5;border-radius: 25px;margin: 0 auto;\">Reset wachtwoord</div></a></div></td></tr><tr><td><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Mocht je het wachtwoord resetten per ongeluk hebben aangevraagd, dan mag je de e-mail negeren. Indien je de aanvraag niet zelf hebt ingediend of ondersteuning nodig heb, contacteer ons dan.</div></td></tr><tr> <td style=\"text-align: center;border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" ><div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div><div class=\"text-grey\" style=\"color: grey;\"><div>Hulp nodig?</div><div>Stuur ons een e-mail via</div><div>info@findey.nl</div></div></td></tr></table></html>";
        }

        public string GetVerificationEmail(string username, string url)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"></head><table align=\"center\"width=\"600\" style=\"font-family: Open Sans\"><tr><td><div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\"><div class=\"text-red\" style=\"color: #f54266;\">Findey</div><div>Vouchers</div></div></td></tr><tr><td><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Hallo, {username}</div><div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center\">We sturen je deze e-mail omdat je een account wil registreren.Verifieer je e-mailadres hieronder. </div><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\"><a class=\"cta-link\" href=\"{url}\" style=\"text-decoration: none;color: white;\"><div class=\"cta\" style=\"width: 200px;padding: 1em;background: #40e376;border-radius: 25px;margin: 0 auto;\">Verifieer je e-mailadres</div></a></div></td></tr><tr><td><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Na het verifiëren van je e-mailadres kan je beginnen met het aanbieden van je eigen unieke vouchers!</div> <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Als deze e-mail niet is aangevraagd door jou,dan mag je de e-mail negeren.</div></td></tr><tr><td style=\"text-align: center;border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; \" ><div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\">Findey Vouchers Team</div><div class=\"text-grey\" style=\"color: grey;\"><div>Hulp nodig?</div><div>Stuur ons een e-mail via</div><div>info@findey.nl</div></div></td></tr></table></html>";
        }

        public string GetVoucherSoldHtml(CustomerVoucher voucher, string b64Image)
        {
            var htmlVoucher = new StringBuilder();

            using (var ms = new MemoryStream())
            {
                var voucherImageUrl = _imageUrl + voucher.MerchantVoucher.Image;
                htmlVoucher.Append(
                    $"<div style=\"align-content: center; justify-content: center; text-align: center; border-bottom: 1px solid grey;margin-bottom:1em\"><div> <img src=\"{voucherImageUrl}\" width=\"300\" style=\"display: block; margin: auto;\"> </div><div style=\"margin: 1em;\"> {voucher.MerchantVoucher.Name} t.w.v. € {voucher.MerchantVoucher.Price} </div><div> <img src=\"data:image/jpeg;base64,{b64Image}\" width=\"150\" style=\"display: block; margin: auto;\"> </div> </div>");

                return htmlVoucher.ToString();
            }
        }

        public string GetVoucherSoldHtmlBody(string companyName, string htmlVouchers)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"> <head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\"> </head> <table align=\"center\" width=\"600\" style=\" font-family: Open Sans\"><tr style=\" font-weight: bold;\"> <td><div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\"> <div class=\"text-red\" style=\"color: #f54266;\">Findey</div> <div>Vouchers</div></div> </td></tr><tr> <td><div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Gefeliciteerd met je vouchers(s)!</div><div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\"> Je kan de vouchers(s) verzilveren door ze te laten scannen bij {companyName}!</div><div>{htmlVouchers}</div> </td></tr><tr> <td style=\"text-align: center; margin-bottom: 2em;\"><div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\"><span class=\"text-red\" style=\"color: #f54266;\">Findey</span> Vouchers & {companyName}</div><div class=\"text-grey\" style=\"color: grey; margin-bottom: 2em;\"> <div>Hulp nodig?</div> <div>Stuur ons een e-mail via</div> <div>info@findey.co</div></div> </td></tr> </table></html>";
        }

        public string GetVoucherNoticiationHtml(MerchantVoucher voucher, int count)
        {
            var htmlVoucher = new StringBuilder();
            var voucherImageUrl = _imageUrl + voucher.Image;
            htmlVoucher.Append(
                $"             <table width=\"600\" style=\"margin-bottom: 1em;\">               <tr style=\"display: inline-block; border-bottom: 1px solid grey; width:600px; padding-bottom: 10px;\">                  <td style=\"display:inline-block;\">                     <img src=\"{voucherImageUrl}\" style=\"width: 300px; height: 150px;\">                  </td>                  <td style=\"display: inline-block; vertical-align: top; margin:5px;\">                     <div style=\"font-weight: bold\">{voucher.Name}</div>                     <div style=\"font-weight: bold\">€ {voucher.Price}</div>                     <div>Aantal: <span style=\"font-weight: bold\"> {count}</span> </div>                  </td>               </tr>            </table>");
            return htmlVoucher.ToString();
        }

        public string GetVoucherNotificationHtmlBody(string companyName, string htmlVouchers, decimal totalAmount,
            int totalCount)
        {
            return
                $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">   <head>      <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">      <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">      <link href=\"https://fonts.googleapis.com/css?family=Open+Sans\" rel=\"stylesheet\">   </head>   <table align=\"center\" width=\"600\" style=\"font-family: Open Sans\">      <tr  style=\" font-weight: bold;\">         <td>            <div class=\"header mb-2 pb-2\" style=\"border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em; text-align: center; font-weight:bold;\">               <div class=\"text-red\" style=\"color: #f54266;\">Findey</div>               <div>Vouchers</div>            </div>         </td>      </tr>      <tr>         <td>            <div class=\"mb-2\" style=\"margin-bottom: 2em; text-align: center\">Beste, {companyName}</div>            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">               Er zijn vouchers verkocht! Hieronder volgt een overzicht van de bestelling!            </div>            <div class=\"text-center mb-2\" style=\"margin-bottom: 2em; text-align: center;\">               De bestelling bevat:            </div>            {htmlVouchers}        </td>      </tr>      <tr>         <td style=\"text-align: center; border-bottom: 1px solid grey;margin-bottom: 2em;padding-bottom: 2em;\">            <div class=\"font-bold mb-2\">Totale omzet: €{totalAmount}</div>            <div class=\"font-bold mb-2\">Aantal verkocht: {totalCount}</div>         </td>      </tr>      <tr>         <td style=\"text-align: center; margin-bottom: 2em;\">            <div class=\"font-bold mb-2\" style=\"margin-bottom: 2em;font-weight: bold;\"><span class=\"text-red\" style=\"color: #f54266;\">Findey</span> Vouchers</div>            <div class=\"text-grey\" style=\"color: grey; margin-bottom: 2em;\">               <div>Hulp nodig?</div>               <div>Stuur ons een e-mail via</div>               <div>info@findey.co</div>            </div>            <div style=\"font-weight: bold;\">Let op! U bent verantwoordelijk dat de vouchers verzilverd worden voor de juiste producten en/of diensten.</div>         </td>      </tr>   </table></html>";
        }
    }
}