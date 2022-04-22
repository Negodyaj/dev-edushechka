using DevEdu.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models
{
    public class UserUpdateInfoOutPutModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public City City { get; set; }
        public string GitHubAccount { get; set; }
        [Url]
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }

    }
}