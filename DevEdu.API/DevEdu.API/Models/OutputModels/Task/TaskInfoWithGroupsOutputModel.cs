using System.Collections.Generic;

namespace DevEdu.API.Models.OutputModels
{
    public class TaskInfoWithGroupsOutputModel : TaskInfoOutputModel
    {
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}