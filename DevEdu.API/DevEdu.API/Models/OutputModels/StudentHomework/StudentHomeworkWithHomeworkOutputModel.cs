namespace DevEdu.API.Models
{
    public class StudentHomeworkWithHomeworkOutputModel : StudentHomeworkOutputModel
    {
        public HomeworkInfoWithTaskOutputModel Homework { get; set; }
    }
}