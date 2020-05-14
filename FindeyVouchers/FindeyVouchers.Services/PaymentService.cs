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

        public void CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public void UpdatePayment(PaymentStatusResponse response)
        {
            var payment = _context.Payments.FirstOrDefault(x => x.Id == response.PaymentId);
            if (payment != null)
            {
                payment.Amount = response.Amount;
                payment.Status = response.PaymentStatus;
                payment.Error = response.ErrorMessage;
                payment.Created = response.Created;
                _context.Update(payment);
                _context.SaveChanges();
            }
        }
    }
}