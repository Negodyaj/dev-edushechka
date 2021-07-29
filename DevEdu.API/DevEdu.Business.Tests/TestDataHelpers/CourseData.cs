using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class CourseData
    {
        public CourseData()
        {

        }
        public static List<CourseTopicDto> GetListCourseTopicDto()
        {
            List<CourseTopicDto> courseTopicsDto = new List<CourseTopicDto>();

            courseTopicsDto.Add(new CourseTopicDto { Position = 5, Id = 1, Topic = new TopicDto { Id = 1 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 6, Id = 2, Topic = new TopicDto { Id = 2 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 8, Id = 3, Topic = new TopicDto { Id = 3 } });

            return courseTopicsDto;
        }
        public static List<CourseTopicDto> GetListCourseTopicDtoFromDataBase()
        {
            List<CourseTopicDto> courseTopicsDto = new List<CourseTopicDto>();

            courseTopicsDto.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 2, Id = 21, Topic = new TopicDto { Id = 21 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 13, Topic = new TopicDto { Id = 13 } });

            return courseTopicsDto;
        }
    }
}
