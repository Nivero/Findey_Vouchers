using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FindeyVouchers.Domain;
using FindeyVouchers.Domain.EfModels;
using FindeyVouchers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QRCoder;
using Serilog;

namespace FindeyVouchers.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;
        private readonly IMerchantService _merchantService;

        public VoucherService(ApplicationDbContext context, IAzureStorageService azureStorageService,
            IConfiguration configuration, IMailService mailService, IMerchantService merchantService)
        {
            _context = context;
            _azureStorageService = azureStorageService;
            _configuration = configuration;
            _mailService = mailService;
            _merchantService = merchantService;
        }

        public string GenerateVoucherCode(int length)
        {
            var random = new Random();
            var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++) result.Append(characters[random.Next(characters.Length)]);

            return result.ToString();
        }

        public string GenerateQrCodeFromString(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            return qrCodeImageAsBase64;
        }

        public MerchantVoucherResponse RetrieveMerchantVouchers(string companyName)
        {
            var url = _configuration.GetValue<string>("VoucherImageContainerUrl");
            var merchant = _context.Users.FirstOrDefault(x => x.NormalizedCompanyName.Equals(companyName));
            if (merchant == null) return null;
            var response = new MerchantVoucherResponse
            {
                Merchant = new Merchant
                {
                    Name = merchant.CompanyName,
                    Email = merchant.Email,
                    PhoneNumber = merchant.PhoneNumber,
                    Website = merchant.Website,
                    Address = $"{merchant.Address}, {merchant.City}",
                    Description = merchant.Description
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
                var voucher = _context.CustomerVouchers.Include(x => x.MerchantVoucher)
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
            if (image != null) voucher.Image = await UploadFile(image);

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

        public void CreateCustomerVoucher(Customer customer, Voucher merchantVoucher, string responsePaymentId)
        {
            var customerVoucher = new CustomerVoucher
            {
                Customer = customer,
                MerchantVoucher = _context.MerchantVouchers.FirstOrDefault(x => x.Id == merchantVoucher.Id),
                PurchasedOn = DateTime.Now,
                Price = merchantVoucher.Price,
                Code = GenerateVoucherCode(12),
                EmailSent = false,
                Payment = _context.Payments.FirstOrDefault(x => x.Id == responsePaymentId)
            };
            _context.CustomerVouchers.Add(customerVoucher);
            _context.SaveChanges();
        }

        public async Task HandleFulfillment(string responsePaymentId)
        {
            var vouchers = _context.CustomerVouchers.Include(x => x.Customer)
                .Include(x => x.MerchantVoucher)
                .Include(x => x.MerchantVoucher.Merchant)
                .Where(x => x.Payment.Id == responsePaymentId).ToList();
            try
            {
                await CreateAndSendVouchers(vouchers);
                await _merchantService.CreateAndSendMerchantNotification(vouchers);
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
        }

        public async Task CreateAndSendVouchers(List<CustomerVoucher> vouchers)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var customerVoucher in vouchers)
                {
                    var emailVoucher =
                        _mailService.GetVoucherSoldHtml(customerVoucher,
                            GenerateQrCodeFromString(customerVoucher.Code));
                    sb.Append(emailVoucher);
                }

                var subject = $"Je vouchers van {vouchers.First().MerchantVoucher.Merchant.CompanyName}";
                var body = _mailService.GetVoucherSoldHtmlBody(vouchers.First().MerchantVoucher.Merchant.CompanyName,
                    sb.ToString());
                var response = await _mailService.SendMail(vouchers.First().Customer.Email, subject, body);
                if (response.StatusCode == HttpStatusCode.Accepted) SetEmailSend(vouchers, true);
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
        }

        public async Task DeactivateMerchantVoucher(Guid id)
        {
            var merchantVoucher = await _context.MerchantVouchers.FindAsync(id);
            merchantVoucher.IsActive = !merchantVoucher.IsActive;
            await _context.SaveChangesAsync();
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

        private void SetEmailSend(List<CustomerVoucher> vouchers, bool sent)
        {
            foreach (var voucher in vouchers)
            {
                var dbVoucher = _context.CustomerVouchers.FirstOrDefault(x => x.Id == voucher.Id);
                if (dbVoucher != null)
                {
                    dbVoucher.EmailSent = true;
                    _context.Update(dbVoucher);
                }
            }

            _context.SaveChangesAsync();
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