using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class GroupFullOutputModel : GroupOutputModel
    {
        public List<UserInfoShortOutputModel> Students { get; set; }
        public List<UserInfoShortOutputModel> Teachers { get; set; }
        public List<UserInfoShortOutputModel> Tutors { get; set; }
        // todo: public List<MaterialOutputModel> Materials { get; set; }
    }
}