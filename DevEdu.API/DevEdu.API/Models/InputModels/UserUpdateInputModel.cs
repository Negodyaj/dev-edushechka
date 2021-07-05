using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class UserUpdateInputModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int? CityId { get; set; }
        [Required]
        public string GitHubAccount { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        public string PhoneNumer { get; set; }
    }
}