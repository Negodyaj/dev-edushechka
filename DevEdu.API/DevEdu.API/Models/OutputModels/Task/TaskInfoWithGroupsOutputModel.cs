using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class TaskInfoWithGroupsOutputModel : TaskInfoOutputModel
    {
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}