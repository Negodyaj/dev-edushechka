namespace DevEdu.API.Models.OutputModels
{
    public class GroupTaskInfoWithTaskOutputModel : GroupTaskInfoOutputModel
    {
        public TaskInfoOutputMiniModel Task { get; set; }
    }
}