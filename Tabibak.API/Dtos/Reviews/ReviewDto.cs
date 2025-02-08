namespace Tabibak.API.Dtos.Reviews
{
    public class ReviewDto
    {
    }
    public class CreateReviewDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int Rating { get; set; }  // 1 to 5 stars
        public string Comments { get; set; }
    }

    public class UpdateReviewDto
    {
        public int Rating { get; set; }
        public string Comments { get; set; }
    }

    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ReviewResponsePagedDto : ReviewResponseDto
    {
        public double OverAllRating { get; set; }
    }

}
