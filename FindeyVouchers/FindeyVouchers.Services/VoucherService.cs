using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QRCoder;
using Serilog;

namespace FindeyVouchers.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public VoucherService(ApplicationDbContext context, IAzureStorageService azureStorageService, IConfiguration configuration)
        {
            _context = context;
            _azureStorageService = azureStorageService;
            _configuration = configuration;
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

        public MerchantVoucherResponse RetrieveMerchantVouchers(string companyName)
        {
            string url = _configuration.GetValue<string>("VoucherImageContainerUrl");
            var merchant = _context.Users.FirstOrDefault(x => x.NormalizedCompanyName.Equals(companyName));
            if (merchant == null) return null;
            var response = new MerchantVoucherResponse
            {
                Merchant = new Merchant
                {
                    Name = merchant.CompanyName,
                    Email = merchant.Email,
                    PhoneNumber = merchant.PhoneNumber,
                    Website = merchant.Website
                },
                Vouchers = new List<Voucher>()
            };
            var vouchers = _context.MerchantVouchers.Include(x => x.Category).Where(x => x.Merchant == merchant);
            foreach (var item in vouchers)
            {
                var tmp = new Voucher
                {
                    Id = item.Id,
                    Description = item.Description,
                    Image = url + item.Image,
                    Category = new Category
                    {
                        Id = item.Category.Id,
                        Name = item.Category.Name
                    },
                    Price = item.Price,
                    Name = item.Name,
                    VoucherType = item.VoucherType
                };
                response.Vouchers.Add(tmp);
            }

            return response;
        }

        public void UpdatePrice(Guid id, decimal price)
        {
            try
            {
                var voucher = _context.CustomerVouchers.FirstOrDefault(x => x.Id == id);
                if (voucher != null)
                {
                    voucher.Price = price;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error updating price in voucher: {id}, {e}");
            }
        }

        public void InvalidateCustomerVoucher(Guid id)
        {
            try
            {
                var voucher = _context.CustomerVouchers.Include(x => x.VoucherMerchant)
                    .FirstOrDefault(x => x.Id == id);
                if (voucher != null)
                {
                    voucher.IsUsed = true;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error invalidating voucher with id: {id}, {e}");
            }
        }

        public async Task CreateMerchantVoucher(MerchantVoucher voucher, IFormFile image, ApplicationUser user)
        {
            voucher.Id = Guid.NewGuid();
            voucher.Merchant = user;

            // Upload file
            if (image != null)
            {
                voucher.Image = await UploadFile(image);
            }

            _context.Add(voucher);

            await _context.SaveChangesAsync();
        }

        public async Task CreateMerchantVoucher(MerchantVoucher voucher, DefaultImages image, ApplicationUser user)
        {
            voucher.Id = Guid.NewGuid();
            voucher.Merchant = user;
            voucher.Image = $"default-images/{GetImageNameFromEnum(image)}";
            _context.Add(voucher);

            await _context.SaveChangesAsync();
        }

        public List<VoucherCategory> GetCategories(ApplicationUser user)
        {
            return _context.VoucherCategories.Where(x => x.Merchant == user).ToList();
        }

        private string GetImageNameFromEnum(DefaultImages image)
        {
            var value = "";
            switch (image)
            {
                case DefaultImages.Black:
                {
                    value = "Black.png";
                    break;
                }
                case DefaultImages.Blue:
                {
                    value = "Blue.png";
                    break;
                }
                case DefaultImages.Bronze:
                {
                    value = "Bronze.png";
                    break;
                }
                case DefaultImages.White:
                {
                    value = "White.png";
                    break;
                }
                case DefaultImages.Yellow:
                {
                    value = "Yellow.png";
                    break;
                }
                case DefaultImages.Gold:
                {
                    value = "Gold.png";
                    break;
                }
                case DefaultImages.Green:
                {
                    value = "Green.png";
                    break;
                }
                case DefaultImages.Pink:
                {
                    value = "Pink.png";
                    break;
                }
                case DefaultImages.Silver:
                {
                    value = "Silver.png";
                    break;
                }
            }

            return value;
        }

        public async Task UpdateMerchantVoucher(MerchantVoucher voucher, IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    _azureStorageService.DeleteBlobData(voucher.Image);
                    voucher.Image = await UploadFile(image);
                }

                _context.Update(voucher);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error updateing merchant voucher with id: {voucher.Id}, {e}");
            }
        }

        public async Task UpdateMerchantVoucher(MerchantVoucher voucher, DefaultImages image)
        {
            try
            {
                voucher.Image = $"default-images/{GetImageNameFromEnum(image)}";

                _context.Update(voucher);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error updateing merchant voucher with id: {voucher.Id}, {e}");
            }
        }

        public async Task DeactivateMerchantVoucher(Guid id)
        {
            var merchantVoucher = await _context.MerchantVouchers.FindAsync(id);
            merchantVoucher.IsActive = !merchantVoucher.IsActive;
            await _context.SaveChangesAsync();
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            var result =
                await _azureStorageService.UploadFileToBlobAsync(file.FileName, bytes, file.ContentType);
            return result;
        }
    }
}