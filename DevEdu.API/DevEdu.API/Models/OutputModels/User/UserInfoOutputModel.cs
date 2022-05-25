using DevEdu.DAL.Enums;
using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class UserInfoOutPutModel : UserInfoShortOutputModel
    {
        public List<Role> Roles { get; set; }
    }
}