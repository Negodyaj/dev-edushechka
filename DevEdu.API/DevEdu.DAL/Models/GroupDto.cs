using DevEdu.DAL.Enums;
using System;

namespace DevEdu.DAL.Models
{
    public class GroupDto : BaseDto
    {
        public CourseDto Course { get; set; }
        public GroupStatus GroupStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
    }
}