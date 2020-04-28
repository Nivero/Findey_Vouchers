using System;
using System.Drawing;
using System.Text;
using FindeyVouchers.Interfaces;
using QRCoder;

namespace FindeyVouchers.Services
{
    public class VoucherService : IVoucherService
    {
        public string GenerateVoucherCode(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
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
    }
}