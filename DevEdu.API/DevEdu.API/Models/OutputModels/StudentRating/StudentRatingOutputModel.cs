namespace DevEdu.API.Models
{
    public class StudentRatingOutputModel
    {
        public int Id { get; set; }
        public UserInfoOutPutModel User { get; set; }
        public GroupInfoOutputModel Group { get; set; }
        public RatingTypeOutputModel RatingType { get; set; }
        public int Rating { get; set; }
        public int ReportingPeriodNumber { get; set; }
    }
}