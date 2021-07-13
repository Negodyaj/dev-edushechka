using DevEdu.DAL.Enums;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class StudentAnswerOnTaskDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int StudentId { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public string Answer { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
