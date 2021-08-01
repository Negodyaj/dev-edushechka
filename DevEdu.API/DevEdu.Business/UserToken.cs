using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.Business
{
    public class UserToken
    {
        public int UserId { get; set; }
        public List<Role> Roles { get; set; }
    }
}