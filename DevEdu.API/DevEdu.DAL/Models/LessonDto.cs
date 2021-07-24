using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public string TeacherComment { get; set; }
        public UserDto Teacher { get; set; }
        public string LinkToRecord { get; set; }
        public List<TopicDto> Topics { get; set; }
        public List<CommentDto> Comments { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<StudentLessonDto> Students { get; set; }
        public CourseDto Course { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LessonDto dto &&
                   Id == dto.Id &&
                   IsDeleted == dto.IsDeleted &&
                   Date == dto.Date &&
                   TeacherComment == dto.TeacherComment &&
                   EqualityComparer<UserDto>.Default.Equals(Teacher, dto.Teacher) &&
                   LinkToRecord == dto.LinkToRecord &&
                   EqualityComparer<List<TopicDto>>.Default.Equals(Topics, dto.Topics) &&
                   EqualityComparer<List<CommentDto>>.Default.Equals(Comments, dto.Comments) &&
                   EqualityComparer<List<GroupDto>>.Default.Equals(Groups, dto.Groups) &&
                   EqualityComparer<List<StudentLessonDto>>.Default.Equals(Students, dto.Students) &&
                   EqualityComparer<CourseDto>.Default.Equals(Course, dto.Course);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(IsDeleted);
            hash.Add(Date);
            hash.Add(TeacherComment);
            hash.Add(Teacher);
            hash.Add(LinkToRecord);
            hash.Add(Topics);
            hash.Add(Comments);
            hash.Add(Groups);
            hash.Add(Students);
            hash.Add(Course);
            return hash.ToHashCode();
        }
    }
}