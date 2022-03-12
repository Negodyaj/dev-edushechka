using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class TaskData
    {
        public const int expectedTaskId = 55;
        public const int expectedTaskId2 = 42;

        public static TaskDto GetTaskDtoWithCourse()
        {
            return new TaskDto
            {
                Id = 1, Name = "Task1", Description = "Description1", Links = "noLinks", IsRequired = true,
                Courses = new List<CourseDto> { new() { Id = 1 } }
            };
        }

        public static TaskDto GetTaskDtoWithoutCourse()
        {
            return new TaskDto
            {
                Id = 1,
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true
            };
        }

        public static TaskDto GetTaskDto()
        {
            return new TaskDto
            {
                Id = 1,
                Name = "Task1",
                Description = "Description1",
                Links = "noLinks",
                IsRequired = true
            };
        }

        public static TaskDto GetAnotherTaskDto()
        {
            return new TaskDto
            {
                Id = 1,
                Name = "Task2",
                Description = "Description2",
                Links = "noLinks",
                IsRequired = true
            };
        }


        public static List<CourseDto> GetListOfCourses()
        {
            return new List<CourseDto>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };
        }

        public static List<StudentHomeworkDto> GetListOfStudentAnswers()
        {
            return new List<StudentHomeworkDto>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };
        }

        public static List<GroupDto> GetListOfGroups()
        {
            return new List<GroupDto>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };
        }

        public static List<GroupDto> GetListOfSameGroups()
        {
            return new List<GroupDto>
            {
                new() { Id = 1 },
                new() { Id = 2 },
                new() { Id = 3 }
            };
        }

        public static List<TaskDto> GetListOfTasks()
        {
            return new List<TaskDto>
            {
                new()
                {
                    Id = 1,
                    Name = "Task1",
                    Description = "Description1",
                    Links = "noLinks",
                    IsRequired = true
                },
                new()
                {
                    Id = 2,
                    Name = "Task2",
                    Description = "Description2",
                    Links = "noLinks",
                    IsRequired = true
                },
                new()
                {
                    Id = 3,
                    Name = "Task3",
                    Description = "Description3",
                    Links = "noLinks",
                    IsRequired = true
                }
            };
        }

        public static List<TaskDto> GetListOfTasksWithCourses()
        {
            return new List<TaskDto>
            {
                new()
                {
                    Id = 1,
                    Name = "Task1",
                    Description = "Description1",
                    Links = "noLinks",
                    IsRequired = true,
                    Courses = new List<CourseDto> { new() { Id = 1 } }
                },
                new()
                {
                    Id = 2,
                    Name = "Task2",
                    Description = "Description2",
                    Links = "noLinks",
                    IsRequired = true,
                    Courses = new List<CourseDto> { new() { Id = 2 } }
                },
                new()
                {
                    Id = 3,
                    Name = "Task3",
                    Description = "Description3",
                    Links = "noLinks",
                    IsRequired = true,
                    Courses = new List<CourseDto> { new() { Id = 3 } }
                }
            };
        }
    }
}