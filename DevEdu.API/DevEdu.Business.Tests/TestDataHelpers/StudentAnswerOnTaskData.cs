using System;
using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskData
    {
        public const int ExpectedStudentAnswerId = 24;
        public const int UserId = 1;
        public const int AsnwerId = 1;
        public const int TaskId = 1;
        public const int CheckedStatus = 1;
        public const int AcceptedStatus = 2;
        public const int ReturnedStatus = 3;

        public static StudentAnswerOnTaskDto DtoForTaskIdAndUserId()
        {
            return new StudentAnswerOnTaskDto
            {
                Task = new TaskDto
                {
                    Id = 1
                },
                User = new UserDto
                {
                    Id = 1
                },
                Comments = new List<CommentDto>()
                ,
                TaskStatus = (DAL.Enums.TaskStatus)1,
                CompletedDate = default,
                IsDeleted = false
            };
        }

        public static StudentAnswerOnTaskDto GetTaskIdStudentIdAndStatusIdDto()
        {
            return new StudentAnswerOnTaskDto
            {
                Task = new TaskDto
                {
                    Id = 1,
                    Description= default,
                    IsRequired = false

                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },
                Comments = new List<CommentDto>(),
                TaskStatus = (DAL.Enums.TaskStatus)3,
                CompletedDate = default

            };
        }

        public static StudentAnswerOnTaskDto GetAnswerOfStudent()
        {
            return new StudentAnswerOnTaskDto 
            {
                User = new UserDto
                {
                    Id = 1
                },
                Answer = "I changed answer for first task. And now answer is good answer and I am User 1.",
            };
        }

        public static StudentAnswerOnTaskDto GetStudentAnswerOnTaskDto()
        {
            return new StudentAnswerOnTaskDto
            {
                Id = 1,
                Answer = "My answer for task is vot tak vot.",
                Task = new TaskDto
                {
                    Id = 1,
                    Description = "Taska horochaia.",
                    IsRequired = false
                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },

                Comments = new List<CommentDto>
                {
                    new CommentDto
                    {
                        Id = 1,
                        Text = "Vot - comment",
                            User = new UserDto
                            {
                                Id = 1,
                                FirstName = "Pol",
                                LastName = "Giggs",
                                Email = "polgigs@gmail.com",
                                Photo = "sigs.jpeg"
                            },
                        Date = DateTime.Parse("10.01.2021"),
                        IsDeleted = false
                    },
                    new CommentDto
                    {
                        Id = 2,
                        Text = "Tak - comment",
                        User = new UserDto
                        {
                            Id = 2,
                            FirstName = "Hustan",
                            LastName = "Beker",
                            Email = "hubeker@uandex.ru",
                            Photo = "beker.jpeg"
                        },
                        Date = DateTime.Parse("20.01.2021"),
                        IsDeleted = false
                    },
                    new CommentDto
                    {
                        Id = 3,
                        Text = "Vot2 - comment",
                        User = new UserDto
                        {
                            Id = 3,
                            FirstName = "Liker",
                            LastName = "Shots",
                            Email = "linkeshot@mail.ru",
                            Photo = "shot.jpeg"
                        },
                        Date = DateTime.Parse("30.01.2021"),
                        IsDeleted = false
                    }
                },

                TaskStatus = (DAL.Enums.TaskStatus)1,
                CompletedDate = DateTime.Parse("01.01.2021"),
                IsDeleted = false
            };
        }

        public static StudentAnswerOnTaskDto GetChangedStudentAnswerOnTaskDto()
        {
            return new StudentAnswerOnTaskDto
            {
                Id = 1,
                Answer = "I changed answer for first task. And now answer is good answer and I am User 1.",
                Task = new TaskDto
                {
                    Id = 1,
                    Description = "First taska.",
                    IsRequired = false
                },
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Peter",
                    LastName = "Petrov",
                    Email = "petr@mail.com",
                    Photo = "peter.jpeg"
                },
                Comments = new List<CommentDto>
                {
                    new CommentDto
                    {
                        Id = 1,
                        Text = "Vot - comment",
                            User = new UserDto
                            {
                                Id = 1,
                                FirstName = "Pol",
                                LastName = "Giggs",
                                Email = "polgigs@gmail.com",
                                Photo = "sigs.jpeg"
                            },
                        Date = DateTime.Parse("10.01.2021"),
                        IsDeleted = false
                    },
                    new CommentDto
                    {
                        Id = 2,
                        Text = "Tak - comment",
                        User = new UserDto
                        {
                            Id = 2,
                            FirstName = "Hustan",
                            LastName = "Beker",
                            Email = "hubeker@uandex.ru",
                            Photo = "beker.jpeg"
                        },
                        Date = DateTime.Parse("20.01.2021"),
                        IsDeleted = false
                    },
                    new CommentDto
                    {
                        Id = 3,
                        Text = "Vot2 - comment",
                        User = new UserDto
                        {
                            Id = 3,
                            FirstName = "Liker",
                            LastName = "Shots",
                            Email = "linkeshot@mail.ru",
                            Photo = "shot.jpeg"
                        },
                        Date = DateTime.Parse("30.01.2021"),
                        IsDeleted = false
                    }
                },

                TaskStatus = (DAL.Enums.TaskStatus)1,
                CompletedDate = DateTime.Parse("01.01.2021"),
                IsDeleted = false
            };
        }

        public static List<StudentAnswerOnTaskDto> GetListStudentAnswersOnTaskDto()
        {
            return new List<StudentAnswerOnTaskDto>
            {
                new StudentAnswerOnTaskDto
                {
                    Id = 1,
                    Answer = "My answer for first task is vot tak vot and I am User 1.",
                    Task = new TaskDto
                    {
                        Id = 1,
                        Description = "First taska.",
                        IsRequired = false
                    },
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Peter",
                        LastName = "Petrov",
                        Email = "petr@mail.com",
                        Photo = "peter.jpeg"
                    },
                    Comments = new List<CommentDto>(),
                    TaskStatus = (DAL.Enums.TaskStatus)1,
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },

                new StudentAnswerOnTaskDto
                {
                    Id = 2,
                    Answer = "I decide answer for first task and I have UserId 2",
                    Task = new TaskDto
                    {
                        Id = 1,
                        Description = "First taska.",
                        IsRequired = false
                    },
                    User = new UserDto
                    {
                        Id = 2,
                        FirstName = "Hustan",
                        LastName = "Beker",
                        Email = "hubeker@uandex.ru",
                        Photo = "beker.jpeg"
                    },
                    Comments = new List<CommentDto>(),
                    TaskStatus = (DAL.Enums.TaskStatus)1,
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },

                new StudentAnswerOnTaskDto
                {
                    Id = 3,
                    Answer = "I think much of more and write answer for first task and User 3",
                    Task = new TaskDto
                    {
                        Id = 1,
                        Description = "First taska.",
                        IsRequired = false
                    },
                    User = new UserDto
                    {
                        Id = 3,
                        FirstName = "Liker",
                        LastName = "Shots",
                        Email = "linkeshot@mail.ru",
                        Photo = "shot.jpeg"
                    },
                    Comments = new List<CommentDto>(),
                    TaskStatus = (DAL.Enums.TaskStatus)1,
                    CompletedDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },


                //new StudentAnswerOnTaskDto
                //{
                //    Id = 4,
                //    Answer = "I solved second taska vot tas vot. I have User 1",
                //    Task = new TaskDto
                //    {
                //        Id = 2,
                //        Description = "This second task.",
                //        IsRequired = false
                //    },
                //    User = new UserDto
                //    {
                //        Id = 1,
                //        FirstName = "Peter",
                //        LastName = "Petrov",
                //        Email = "petr@mail.com",
                //        Photo = "peter.jpeg"
                //    },
                //    TaskStatus = (DAL.Enums.TaskStatus)1,
                //    CompletedDate = DateTime.Parse("01.01.2021"),
                //    IsDeleted = false
                //}


            };
        }
    }
}
