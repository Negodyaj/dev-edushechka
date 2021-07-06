using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class MaterialInputModel
    {
        [Required]
        public string Content { get; set; }
    }
}