using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageManagement.Models
{
    public interface IFileManagerLogic
    {
        Task Upload(FileModel model);
        Task Rename(string fileName, string newName);
        Task RenameCreateNewBlobThenDeleteOld(string fileName, string newName);
    }
}
