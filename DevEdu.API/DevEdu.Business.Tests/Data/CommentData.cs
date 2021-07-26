using System;
using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class CommentData
    {
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
    }
}