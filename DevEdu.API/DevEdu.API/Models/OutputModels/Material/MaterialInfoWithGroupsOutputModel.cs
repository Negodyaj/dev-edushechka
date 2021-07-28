using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels
{
    public class MaterialInfoWithGroupsOutputModel : MaterialInfoOutputModel
    {
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}
