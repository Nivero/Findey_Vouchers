using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface IMerchantService
    {
        ApplicationUser GetMerchantInfo(string merchantName);
    }
}