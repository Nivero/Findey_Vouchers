using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using QRCoder;
using Serilog;

namespace FindeyVouchers.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly ApplicationDbContext _context;

        public VoucherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateVoucherCode(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public Bitmap GenerateQrCodeFromString(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        public IQueryable<MerchantVoucher> RetrieveMerchantVouchers(string companyName)
        {
            return _context.MerchantVouchers.Where(x => x.Merchant.NormalizedCompanyName.Equals(companyName));
        }

        public bool UpdatePrice(Guid id, decimal price)
        {
            try
            {
                var voucher = _context.CustomerVouchers.FirstOrDefault(x => x.Id == id);
                if (voucher != null)
                {
                    voucher.Price = price;
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating price in voucher: {id}, {e}");
                return false;
            }
        }
    }
}