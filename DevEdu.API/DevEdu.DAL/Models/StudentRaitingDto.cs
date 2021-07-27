namespace DevEdu.DAL.Models
{
    public class StudentRaitingDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public GroupDto Group { get; set; }
        public RaitingTypeDto RaitingType { get; set; }
        public int Raiting { get; set; }
        public int ReportingPeriodNumber { get; set; }
    }
}
