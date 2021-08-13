using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class CourseData
    {
        public static CourseDto GetUpdCourseDto()
        {
            return new CourseDto
            {
                Id = 1,
                Name = "C#",
                Description = "api",
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