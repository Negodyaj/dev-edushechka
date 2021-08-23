namespace DevEdu.API.Models
{
    public class HomeworkInfoFullOutputModel : HomeworkInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
        public GroupOutputMiniModel Group { get; set; }
    }
}