using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using SendGrid;

namespace FindeyVouchers.Interfaces
{
    public interface IMailService
    {
        Task<Response> SendMail(string receiverAddress, string subject, string body);
        public string GetPasswordForgetEmail(string username, string url);
        string GetVerificationEmail(string username, string url);
        string GetVoucherHtml(CustomerVoucher voucher, Bitmap bmp);
        string GetVoucherHtmlBody(string companyName, string htmlVouchers);
    }
}