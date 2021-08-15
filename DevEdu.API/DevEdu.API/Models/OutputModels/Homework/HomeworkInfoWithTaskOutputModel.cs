namespace DevEdu.API.Models
{
    public class HomeworkInfoWithTaskOutputModel : HomeworkInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
    }
}