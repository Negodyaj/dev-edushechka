using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class GroupDto : BaseDto
    {
        public string Name { get; set; }
        public CourseDto Course { get; set; }
        public GroupStatus? GroupStatus { get; set; }
        public DateTime StartDate { get; set; }
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
        public List<UserDto> Users { get; set; }
    }
}