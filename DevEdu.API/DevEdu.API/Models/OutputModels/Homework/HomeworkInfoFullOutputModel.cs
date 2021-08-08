namespace DevEdu.API.Models.OutputModels
{
    public class HomeworkInfoFullOutputModel : HomeworkInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
        public GroupOutputMiniModel Group { get; set; }
    }
}