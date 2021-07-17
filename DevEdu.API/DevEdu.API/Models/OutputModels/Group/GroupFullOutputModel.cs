using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Models.OutputModels.Group
{
    public class GroupFullOutputModel : GroupOutputModel
    {
        public List<UserInfoShortOutputModel> Students { get; set; }
        public List<UserInfoShortOutputModel> Teachers { get; set; }
        public List<UserInfoShortOutputModel> Tutors { get; set; }
        // todo: public List<MaterialOutputModel> Materials { get; set; }
    }
}
