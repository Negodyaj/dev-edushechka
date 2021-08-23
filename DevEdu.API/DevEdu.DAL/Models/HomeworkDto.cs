using System;

namespace DevEdu.DAL.Models
{
    public class HomeworkDto
    {
        public int Id { get; set; }
        public TaskDto Task { get; set; }
        public GroupDto Group { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}