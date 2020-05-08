using System;
using System.Linq;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;

namespace FindeyVouchers.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Guid CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment.Id;
        }
    }
}