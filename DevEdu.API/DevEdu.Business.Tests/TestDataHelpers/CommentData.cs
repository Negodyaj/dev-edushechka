using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DevEdu.Business.Tests
{
    public static class CommentData
    {
        private const string _dateFormat = "dd.MM.yyyy";

        public static CommentDto GetCommentDto()
        {
            return new CommentDto
            {
                Id = 1,
                Lesson = new LessonDto
                {
                    Id = 1
                },
                Text = "comment1",
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Cat",
                    LastName = "Jack",
                    Email = "CatJack@meow.cat",
                    Photo = "Cat.jpg"
                },
                Date = DateTime.ParseExact("19.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                IsDeleted = false
            };
        }

        public static LessonDto GetLessonDto()
        {
            return new LessonDto
            {
                Id = 1
            };
        }

        public static StudentLessonDto GetStudentLessonDto()
        {
            return new StudentLessonDto
            {
                Id = 1
            };
        }

        public static StudentHomeworkDto GetStudentHomeworkDto()
        {
            return new StudentHomeworkDto
            {
                Id = 1,
                User = new UserDto
                {
                    Id = 10
                }
            };
        }

        public static List<GroupDto> GetGroupsDto()
        {
            return new List<GroupDto>
            {
                new GroupDto
                {
                    Id = 10
                },

                new GroupDto
                {
                    Id = 20
                }
            };
        }

        public static List<CommentDto> GetListCommentsDto()
        {
            return new List<CommentDto>
            {
                new CommentDto
                {
                    Id = 1,
                    Text = "comment1",
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Cat",
                        LastName = "Jack",
                        Email = "CatJack@meow.cat",
                        Photo = "Cat.jpg"
                    },
                    Date = DateTime.ParseExact("19.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                    IsDeleted = false
                },
                new CommentDto
                {
                    Id = 2,
                    Text = "comment2",
                    User = new UserDto
                    {
                        Id = 2,
                        FirstName = "Zloo",
                        LastName = "Evil",
                        Email = "ZlooEvil@dark.hell",
                        Photo = "Zloo.jpg"
                    },
                    Date = DateTime.ParseExact("22.02.2021", _dateFormat, CultureInfo.InvariantCulture),
                    IsDeleted = false
                },
                new CommentDto
                {
                    Id = 3,
                    Text = "comment3",
                    User = new UserDto
                    {
                        Id = 3,
                        FirstName = "Teacher",
                        LastName = "Anton",
                        Email = "AntonTeacher@back.sharp",
                        Photo = "Anton.jpg"
                    },
                    Date = DateTime.ParseExact("30.03.2021", _dateFormat, CultureInfo.InvariantCulture),
                    IsDeleted = false
                }
            };
        }
    }
}