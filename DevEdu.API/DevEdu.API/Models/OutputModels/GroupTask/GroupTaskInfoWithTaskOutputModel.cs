namespace DevEdu.API.Models
{
    public class GroupTaskInfoWithTaskOutputModel : GroupTaskInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
    }
}