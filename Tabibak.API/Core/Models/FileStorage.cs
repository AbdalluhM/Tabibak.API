namespace Tabibak.Core.Models
{
    public class FileStorage
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string Extention { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
