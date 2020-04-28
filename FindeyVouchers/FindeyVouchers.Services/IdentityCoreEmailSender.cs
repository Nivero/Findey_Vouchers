using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FindeyVouchers.Services
{
    public class IdentityCoreEmailSender : IEmailSender
    {
        public IdentityCoreEmailSender(IOptions<FindeyVouchers.Domain.SendGrid> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public FindeyVouchers.Domain.SendGrid Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@finde.co", Options.SendGridUser);
            var to = new EmailAddress(email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            return await client.SendEmailAsync(msg);

            // var client = new SendGridClient(apiKey);
            // var msg = new SendGridMessage()
            // {
            //     From = new EmailAddress("noreply@finde.co", Options.SendGridUser),
            //     Subject = subject,
            //     PlainTextContent = message,
            //     HtmlContent = message
            // };
            // var tmp = MailHelper.CreateSingleEmail(msg.From, new EmailAddress(email), subject, msg.PlainTextContent, msg.HtmlContent);
            // // msg.AddTo(new EmailAddress(email));
            //
            // return client.SendEmailAsync(tmp);
        }
    }
}