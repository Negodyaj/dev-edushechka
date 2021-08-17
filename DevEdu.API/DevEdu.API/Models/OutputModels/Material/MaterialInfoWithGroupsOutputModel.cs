using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class MaterialInfoWithGroupsOutputModel : MaterialInfoOutputModel
    {
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}