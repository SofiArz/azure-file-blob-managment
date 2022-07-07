using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStorageManagement.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileManagerLogic _fileManagerLogic;

        public FileController(IFileManagerLogic fileManagerLogic)
        {
            _fileManagerLogic = fileManagerLogic;
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileModel model)
        {
            if (model.File != null)
            {
                await _fileManagerLogic.Upload(model);
            }
            return Ok();
        }

        [Route("rename")]
        [HttpPost]
        public async Task<IActionResult> Rename([FromForm] string fileName, string newName)
        {
            if (fileName != String.Empty)
            {
                await _fileManagerLogic.Rename(fileName, newName);
            }
            return Ok();
        }

        [Route("createthendelete")]
        [HttpPost]
        public async Task<IActionResult> CreateThenDeleteForNameChange([FromForm] string fileName, string newName)
        {
            if (fileName != String.Empty)
            {
                await _fileManagerLogic.RenameCreateNewBlobThenDeleteOld(fileName, newName);
            }
            return Ok();
        }
    }
}
