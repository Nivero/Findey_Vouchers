using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
    }
}