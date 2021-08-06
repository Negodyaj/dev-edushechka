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

        public static List<CourseDto> GetCoursesDtos()
        {
            return new List<CourseDto>
            {
                new CourseDto
                {
                    Id = 1,
                    Name = "Курс первый",
                    Description = "Лучший",
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 1}
                    },
                    Topics = new List<TopicDto>
                    {
                        new TopicDto {Id = 2},
                        new TopicDto {Id = 3},
                        new TopicDto {Id = 5}
                    },
                    Materials = new List<MaterialDto>
                    {
                        new MaterialDto {Id = 2},
                        new MaterialDto {Id = 3},
                        new MaterialDto {Id = 5}
                    },
                    Tasks = new List<TaskDto>
                    {
                        new TaskDto {Id = 2},
                        new TaskDto {Id = 3},
                        new TaskDto {Id = 5}
                    },
                    IsDeleted = false
                },
                new CourseDto
                {
                    Id = 2,
                    Name = "Курс первый",
                    Description = "Лучший",
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 2},
                        new GroupDto {Id = 3},
                        new GroupDto {Id = 5}
                    },
                    Topics = new List<TopicDto>
                    {
                        new TopicDto {Id = 2},
                        new TopicDto {Id = 3},
                        new TopicDto {Id = 5}
                    },
                    Materials = new List<MaterialDto>
                    {
                        new MaterialDto {Id = 2},
                        new MaterialDto {Id = 3},
                        new MaterialDto {Id = 5}
                    },
                    Tasks = new List<TaskDto>
                    {
                        new TaskDto {Id = 2},
                        new TaskDto {Id = 3},
                        new TaskDto {Id = 5}
                    },
                    IsDeleted = false
                },
                new CourseDto
                {
                    Id = 3,
                    Name = "Курс первый",
                    Description = "Лучший",
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 2},
                        new GroupDto {Id = 3},
                        new GroupDto {Id = 5}
                    },
                    Topics = new List<TopicDto>
                    {
                        new TopicDto {Id = 2},
                        new TopicDto {Id = 3},
                        new TopicDto {Id = 5}
                    },
                    Materials = new List<MaterialDto>
                    {
                        new MaterialDto {Id = 2},
                        new MaterialDto {Id = 3},
                        new MaterialDto {Id = 5}
                    },
                    Tasks = new List<TaskDto>
                    {
                        new TaskDto {Id = 2},
                        new TaskDto {Id = 3},
                        new TaskDto {Id = 5}
                    },
                    IsDeleted = false
                }
            };
        }
    }
}
