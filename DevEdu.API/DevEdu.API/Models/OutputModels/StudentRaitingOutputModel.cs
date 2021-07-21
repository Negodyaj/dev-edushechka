

namespace DevEdu.API.Models.OutputModels
{
    public class StudentRaitingOutputModel
    {
        public int Id { get; set; }
        public UserInfoOutPutModel User { get; set; } 
        public int GroupId { get; set; } // change to GroupOutputModel
        public RaitingTypeOutputModel RaitingType { get; set; }
        public int Raiting { get; set; }
        public int ReportingPeriodNumber { get; set; }
    }
}
