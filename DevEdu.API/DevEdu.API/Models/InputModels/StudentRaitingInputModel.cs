using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class StudentRaitingInputModel
    {
        [Required(ErrorMessage = UserIdRequired)]
        public int UserId { get; set; }

        [Required(ErrorMessage = GroupIdRequired)]
        public int GroupId { get; set; }

        [Required(ErrorMessage = RaitingTypeIdRequired)]
        public int RaitingTypeId { get; set; }

        [Required(ErrorMessage = RaitingRequired)]
        public int Raiting { get; set; }

        [Required(ErrorMessage = ReportingPeriodNumberRequired)]
        public int ReportingPeriodNumber { get; set; }
    }
}
