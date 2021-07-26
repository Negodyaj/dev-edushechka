using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
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
        public string Photo { get; set; }
        public string PhoneNumber { get; set; }

    }
}