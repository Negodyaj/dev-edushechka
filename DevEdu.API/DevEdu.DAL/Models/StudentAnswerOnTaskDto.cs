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
        public DateTime? CompletedDate { get; set; }
        public List<CommentDto> Comments { get; set; }
        public bool IsDeleted { get; set; }


        public override bool Equals(object obj)
        {
            return obj is StudentAnswerOnTaskDto dto &&
                Id == dto.Id &&
                Task == dto.Task &&
                User == dto.User &&
                TaskStatus == dto.TaskStatus &&
                Answer == dto.Answer &&
                CompletedDate == dto.CompletedDate &&
                EqualityComparer<List<CommentDto>>.Default.Equals(Comments, dto.Comments) &&
                IsDeleted == dto.IsDeleted;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Task);
            hash.Add(User);
            hash.Add(TaskStatus);
            hash.Add(Answer);
            hash.Add(CompletedDate);
            hash.Add(Comments);
            hash.Add(IsDeleted);
            return hash.ToHashCode();
        }
    }
}