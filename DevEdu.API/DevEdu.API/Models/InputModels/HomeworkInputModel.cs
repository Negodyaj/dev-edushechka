using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class HomeworkInputModel
    {
        [Required(ErrorMessage = StartDateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatDate)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatDate)]
        public string EndDate { get; set; }
    }
}