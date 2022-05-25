using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class UserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ExileDate { get; set; }
        public City City { get; set; }
        public List<Role> Roles { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}