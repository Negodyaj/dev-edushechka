namespace DevEdu.API.Models.OutputModels
{
    public class StudentHomeworkWithHomeworkOutputModel : StudentHomeworkOutputModel
    {
        public HomeworkInfoWithTaskOutputModel Homework { get; set; }
    }
}