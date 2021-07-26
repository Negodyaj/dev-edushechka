using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevEdu.API.Common;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class GroupTaskInputModel
    {
        [Required(ErrorMessage = StartDateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatDate)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = EndDateRequired)]
        [CustomDateFormatAttribute(ErrorMessage = WrongFormatDate)]
        public string EndDate { get; set; }
        public List<int> GroupsIds { get; set; }
    }
}