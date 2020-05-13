using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface IPaymentService
    {
        void CreatePayment(Payment payment);
        void UpdatePayment(PaymentStatusResponse response);
    }
}