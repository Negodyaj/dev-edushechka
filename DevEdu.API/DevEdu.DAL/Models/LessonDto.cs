using System;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.DAL.Models
{
    public class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public String TeacherComment { get; set; }
        public UserDto TeacherDto { get; set; }
        public List<CommentDto> CommentDtos { get; set; }
        public List<TopicDto> TopicDtos { get; set; }
        public List<GroupDto> GroupDtos { get; set; }
        public List<UserDto> StudentDtos { get; set; }

        public void Distinct()
        {
            CommentDtos = CommentDtos.Distinct().ToList();
            TopicDtos = TopicDtos.Distinct().ToList();
            GroupDtos = GroupDtos.Distinct().ToList();
            StudentDtos = StudentDtos.Distinct().ToList();
        }
    }
}