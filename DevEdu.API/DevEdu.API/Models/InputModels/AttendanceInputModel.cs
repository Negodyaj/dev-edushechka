using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class AttendanceInputModel
    {
        [Required(ErrorMessage = AttendanceRequired)]
        public bool IsPresent { get; set; }
    }
}