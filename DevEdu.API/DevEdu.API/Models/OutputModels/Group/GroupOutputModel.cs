using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.API.Models
{
    public class GroupOutputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseDto Course { get; set; }
        public GroupStatus GroupStatus { get; set; }
        public string StartDate { get; set; }
        public string Timetable { get; set; }
        public decimal PaymentPerMonth { get; set; }
    }
}