using DevEdu.DAL.Enums;
using System;

namespace DevEdu.DAL.Models
{
    public class StudentHomeworkDto
    {
        public int Id { get; set; }
        public HomeworkDto Homework { get; set; }
        public UserDto User { get; set; }
        public StudentHomeworkStatus StudentHomeworkStatus { get; set; }
        public string Answer { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}