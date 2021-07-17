using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;

namespace DevEdu.API.Models.OutputModels
{
    public class GroupOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseDto Course { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public DateTime StartDate { get; set; }     // string
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
    }
}