using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using AzureBlobStorageManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageManagement.Services
{
    public class FileManagerLogic : IFileManagerLogic
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FileManagerLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(FileModel model)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("files");

            var blobClient = blobContainer.GetBlobClient(model.File.FileName);

            await blobClient.UploadAsync(model.File.OpenReadStream());
        }

        public async Task Rename(string fileName, string newName)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("files");

            try
            {
                BlobClient sourceBlob = container.GetBlobClient(fileName);


                // Ensure that the source blob exists.
                if (await sourceBlob.ExistsAsync())
                {
                    // Lease the source blob for the copy operation
                    // to prevent another client from modifying it.
                    BlobLeaseClient lease = sourceBlob.GetBlobLeaseClient();
                    // Specifying -1 for the lease interval creates an infinite lease.
                    await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                    // Get the source blob's properties and display the lease state.
                    BlobProperties sourceProperties = await sourceBlob.GetPropertiesAsync();
                    Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");

                    // Get a BlobClient representing the destination blob with a unique name.
                    BlobClient destBlob = container.GetBlobClient(newName);

                    // Start the copy operation.
                    await destBlob.StartCopyFromUriAsync(sourceBlob.Uri);

                    // Update the source blob's properties.
                    sourceProperties = await sourceBlob.GetPropertiesAsync();

                    if (sourceProperties.LeaseState == LeaseState.Leased)
                    {
                        // Break the lease on the source blob.
                        await lease.BreakAsync();

                        // Update the source blob's properties to check the lease state.
                        sourceProperties = await sourceBlob.GetPropertiesAsync();
                        Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");
                    }
                }

                await sourceBlob.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);

            }
            catch (Exception ex)
            {

            }

        }

        public async Task RenameCreateNewBlobThenDeleteOld(string fileName, string newName)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient("files");
            BlobClient sourceBlob = container.GetBlobClient(fileName);
            var target = container.GetBlobClient(newName);

            var copyStatus =  target.StartCopyFromUri(sourceBlob.Uri);
         
            while (!copyStatus.HasCompleted)
                await Task.Delay(100);

            await sourceBlob.DeleteAsync();

        }
    }
}
