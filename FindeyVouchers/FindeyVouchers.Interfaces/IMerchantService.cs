using System.Collections.Generic;
using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface IMerchantService
    {
        ApplicationUser GetMerchantInfo(string merchantName);
        Task CreateAndSendMerchantNotification(List<CustomerVoucher> vouchers);
    }
}