using System.Text.Json.Serialization;

namespace Tabibak.API.Dtos.Favourites
{
    public class FavoriteDoctorDto
    {
        [JsonIgnore]
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
    public class DoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }
    }

}
