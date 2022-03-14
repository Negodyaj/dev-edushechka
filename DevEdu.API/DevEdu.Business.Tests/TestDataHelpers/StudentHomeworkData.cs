using System;
using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests.TestDataHelpers
{
    public class StudentAnswerOnTaskData
    {
        private static readonly DateTime _timeNow = DateTime.Now;

        public static StudentHomeworkDto DtoForTaskIdAndUserId()
        {
            return new StudentHomeworkDto
            {
                Homework = new HomeworkDto
                {
                    Task = new TaskDto
                    {
                        Id = 1
                    }
                },
                User = new UserDto
                {
                    Id = 1
                },
                CompletedDate = default,
                IsDeleted = false
            };
        }

        public static StudentHomeworkDto GetAnswerOfStudent()
        {
            return new StudentHomeworkDto
            {
                User = new UserDto
                {
                    Id = 1
                },
                Answer = "I changed answer for first task. And now answer is good answer and I am User 1.",
            };
        }

        public static StudentHomeworkDto GetStudentAnswerOnTaskDto()
        {
            return  new StudentHomeworkDto
            {
                Id = 1,
                Answer = "My answer for task is vot tak vot.",
                Homework = new HomeworkDto
                {
                    Task = new TaskDto
                    {
                        Id = 1
                    }
                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },
                StudentHomeworkStatus = StudentHomeworkStatus.OnCheck,
                CompletedDate = DateTime.Parse("01.01.2021"),
                IsDeleted = false
            };
        }

        public static StudentHomeworkDto GetStudentAnswerOnTaskWithAcceptedTaskStatusDto()
        {
            return new StudentHomeworkDto
            {
                Id = 1,
                Answer = "My answer for task is vot tak vot.",
                Homework = new HomeworkDto
                {
                    Task = new TaskDto
                    {
                        Id = 1
                    }
                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },
                StudentHomeworkStatus = StudentHomeworkStatus.OnCheck,
                CompletedDate = new DateTime(_timeNow.Year, _timeNow.Month, _timeNow.Day, _timeNow.Hour, _timeNow.Minute, _timeNow.Second),
                IsDeleted = false
            };
        }

        public static StudentHomeworkDto GetChangedStudentAnswerOnTaskDto()
        {
            return new StudentHomeworkDto
            {
                Id = 1,
                Answer = "I changed answer for first task. And now answer is good answer and I am User 1.",
                Homework = new HomeworkDto
                {
                    Task = new TaskDto
                    {
                        Id = 1
                    }
                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },
                CompletedDate = DateTime.Parse("01.01.2021"),
                IsDeleted = false
            };
        }

        public static List<StudentHomeworkDto> GetListStudentAnswersOnTaskDto()
        {
            return new List<StudentHomeworkDto>
            {
                new StudentHomeworkDto
                {
                    Id = 1,
                    Answer = "My answer for first task is vot tak vot and I am User 1.",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Peter",
                        LastName = "Petrov",
                        Email = "petr@mail.com",
                        Photo = "peter.jpeg"
                    },
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },

                new StudentHomeworkDto
                {
                    Id = 2,
                    Answer = "I decide answer for first task and I have UserId 2",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 2,
                        FirstName = "Hustan",
                        LastName = "Beker",
                        Email = "hubeker@uandex.ru",
                        Photo = "beker.jpeg"
                    },
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },

                new StudentHomeworkDto
                {
                    Id = 3,
                    Answer = "I think much of more and write answer for first task and User 3",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 3,
                        FirstName = "Liker",
                        LastName = "Shots",
                        Email = "linkeshot@mail.ru",
                        Photo = "shot.jpeg"
                    },
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },
            };
        }

        public static List<StudentHomeworkDto> GetAllAnswerOfStudent()
        {
            return new List<StudentHomeworkDto>()
            {
                new StudentHomeworkDto
                {
                    Id = 1,
                    Answer = "My answer for first task is vot tak vot and I am User 1.",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Peter",
                        LastName = "Petrov",
                        Email = "petr@mail.com",
                        Photo = "peter.jpeg"
                    },
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },

                new StudentHomeworkDto
                {
                    Id = 4,
                    Answer = "I am User 1, but task is Second.",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Peter",
                        LastName = "Petrov",
                        Email = "petr@mail.com",
                        Photo = "peter.jpeg"
                    },
                    CompletedDate = DateTime.Parse("02.01.2021"),
                    IsDeleted = false
                },

                new StudentHomeworkDto
                {
                    Id = 6,
                    Answer = "Answer for third task and I am User 1.",
                    Homework = new HomeworkDto
                    {
                        Task = new TaskDto
                        {
                            Id = 1
                        }
                    },
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Peter",
                        LastName = "Petrov",
                        Email = "petr@mail.com",
                        Photo = "peter.jpeg"
                    },
                    CompletedDate = DateTime.Parse("03.01.2021"),
                    IsDeleted = false
                },
            };
        }
    }
}