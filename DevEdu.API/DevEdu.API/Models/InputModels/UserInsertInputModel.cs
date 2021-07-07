using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class UserInsertInputModel
    {
        [Required(ErrorMessage = FirstNameRequired)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequired)]
        public string LastName { get; set; }

        [Required(ErrorMessage = PatronymicRequired)]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = EmailRequired)]
        [EmailAddress(ErrorMessage = WrongFormatEmailFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = UsernameRequired)]
        public string Username { get; set; }

        [Required(ErrorMessage = PasswordRequired)]
        [MinLength(8, ErrorMessage = WrongFormatPasswordRequired)]
        public string Password { get; set; }

        [Required(ErrorMessage = ContractNumberRequired)]
        public string ContractNumber { get; set; }

        [Required(ErrorMessage = CityIdRequired)]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = WrongFormatCityIdRequired)]
        public int CityId { get; set; }

        [Required(ErrorMessage = BirthDateRequired)]
        public DateTime BirthDate { get; set; }

        public string GitHubAccount { get; set; }

        [Url(ErrorMessage = WrongFormatPhotoRequired)]
        public string Photo { get; set; }

        [Required(ErrorMessage = PhoneNumberRequired)]
        public string PhoneNumber { get; set; }
    }
}
