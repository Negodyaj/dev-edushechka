using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class GroupTaskData
    {
        public static UserIdentityInfo GetUserInfo()
        {
            return new UserIdentityInfo() { UserId = 1, Roles = new() {Role.Admin, Role.Manager } };
        }

        public static GroupTaskDto GetGroupTaskWithoutGroupAndTask()
        {
            return new GroupTaskDto
            {
                Id = 1,
                StartDate = DateTime.Parse("28.10.2020"),
                EndDate = DateTime.Parse("28.10.2021")
            };
        }

        public static GroupTaskDto GetGroupTaskWithGroupAndTask()
        {
            return new GroupTaskDto
            {
                Id = 1,
                Task = new TaskDto
                {
                    Id = 1,
                    Name = "task",
                    Description = "Description",
                    Links = "Links",
                    IsRequired = true,
                    IsDeleted = false
                },
                Group = new GroupDto
                {
                    Id = 1,
                    Name = "group",
                    GroupStatus = new GroupStatus(),
                    StartDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },
                StartDate = DateTime.Parse("28.10.2020"),
                EndDate = DateTime.Parse("28.10.2021")
            };
        }

        public static List<GroupTaskDto> GetListOfGroupTaskDtoWithTask()
        {
            return new List<GroupTaskDto>
            {
                new GroupTaskDto
                {
                    Id = 1,
                    Task = new TaskDto
                    {
                        Id = 1,
                        Name = "task1",
                        Description = "Description1",
                        Links = "Links1",
                        IsRequired = true,
                        IsDeleted = false
                    },
                    StartDate = DateTime.Parse("28.10.2020"),
                    EndDate = DateTime.Parse("28.10.2021")
                },
                new GroupTaskDto
                {
                    Id = 2,
                    Task = new TaskDto
                    {
                        Id = 2,
                        Name = "task2",
                        Description = "Description2",
                        Links = "Links2",
                        IsRequired = true,
                        IsDeleted = false
                    },
                    StartDate = DateTime.Parse("22.10.2020"),
                    EndDate = DateTime.Parse("22.10.2021")
                },
                new GroupTaskDto
                {
                    Id = 3,
                    Task = new TaskDto
                    {
                        Id = 3,
                        Name = "task3",
                        Description = "Description3",
                        Links = "Links3",
                        IsRequired = true,
                        IsDeleted = false
                    },
                    StartDate = DateTime.Parse("23.10.2020"),
                    EndDate = DateTime.Parse("23.10.2021")
                }
            };
        }

        public static List<GroupTaskDto> GetListOfGroupTaskDtoWithGroup()
        {
            return new List<GroupTaskDto>
            {
                new GroupTaskDto
                {
                    Id = 1,
                    Group = new GroupDto
                    {
                        Id = 1,
                        Name = "group1",
                        GroupStatus = new GroupStatus(),
                        StartDate=DateTime.Parse("01.01.2021"),
                        IsDeleted=false
                    },
                    StartDate = DateTime.Parse("28.10.2020"),
                    EndDate = DateTime.Parse("28.10.2021")
                },
                new GroupTaskDto
                {
                    Id = 2,
                    Group = new GroupDto
                    {
                        Id = 2,
                        Name = "group2",
                        GroupStatus = new GroupStatus(),
                        StartDate=DateTime.Parse("02.01.2021"),
                        IsDeleted=false
                    },
                    StartDate = DateTime.Parse("22.10.2020"),
                    EndDate = DateTime.Parse("22.10.2021")
                },
                new GroupTaskDto
                {
                    Id = 3,
                    Group = new GroupDto
                    {
                        Id = 3,
                        Name = "group3",
                        GroupStatus = new GroupStatus(),
                        StartDate=DateTime.Parse("03.01.2021"),
                        IsDeleted=false
                    },
                    StartDate = DateTime.Parse("23.10.2020"),
                    EndDate = DateTime.Parse("23.10.2021")
                }
            };
        }
    }
}