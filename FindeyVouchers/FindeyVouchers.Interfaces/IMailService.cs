using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using SendGrid;

namespace FindeyVouchers.Interfaces
{
    public interface IMailService
    {
        Task<Response> SendVoucherMail(Customer customer, ApplicationUser user);
    }
}