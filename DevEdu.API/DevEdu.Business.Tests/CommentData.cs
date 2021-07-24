using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class CommentData
    {
        public static List<CommentDto> GetComments() 
        {
            return new List<CommentDto> {
                new CommentDto
                {
                    Id = 12,
                    User = new UserDto
                    {
                        Id = 46
                    },
                    Text = "Ok",
                    Date = new DateTime(2021, 4, 17, 0, 0, 0)
                },
                new CommentDto
                {
                    Id = 13,
                    User = new UserDto
                    {
                        Id = 54
                    },
                    Text = "Good",
                    Date = new DateTime(2021, 4, 17, 0, 0, 0)
                }
            };
        }

    }
}
