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
        /// <summary> This method returns List CourseTopicDto, you can choose 1-3 version.</summary>
        public static List<CourseTopicDto> GetListCourseTopicDto(int vesionOfList)
        {
            List<CourseTopicDto> courseTopicsDto = new List<CourseTopicDto>();
            switch (vesionOfList)
            {
                case 1:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 5 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 6 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8 });
                    break;
                case 2:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3 });
                    break;
                case 3:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 1 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 6 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8 });
                    break;
                default:
                    courseTopicsDto.Add(new CourseTopicDto { Position = 5 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 3 });
                    courseTopicsDto.Add(new CourseTopicDto { Position = 8 });
                    break;
            }
            return courseTopicsDto;
        }
    }
}
