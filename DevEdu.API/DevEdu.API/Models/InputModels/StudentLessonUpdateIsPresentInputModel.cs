using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class StudentLessonUpdateIsPresentInputModel
    {
        [Required]
         public string IsPresent { get; set; }

    }
}
