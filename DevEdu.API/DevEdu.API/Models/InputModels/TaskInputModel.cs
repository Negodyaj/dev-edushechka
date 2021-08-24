using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class TaskInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
        [Required(ErrorMessage = DescriptionRequired)]
        public string Description { get; set; }
        public string Links { get; set; }
        [Required(ErrorMessage = IsRequiredErrorMessage)]
        public bool IsRequired { get; set; }
        public List<int> Tags { get; set; }
    }
}