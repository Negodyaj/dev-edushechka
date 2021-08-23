using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models
{
    public class CourseInputModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}