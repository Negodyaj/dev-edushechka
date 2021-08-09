using DevEdu.DAL.Enums;
using System.Collections.Generic;

namespace DevEdu.API.Models
{
    public class UserIdentityInfo
    {
        public int UserId { get; set; }
        public List<Role> Roles { get; set; }
    }
}
