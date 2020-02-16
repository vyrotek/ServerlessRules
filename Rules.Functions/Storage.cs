using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Functions
{
    public class Storage
    {
        private readonly CloudStorageAccount _storageAccount;
        
        public Storage()
        {
            var connString = Environment.GetEnvironmentVariable("ConnectionStrings:storage");
            
            if (string.IsNullOrWhiteSpace(connString))
                throw new ArgumentNullException("ConnectionStrings.Storage", "Config missing Azure Storage Connection String 'Storage'");

            if (!CloudStorageAccount.TryParse(connString, out CloudStorageAccount storageAccount))
                throw new ArgumentException("ConnectionStrings.Storage", "Invalid Azure Storage Connection String 'Storage'");

            _storageAccount = storageAccount;

        }

        public async Task<string> DownloadBlobTextAsync(string container, string blob)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(container);
            var blobReference = blobContainer.GetBlockBlobReference(blob);

            return await blobReference.DownloadTextAsync();
        }

        public async Task UploadBlobTextAsync(string container, string blob, string text)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(container);
            var blobReference = blobContainer.GetBlockBlobReference(blob);

            await blobReference.UploadTextAsync(text);
        }
    }
}
