using System.Linq;
using System.Xml;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;

namespace FindeyVouchers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Customer CreateCustomer(Customer customer)
        {
            var existing = _context.Customers.FirstOrDefault(x => x.Email.ToLower().Equals(customer.Email.ToLower()));
            if (existing != null)
            {
                existing.FirstName = customer.FirstName;
                existing.LastName = customer.LastName;
                existing.PhoneNumber = customer.PhoneNumber;
                _context.Customers.Update(existing);
            }
            else
            {
                _context.Customers.Add(customer);
            }
            _context.SaveChanges();
            return customer;
        }
    }
}