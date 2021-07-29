using DevEdu.API.Common;
using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicInputModel
    {
        [Required(ErrorMessage = ValidationMessage.PositionRequired)]
        public int Position { get; set; }
    }
}