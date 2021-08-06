using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class TaskData
    {
        public const int expectedTaskId = 55;

        public static TaskDto GetTaskDtoWithoutTags()
        {
            return new TaskDto { Name = "Task1", Description = "Description1", Links = "noLinks", IsRequired = true };
        }

        public static TaskDto GetTaskDtoWithTags()
        {
            return new TaskDto
            {
                Id = 1,
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto
                    {
                        Id = 13,
                        Name = "Tag",
                        IsDeleted = false
                    },
                    new TagDto
                    {
                        Id = 15,
                        Name = "DevEdu",
                        IsDeleted = false
                    },
                    new TagDto
                    {
                        Id = 14,
                        Name = "Tag"
                    }
                }
            };
        }

        public static TaskDto GetAnotherTaskDtoWithTags()
        {
            return new TaskDto
            {
                Id = 1,
                Name = "Task2",
                Description = "Description2",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 3},
                    new TagDto {Id = 4},
                    new TagDto {Id = 2}
                }
            };
        }


        public static List<CourseDto> GetListOfCourses()
        {
            return new List<CourseDto>
            {
                new CourseDto {Id = 1},
                new CourseDto {Id = 2},
                new CourseDto {Id = 3}
            };
        }

        public static List<StudentAnswerOnTaskDto> GetListOfStudentAnswers()
        {
            return new List<StudentAnswerOnTaskDto>
            {
                new StudentAnswerOnTaskDto {Id = 1},
                new StudentAnswerOnTaskDto {Id = 2},
                new StudentAnswerOnTaskDto {Id = 3}
            };
        }

        public static List<GroupDto> GetListOfGroups()
        {
            return new List<GroupDto>
            {
                new GroupDto {Id = 1},
                new GroupDto {Id = 2},
                new GroupDto {Id = 3}
            };
        }

        public static List<GroupDto> GetListOfSameGroups()
        {
            return new List<GroupDto>
            {
                new GroupDto {Id = 1},
                new GroupDto {Id = 2},
                new GroupDto {Id = 3}
            };
        }

        public static List<TaskDto> GetListOfTasks()
        {
            return new List<TaskDto>
            {
                new TaskDto
                {
                    Id = 1,
                    Name = "Task1",
                    Description = "Description1",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto
                        {
                            Id = 13,
                            Name = "Tag",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 15,
                            Name = "DevEdu",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 14,
                            Name = "Tag"
                        }
                    }
                },
                new TaskDto
                {
                     Id = 2,
                    Name = "Task2",
                    Description = "Description2",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto
                        {
                            Id = 18,
                            Name = "Tag",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 19,
                            Name = "DevEdu",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 20,
                            Name = "Tag"
                        }
                    }
                },
                new TaskDto
                {
                    Id = 3,
                    Name = "Task3",
                    Description = "Description3",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto
                        {
                            Id = 21,
                            Name = "Tag",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 22,
                            Name = "DevEdu",
                            IsDeleted = false
                        },
                        new TagDto
                        {
                            Id = 23,
                            Name = "Tag"
                        }
                    }
                }
            };
        }
    }
}