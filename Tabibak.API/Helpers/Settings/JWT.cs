namespace Tabibak.Api.Helpers.Settings
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audiance { get; set; }
        public double ExpireOn { get; set; }
    }
}
