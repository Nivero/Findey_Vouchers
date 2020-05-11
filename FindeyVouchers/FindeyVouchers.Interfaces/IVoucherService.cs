using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using Microsoft.AspNetCore.Http;

namespace FindeyVouchers.Interfaces
{
    public interface IVoucherService
    {
        string GenerateVoucherCode(int length);
        Bitmap GenerateQrCodeFromString(string text);
        MerchantVoucherResponse RetrieveMerchantVouchers(string companyName);
        void UpdatePrice(Guid id, decimal price);
        void InvalidateCustomerVoucher(Guid id);
        Task CreateMerchantVoucher(MerchantVoucher voucher, IFormFile image, ApplicationUser user);
        Task UpdateMerchantVoucher(MerchantVoucher merchantVoucher, IFormFile file);
        Task UpdateMerchantVoucher(MerchantVoucher merchantVoucher, DefaultImages file);
        Task DeactivateMerchantVoucher(Guid id);
        Task CreateMerchantVoucher(MerchantVoucher voucher, DefaultImages image, ApplicationUser user);
        List<VoucherCategory> GetCategories(ApplicationUser user);
        void CreateCustomerVoucher(Customer customer, Voucher merchantVoucher, string responsePaymentId);
        Task CreateAndSendVouchers(List<CustomerVoucher> vouchers);
        Task HandleFulfillment(string responsePaymentId);
    }
}