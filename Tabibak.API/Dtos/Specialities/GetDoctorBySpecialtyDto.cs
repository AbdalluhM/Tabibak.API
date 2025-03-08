namespace Tabibak.API.Dtos.Specialities
{
    public class GetDoctorBySpecialtyDto
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }
        public decimal Fees { get; set; }
        public double Review { get; set; }
        public bool IsFavorite { get; set; }

    }
    public class DoctorDetailResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string ContactInfo { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }
        public double Review { get; set; }
        public bool HasReview { get; set; }
        public double PatienReview { get; set; }
        public TimeOnly WaitedTime { get; set; }
    }
}
