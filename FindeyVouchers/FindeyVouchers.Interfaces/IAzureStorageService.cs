using System.Threading.Tasks;

namespace FindeyVouchers.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType);
        void DeleteBlobData(string fileUrl);
    }
}