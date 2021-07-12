using System.ComponentModel.DataAnnotations;
using DevEdu.API.Common;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicInputModel
    {
        [Required(ErrorMessage = ValidationMessage.PositionRequired)]
        public int Position { get; set; }
    }
}