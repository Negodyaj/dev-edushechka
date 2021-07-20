using DevEdu.DAL.Enums;
using System;

namespace DevEdu.DAL.Models
{
    public class GroupDto : BaseDto
    {
        public string Name { get; set; }
        public CourseDto Course { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public DateTime StartDate { get; set; }
    }
}