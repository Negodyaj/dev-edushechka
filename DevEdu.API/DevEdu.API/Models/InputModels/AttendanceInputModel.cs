using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class AttendanceInputModel
    {
        [Required]
         public string IsPresent { get; set; }

    }
}
