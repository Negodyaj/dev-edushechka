using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class UserInfoWithGroupsResponse : UserInfoShortOutputModel
    {
        public List<int> GroupIds { get; set; }
    }
}
