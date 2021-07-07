using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class TagInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}