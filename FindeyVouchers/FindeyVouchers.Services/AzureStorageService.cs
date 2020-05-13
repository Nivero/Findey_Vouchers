using System;
using System.Threading.Tasks;
using FindeyVouchers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Serilog;

namespace FindeyVouchers.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly string _accessKey;
        private readonly IConfiguration _configuration;

        public AzureStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = configuration.GetConnectionString("DefaultStorageAccount");
        }

        public async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(_accessKey);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var strContainerName = _configuration.GetValue<string>("VoucherImageContainerName");
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                var fileName = GenerateFileName(strFileName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                        {PublicAccess = BlobContainerPublicAccessType.Blob});

                if (fileName != null && fileData != null)
                {
                    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return fileName;
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Error("An error occured uploading file {0}", ex);
                return null;
            }
        }

        public async void DeleteBlobData(string fileName)
        {
            try
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(_accessKey);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var strContainerName = _configuration.GetValue<string>("VoucherImageContainerName");
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                // get block blob refarence    
                var blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                // delete blob from container        
                await blockBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                Log.Error("An error occured uploading file {0}", ex);
            }
        }

        private string GenerateFileName(string fileName)
        {
            var strName = fileName.Split('.');
            var strFileName = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." +
                              strName[strName.Length - 1];
            return strFileName;
        }
    }
}