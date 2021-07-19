using System.ComponentModel.DataAnnotations;
using DevEdu.API.Common;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class GroupTaskInputModel
    {
        [Required(ErrorMessage = StartDateRequired)]
        [DateTimeToStringAttribute(ErrorMessage = WrongFormatDate)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        [DateTimeToStringAttribute(ErrorMessage = WrongFormatDate)]
        public string EndDate { get; set; }
    }
}