using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class LessonDto : BaseDto
    {
        public DateTime Date { get; set; }
        public String TeacherComment { get; set; }
        public UserDto Teacher { get; set; }
        public List<CommentDto> Comments { get; set; }
        public List<TopicDto> Topics { get; set; }
        public List<GroupDto> Groups { get; set; }
        public List<UserDto> Students { get; set; }
    }
}