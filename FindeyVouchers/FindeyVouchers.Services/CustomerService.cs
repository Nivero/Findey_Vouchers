using System.Linq;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;

namespace FindeyVouchers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateCustomer(Customer customer)
        {
            var existing = _context.Customers.FirstOrDefault(x => x.Email.ToLower().Equals(customer.Email.ToLower()));
            if (existing != null)
            {
                _context.Customers.Add(customer);
            }

            {
                existing = customer;
                _context.Customers.Update(existing);
            }

            _context.SaveChanges();
        }
    }
}