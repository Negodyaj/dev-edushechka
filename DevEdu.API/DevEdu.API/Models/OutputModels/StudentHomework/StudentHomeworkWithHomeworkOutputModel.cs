namespace DevEdu.API.Models.OutputModels
{
    public class StudentHomeworkWithHomeworkOutputModel : StudentHomeworkOutputModel
    {
        public HomeworkInfoFullOutputModel Homework { get; set; }
    }
}