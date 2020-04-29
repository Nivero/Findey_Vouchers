using System.Drawing;

namespace FindeyVouchers.Interfaces
{
    public interface IVoucherService
    {
        string GenerateVoucherCode(int length);
        Bitmap GenerateQrCodeFromString(string text);
    }
}