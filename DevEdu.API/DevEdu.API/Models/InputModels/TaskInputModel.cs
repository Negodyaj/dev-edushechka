using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class TaskInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
        [Required(ErrorMessage = StartDateRequired)]
        public DateTime StartDate  { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        public DateTime EndDate  { get; set; }
        [Required(ErrorMessage = NameRequired)]
        public string Description { get; set; }
        [Required(ErrorMessage = DescriptionRequired)]
        public string Links { get; set; }
        [Required(ErrorMessage = IsRequiredErrorMessge)]
        public bool IsRequired { get; set; }
    }
}