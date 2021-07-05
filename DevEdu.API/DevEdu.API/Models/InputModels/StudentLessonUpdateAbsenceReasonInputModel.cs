using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class StudentLessonUpdateAbsenceReasonInputModel
    {
        [Required]
        public string AbsenceReason { get; set; }
    }
}
