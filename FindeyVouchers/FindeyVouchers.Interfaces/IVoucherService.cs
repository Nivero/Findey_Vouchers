using System;
using System.Drawing;
using System.Linq;
using FindeyVouchers.Domain.EfModels;

namespace FindeyVouchers.Interfaces
{
    public interface IVoucherService
    {
        string GenerateVoucherCode(int length);
        Bitmap GenerateQrCodeFromString(string text);
        IQueryable<MerchantVoucher> RetrieveMerchantVouchers(string companyName);

        bool UpdatePrice(Guid id, decimal price);
    }
}