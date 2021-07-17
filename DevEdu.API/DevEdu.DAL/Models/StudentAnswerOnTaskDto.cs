using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class StudentAnswerOnTaskDto
    {
        public int Id { get; set; }
        public TaskDto Task { get; set; }
        public UserDto User { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string Answer { get; set; }
        public DateTime CompletedDate { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
