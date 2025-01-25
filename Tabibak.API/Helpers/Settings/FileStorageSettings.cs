namespace Tabibak.Api.Helpers.Settings
{
    public class FileStorageSettings
    {
        public File Files { get; set; }
    }
    public class File
    {
        public Product Product { get; set; }
        public Category Ctegory { get; set; }
        public Default Default { get; set; }
    }
    public class Product
    {
        public string Path { get; set; }
    }
    public class Category
    {
        public string Path { get; set; }
    }
    public class Default
    {
        public string Path { get; set; }
    }
}
