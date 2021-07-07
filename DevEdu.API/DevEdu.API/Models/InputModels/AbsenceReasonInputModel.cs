using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class AbsenceReasonInputModel
    {
        [Required]
        public string AbsenceReason { get; set; }
    }
}
