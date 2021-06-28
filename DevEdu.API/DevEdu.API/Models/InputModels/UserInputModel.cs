using System;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class UserInputModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
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
        [Required]
        public string Photo { get; set; }
        [Required]
        public string PhoneNumer { get; set; }

    }
}
