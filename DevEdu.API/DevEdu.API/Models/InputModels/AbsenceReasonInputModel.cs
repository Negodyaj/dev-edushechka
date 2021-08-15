using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class AbsenceReasonInputModel
    {
        [Required(ErrorMessage = AbsenceReasonRequired)]
        public string AbsenceReason { get; set; }
    }
}