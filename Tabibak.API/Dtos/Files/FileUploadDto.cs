namespace Tabibak.Api.Dtos.Files
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
    }
}
