using Microsoft.AspNetCore.Http;

namespace AzureBlobStorageManagement.Models
{
    public class FileModel
    {
        public IFormFile File { get; set; }
    }
}
