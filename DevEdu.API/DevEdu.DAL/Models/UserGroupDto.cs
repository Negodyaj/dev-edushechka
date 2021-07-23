using System;
using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Models
{
    public class UserGroupDto
    {
        public int Id { get; set; }
        public GroupDto Group { get; set; }
        public UserDto User { get; set; }
        public Role Role { get; set; }
    }
}