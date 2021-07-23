using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class CourseData
    {
        public CourseData()
        {

        }
        /// <summary> This method returns CourseTopicDto, you can choose 1-3 version.</summary>
        public static CourseTopicDto GetCourseTopicDto(int vesionOfDto)
        {
            CourseTopicDto courseTopicDto;
            switch (vesionOfDto)
            {
                case 1:
                    courseTopicDto = new CourseTopicDto { Position = 3 };
                    break;
                case 2:
                    courseTopicDto = new CourseTopicDto { Position = 5 };
                    break;
                case 3:
                    courseTopicDto = new CourseTopicDto { Position = 11 };
                    break;
                default:
                    courseTopicDto = new CourseTopicDto { Position = 1 };
                    break;
            }

            return courseTopicDto;
        }
        /// <summary> This method returns List CourseTopicDto, you can choose 1-7 version.</summary>
        public static List<CourseTopicDto> GetListCourseTopicDto(int vesionOfList)
        {
            List<CourseTopicDto> courseTopicsDto = new List<CourseTopicDto>();
            switch (vesionOfList)
            {
                case 1:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 5, Id = 1, Topic = new TopicDto { Id = 1 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 6, Id = 2, Topic = new TopicDto { Id = 2 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8, Id = 3, Topic = new TopicDto { Id = 3 } });
                    break;
                case 2:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 3, Topic = new TopicDto { Id = 3 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } });
                    break;
                case 3:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 8, Topic = new TopicDto { Id = 8 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 6, Id = 9, Topic = new TopicDto { Id = 9 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8, Id = 2, Topic = new TopicDto { Id = 2 } });
                    break;
                case 4:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 2, Id = 21, Topic = new TopicDto { Id = 21 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 13, Topic = new TopicDto { Id = 13 } });
                    break;
                case 5:
                    courseTopicsDto = new List<CourseTopicDto>();
                    break;

                case 6:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 4, Id = 21, Topic = new TopicDto { Id = 21 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 13, Topic = new TopicDto { Id = 13 } });
                    break;
                case 7:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3, Id = 21, Topic = new TopicDto { Id = 21 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1, Id = 15, Topic = new TopicDto { Id = 15 } });
                    break;
                default:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 5, Id = 7, Topic = new TopicDto { Id = 7 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3, Id = 11, Topic = new TopicDto { Id = 11 } });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8, Id = 12, Topic = new TopicDto { Id = 12 } });
                    break;
            }
            return courseTopicsDto;
        }
    }
}
