using Tabibak.Api.Helpers.Enums;
using Tabibak.Core.Models;

namespace Tabibak.Api.BLL.Files
{
    public interface IFileBLL
    {
        Task<FileStorage> UploadFileAsync(IFormFile file, TableEnum table);
    }
}
