namespace DevEdu.API.Models.OutputModels
{
    public class GroupTaskInfoFullOutputModel : GroupTaskInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
        public GroupOutputMiniModel Group { get; set; }
    }
}