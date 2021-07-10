using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CourseTopicInputModel
    {
        [Required]
        public int Position { get; set; }
    }
}