using DevEdu.DAL.Enums;

namespace DevEdu.API.Models.OutputModels
{
    public class UserExistingFullInfoOutPutModel : UserInfoOutPutModel
    {
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string RegistrationDate { get; set; }
        public string ContractNumber { get; set; }
        public string BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        public string PhoneNumber { get; set; }
        public string ExileDate { get; set; }
        public City City { get; set; }
    }
}