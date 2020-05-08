using System;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface IPaymentService
    {
        Guid CreatePayment(Payment payment);
    }
}