using DevEdu.DAL.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using static DevEdu.API.Common.ValidationMessage;

namespace DevEdu.API.Models.InputModels
{
    public class GroupInputModel
    {
        [Required(ErrorMessage = NameRequired)]
        public string Name { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required(ErrorMessage = GroupStatusIdRequired)]
        public GroupStatus GroupStatusId { get; set; }
        [Required(ErrorMessage = DateRequired)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = TimetableRequired)]
        public string Timetable { get; set; }
        [Required(ErrorMessage = PaymentPerMonthRequired)]
        public decimal PaymentPerMonth { get; set; }
    }
}