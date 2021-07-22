namespace DevEdu.API.Models.OutputModels
{
    public class StudentRaitingOutputModel
    {
        public int Id { get; set; }
        public UserInfoOutPutModel User { get; set; } 
        public int GroupId { get; set; } // change to GroupOutputModel
        public RaitingTypeOutputModel RatingType { get; set; }
        public int Rating { get; set; }
        public int ReportingPeriodNumber { get; set; }
    }
}