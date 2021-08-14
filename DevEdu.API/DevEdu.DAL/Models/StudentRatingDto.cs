namespace DevEdu.DAL.Models
{
    public class StudentRatingDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public GroupDto Group { get; set; }
        public RatingTypeDto RatingType { get; set; }
        public int Rating { get; set; }
        public int ReportingPeriodNumber { get; set; }
    }
}