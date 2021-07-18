using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class TaskData
    {
        public const int expectedTaskId = 55;

        public static TaskDto GetTaskDtoWithoutTags()
        {
            return new TaskDto {Name = "Task1", Description = "Description1", Links = "noLinks", IsRequired = true};
        }

        public static TaskDto GetTaskDtoWithTags()
        {
            return new TaskDto
            {
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true,
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                }
            };
        }

        public static TaskDto GetAnotherTaskDtoWithTags()
        {
            return new TaskDto
            {
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

        public static List<StudentAnswerOnTaskForTaskDto> GetListOfStudentAnswers()
        {
            return new List<StudentAnswerOnTaskForTaskDto>
            {
                new StudentAnswerOnTaskForTaskDto {Id = 1},
                new StudentAnswerOnTaskForTaskDto {Id = 2},
                new StudentAnswerOnTaskForTaskDto {Id = 3}
            };
        }

        public static List<TaskDto> GetListOfTasks()
        {
            return new List<TaskDto>
            {
                new TaskDto
                {
                    Name = "Task1",
                    Description = "Description1",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 1},
                        new TagDto {Id = 2},
                        new TagDto {Id = 3}
                    }
                },
                new TaskDto
                {
                    Name = "Task2",
                    Description = "Description2",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 4},
                        new TagDto {Id = 5},
                        new TagDto {Id = 6}
                    }
                },
                new TaskDto
                {
                    Name = "Task3",
                    Description = "Description3",
                    Links = "noLinks",
                    IsRequired = true,
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 2},
                        new TagDto {Id = 4},
                        new TagDto {Id = 6}
                    }
                }
            };
        }
    }
}