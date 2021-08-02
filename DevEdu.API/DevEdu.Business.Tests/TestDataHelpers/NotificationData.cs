using System;
using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class NotificationData
    {
        public static NotificationDto GetNotificationDtoByRole()
        {
            return new NotificationDto
            {
                Id = 1,
                Text = "SimpleText",
                Role = DAL.Enums.Role.Student,
                Date = DateTime.Parse("11.11.2011"),
                IsDeleted = false
            };
        }
        public static NotificationDto GetNotificationDtoByUser()
        {
            return new NotificationDto
            {
                Id = 1,
                Text = "SimpleText",
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "SimlpeUserName",
                    LastName = "SimplaeLastName",
                    Email = "simple@simple.net",
                    Photo = "simple.jpg"
                },
                Date = DateTime.Parse("11.11.2011"),
                IsDeleted = false
            };
        }
        public static NotificationDto GetNotificationByGroupDto()
        {
            return new NotificationDto
            {
                Id = 1,
                Text = "SimpleText",
                Group = new GroupDto
                {
                    Id = 12,
                    Name = "SimpleGroupName",
                    StartDate = DateTime.Parse("01.01.1970")
                },
                Date = DateTime.Parse("11.11.2011"),
                IsDeleted = false
            };
        }
        public static List<NotificationDto> GetListNotificationByUserDto()
        {
            return new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText1",
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "SimlpeUserName1",
                        LastName = "SimplaeLastName1",
                        Email = "simple1@simple.net",
                        Photo = "simple1.jpg"
                    },
                    Date = DateTime.Parse("11.11.2011"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText2",
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "SimlpeUserName2",
                        LastName = "SimplaeLastName2",
                        Email = "simple2@simple.net",
                        Photo = "simple2.jpg"
                    },
                    Date = DateTime.Parse("12.12.2012"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText3",
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "SimlpeUserName3",
                        LastName = "SimplaeLastName3",
                        Email = "simple3@simple.net",
                        Photo = "simple3.jpg"
                    },
                    Date = DateTime.Parse("11.11.2013"),
                    IsDeleted = false
                }
            };
        }
        public static List<NotificationDto> GetListNotificationByGroupDto()
        {
            return new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText1",
                    Group = new GroupDto
                    {
                        Id = 12,
                        Name = "SimpleGroupName1",
                        StartDate = DateTime.Parse("01.01.1970")
                    },
                    Date = DateTime.Parse("11.11.2011"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText2",
                    Group = new GroupDto
                    {
                        Id = 12,
                        Name = "SimpleGroupName2",
                        StartDate = DateTime.Parse("02.02.1970")
                    },
                    Date = DateTime.Parse("12.12.2012"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText3",
                    Group = new GroupDto
                    {
                        Id = 12,
                        Name = "SimpleGroupName3",
                        StartDate = DateTime.Parse("03.03.1970")
                    },
                    Date = DateTime.Parse("11.11.2013"),
                    IsDeleted = false
                }
            };
        }
        public static List<NotificationDto> GetListNotificationByRoleDto()
        {
            return new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText1",
                    Role = DAL.Enums.Role.Student,
                    Date = DateTime.Parse("11.11.2011"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText2",
                    Role = DAL.Enums.Role.Student,
                    Date = DateTime.Parse("12.12.2012"),
                    IsDeleted = false
                },
                new NotificationDto
                {
                    Id = 1,
                    Text = "SimpleText3",
                    Role = DAL.Enums.Role.Student,
                    Date = DateTime.Parse("11.11.2013"),
                    IsDeleted = false
                }
            };
        }
    }
}
