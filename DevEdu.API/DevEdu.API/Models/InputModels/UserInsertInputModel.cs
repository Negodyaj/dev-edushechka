using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class UserInsertInputModel
    {
        [Required(ErrorMessage = FirstNameRequired)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = EmailRequired)]
        [EmailAddress(ErrorMessage = WrongEmailFormat)]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ContractNumber { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string GitHubAccount { get; set; }
        [Url]
        public string Photo { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}