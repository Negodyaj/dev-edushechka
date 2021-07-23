using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class TaskInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
        [Required(ErrorMessage = NameRequired)]
        public string Description { get; set; }
        [Required(ErrorMessage = DescriptionRequired)]
        public string Links { get; set; }
        [Required(ErrorMessage = IsRequiredErrorMessage)]
        public bool IsRequired { get; set; }
    }
}