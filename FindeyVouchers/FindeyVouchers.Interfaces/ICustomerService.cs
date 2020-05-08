using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface ICustomerService
    {
        void CreateCustomer(Customer customer);
    }
}