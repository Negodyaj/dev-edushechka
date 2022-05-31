using DevEdu.API.Common;
using DevEdu.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models
{
    public class GroupInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
        [Required]
        public int CourseId { get; set; }        
        public GroupStatus? GroupStatusId { get; set; }
        [Required(ErrorMessage = DateRequired)]
        [CustomDateFormat(ErrorMessage = WrongFormatStartDate)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = DateRequired)]
        [CustomDateFormat(ErrorMessage = WrongFormatStartDate)]
        public string EndDate { get; set; }
        [Required(ErrorMessage = TimetableRequired)]
        public string Timetable { get; set; }
        [Required(ErrorMessage = PaymentPerMonthRequired)]
        public decimal PaymentPerMonth { get; set; }
        [Range(minimum: 1, maximum: 20, ErrorMessage = WrongValueOfPaymentsCount)]
        public int PaymentsCount { get; set; }
    }
}