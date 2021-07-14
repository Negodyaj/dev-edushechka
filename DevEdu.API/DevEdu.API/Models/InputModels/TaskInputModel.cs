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
        [DateStringFormat(ErrorMessage = "StartDate must be in date format")]
        public string StartDate  { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        [DateStringFormat(ErrorMessage = "EndDate must be in date format")]
        public string EndDate  { get; set; }
        [Required(ErrorMessage = NameRequired)]
        public string Description { get; set; }
        [Required(ErrorMessage = DescriptionRequired)]
        public string Links { get; set; }
        [Required(ErrorMessage = IsRequiredErrorMessage)]
        public bool IsRequired { get; set; }
    }
}