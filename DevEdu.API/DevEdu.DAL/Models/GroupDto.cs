using System;
using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Models
{
    public class GroupDto : BaseDto
    {
        public CourseDto Course { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public DateTime StartDate { get; set; }
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
    }
}