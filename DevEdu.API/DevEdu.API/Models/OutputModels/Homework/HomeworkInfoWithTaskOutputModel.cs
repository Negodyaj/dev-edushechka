namespace DevEdu.API.Models.OutputModels
{
    public class HomeworkInfoWithTaskOutputModel : HomeworkInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
    }
}