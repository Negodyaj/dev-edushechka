using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public class CourseData
    {
        public static CourseDto GetCourseDto()
        {
            return new CourseDto
            {
                Id = 1,
                Name = "Rock-hard Back",
                Description = "Back with rock-n-roll",
                IsDeleted = false
            };
        }

        public static List<CourseTopicDto> GetListCourseTopicDto()
        {
            List<CourseTopicDto> courseTopicsDto = new List<CourseTopicDto>();

            courseTopicsDto.Add(new CourseTopicDto { Position = 5, Id = 1, Topic = new TopicDto { Id = 1 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 6, Id = 2, Topic = new TopicDto { Id = 2 } });
            courseTopicsDto.Add(new CourseTopicDto { Position = 8, Id = 3, Topic = new TopicDto { Id = 54 } });

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
        public static List<TopicDto> GetTopics()
        {
            List<TopicDto> topicsDto = new List<TopicDto>();

            topicsDto.Add(new TopicDto { Id = 1 });
            topicsDto.Add(new TopicDto { Id = 2 });
            topicsDto.Add(new TopicDto { Id = 3 });
            topicsDto.Add(new TopicDto { Id = 4 });
            topicsDto.Add(new TopicDto { Id = 5 });
            topicsDto.Add(new TopicDto { Id = 6 });
            topicsDto.Add(new TopicDto { Id = 8 });
            topicsDto.Add(new TopicDto { Id = 9 });
            topicsDto.Add(new TopicDto { Id = 10 });
            topicsDto.Add(new TopicDto { Id = 54 });

            return topicsDto;
        }
        public static List<TopicDto> GetTopicsFromBDUseWhenTopicAbsent()
        {
            List<TopicDto> topicsDto = new List<TopicDto>();

            topicsDto.Add(new TopicDto { Id = 1 });
            topicsDto.Add(new TopicDto { Id = 2 });
            topicsDto.Add(new TopicDto { Id = 33 });

            return topicsDto;
        }

    }
}