using Microsoft.Extensions.Options;
using Tabibak.Api.Helpers.Enums;
using Tabibak.Api.Helpers.Settings;
using Tabibak.Context;
using Tabibak.Core.Models;

namespace Tabibak.Api.BLL.Files
{
    public class FileBLL : BaseBLL, IFileBLL
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly FileStorageSettings _fileSettigs;
        private readonly IWebHostEnvironment _environment;

        public FileBLL(ApplicationDbcontext dbcontext, IOptions<FileStorageSettings> fileSettings, IWebHostEnvironment environment)
        {
            _dbcontext = dbcontext;
            _fileSettigs = fileSettings.Value;
            _environment = environment;
        }

        public async Task<FileStorage> UploadFileAsync(IFormFile file, TableEnum table)
        {
            var filePath = GetFilePath(table);
            var path = Path.Combine(_environment.ContentRootPath, filePath);
            var contentType = file.ContentType.Split('/')[0];

            var extension = Path.GetExtension(file.FileName);
            var newFileName = SaveFile(file, path, extension);

            var filestorage = new FileStorage
            {
                ContentType = contentType,
                Extention = extension,
                Name = newFileName.Item1,
                Path = newFileName.Item2
            };
            await _dbcontext.FileStorages.AddAsync(filestorage);

            //await _dbcontext.SaveChangesAsync();

            return filestorage;
        }


        private string GetFilePath(TableEnum table)
        {
            return table switch
            {
                TableEnum.Product => _fileSettigs.Files.Product.Path,
                TableEnum.Category => _fileSettigs.Files.Ctegory.Path,
                _ => _fileSettigs.Files.Default.Path
            };
        }
        private (string, string) SaveFile(IFormFile file, string path, string extension)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString();
                Directory.CreateDirectory(path);
                var fullName = $"{fileName}{extension}";
                var filePath = Path.Combine(path, fullName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return (fileName, filePath);
            }
            catch
            {
                throw;
            }
        }


    }
}
