namespace DevEdu.API.Models.OutputModels
{
    public class StudentHomeworkFullOutputModel : StudentHomeworkOutputModel
    {
        public HomeworkInfoFullOutputModel Homework { get; set; }
    }
}