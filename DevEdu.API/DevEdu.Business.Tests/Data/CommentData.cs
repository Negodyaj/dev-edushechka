using System;
using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class CommentData
    {
        public const int ExpectedCommentId = 42;
        public const int CommentId = 1;
        public const int UserId = 1;

        public static CommentDto GetCommentDto()
        {
            return new CommentDto
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
                Date = DateTime.Parse("19.07.2021"),
                IsDeleted = false
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
                    Date = DateTime.Parse("19.07.2021"),
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
                    Date = DateTime.Parse("22.02.2021"),
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
                    Date = DateTime.Parse("30.03.2021"),
                    IsDeleted = false
                }
            };
        }
    }
}