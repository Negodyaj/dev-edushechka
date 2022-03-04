using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class UserUpdateInputModel
    {
        [Required(ErrorMessage = IdRequired)]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatId)]
        public int Id { get; set; }

        [Required(ErrorMessage = FirstNameRequired)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequired)]
        public string LastName { get; set; }

        [Required(ErrorMessage = PatronymicRequired)]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = UsernameRequired)]
        public string Username { get; set; }

        [Required(ErrorMessage = CityRequired)]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatCityId)]
        public int? City { get; set; }

        [Required(ErrorMessage = GitHubAccountRequired)]
        public string GitHubAccount { get; set; }

        [Required(ErrorMessage = PhoneNumberRequired)]
        public string PhoneNumber { get; set; }
    }
}