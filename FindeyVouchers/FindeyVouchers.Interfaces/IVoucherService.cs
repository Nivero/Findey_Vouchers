using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Http;

namespace FindeyVouchers.Interfaces
{
    public interface IVoucherService
    {
        string GenerateVoucherCode(int length);
        Bitmap GenerateQrCodeFromString(string text);
        IQueryable<MerchantVoucher> RetrieveMerchantVouchers(string companyName);
        void UpdatePrice(Guid id, decimal price);
        void InvalidateCustomerVoucher(Guid id);
        Task CreateMerchantVoucher(MerchantVoucher voucher, IFormFile image, ApplicationUser user);
        Task UpdateMerchantVoucher(MerchantVoucher merchantVoucher, IFormFile file);
        Task DeactivateMerchantVoucher(Guid id);
    }
}