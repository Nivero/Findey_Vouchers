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

        public Task<Response> SendVoucherMail(Customer customer, ApplicationUser user)
        {
            var subject = $"Je voucher van {user.CompanyName}";

            return SendMail(customer.Email, subject, "test");
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