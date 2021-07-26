using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class CourseData
    {
        public CourseData()
        {

        }
        public static CourseDto GetCourseDto()
        {
            return new CourseDto
            {
                Id = 1,
                Name = "Web Api",
                Description = "Entity Framework",
                Groups = new List<GroupDto>
                {
                    new GroupDto
                    {
                        Id = 1,
                        Name = "Volosatiye Zmei",
                        PaymentPerMonth = 10000,
                        Timetable = "Morning"
                    },
                    new GroupDto
                    {
                        Id = 2,
                        Name = "Zhidkie Osnovi",
                        PaymentPerMonth = 20000,
                        Timetable = "Evening"
                    }
                },
                Materials = new List<MaterialDto>
                {
                    new MaterialDto
                    {
                        Id = 1, Content = "Code First"
                    },
                    new MaterialDto
                    {
                        Id = 2, Content =   "Entity First"
                    }
                },
                Tasks = new List<TaskDto>
                {
                    new TaskDto
                    {
                        Id = 1,
                        Name = "New super task"
                    },
                    new TaskDto
                    {
                        Id = 2,
                        Name = "Old super task"
                    }
                },
                Topics = new List<TopicDto>
                {
                    new TopicDto
                    {
                        Id = 1,
                        Name = "Scooter"
                    },
                    new TopicDto
                    {
                        Id = 2,
                        Name = "E-e-e-e-a"
                    }
                }
            };
        }

        public static List<CourseDto> GetListCourses()
        {
            return new List<CourseDto>
            {
                new CourseDto
                {
                    Id = 1,
                    Name = "Web Api",
                    Description = "Entity Framework",
                    Groups = new List<GroupDto>
                    {
                        new GroupDto
                        {
                            Id = 1,
                            Name = "Volosatiye Zmei",
                            PaymentPerMonth = 10000,
                            Timetable = "Morning"
                        },
                        new GroupDto
                        {
                            Id = 2,
                            Name = "Zhidkie Osnovi",
                            PaymentPerMonth = 20000,
                            Timetable = "Evening"
                        }
                    }
                },
                new CourseDto
                {
                    Id = 2,
                    Name = "Web Service",
                    Description = "SOAP",
                    Groups = new List<GroupDto>
                    {
                        new GroupDto
                        {
                            Id = 3,
                            Name = "Krasnie shari",
                            PaymentPerMonth = 30000,
                            Timetable = "Mittag"
                        },
                        new GroupDto
                        {
                            Id = 4,
                            Name = "Sinie priveti",
                            PaymentPerMonth = 40000,
                            Timetable = "MitNacht"
                        }
                    }
                }
            };
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
