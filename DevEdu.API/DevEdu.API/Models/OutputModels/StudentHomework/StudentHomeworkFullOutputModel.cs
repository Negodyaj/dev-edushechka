namespace DevEdu.API.Models.OutputModels
{
    public class StudentHomeworkFullOutputModel : StudentHomeworkOutputModel
    {
        public UserInfoShortOutputModel User { get; set; }
        public HomeworkInfoFullOutputModel Homework { get; set; }
    }
}