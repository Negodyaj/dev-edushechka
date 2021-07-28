using System;
using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests.TestDataHelpers
{
    class NotificationData
    {
        public const int ExpectedNotificationId = 42;
        public const int NotificationId = 1;
        public const int UserId = 1;
        public const int GroupId = 1;

        public static NotificationDto GetCommentDto()
        {
            return new NotificationDto
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
