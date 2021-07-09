using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class TagInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
    }
}