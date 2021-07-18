using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class GroupTaskInputModel
    {
        [Required(ErrorMessage = StartDateRequired)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        public DateTime EndDate { get; set; }
    }
}