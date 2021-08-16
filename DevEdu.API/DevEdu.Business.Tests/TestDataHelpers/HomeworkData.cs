using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DevEdu.Business.Tests
{
    public static class HomeworkData
    {
        private const string _dateFormat = "dd.MM.yyyy";

        public static HomeworkDto GetHomeworkDtoWithoutGroupAndTask()
        {
            return new HomeworkDto
            {
                Id = 1,
                StartDate = DateTime.ParseExact("28.10.2020", _dateFormat, CultureInfo.InvariantCulture),
            };
        }

        public static HomeworkDto GetHomeworkDtoWithGroupAndTask()
        {
            return new HomeworkDto
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
                    StartDate = DateTime.ParseExact("01.01.2021", _dateFormat, CultureInfo.InvariantCulture),
                    IsDeleted = false
                },
                StartDate = DateTime.ParseExact("28.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact("28.10.2021", _dateFormat, CultureInfo.InvariantCulture)
            };
        }

        public static List<HomeworkDto> GetListOfHomeworkDtoWithTask()
        {
            return new List<HomeworkDto>
            {
                new HomeworkDto
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
                    StartDate = DateTime.ParseExact("28.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("28.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                },
                new HomeworkDto
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
                    StartDate = DateTime.ParseExact("22.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("22.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                },
                new HomeworkDto
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
                    StartDate = DateTime.ParseExact("23.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("23.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                }
            };
        }

        public static List<HomeworkDto> GetListOfHomeworkDtoWithGroup()
        {
            return new List<HomeworkDto>
            {
                new HomeworkDto
                {
                    Id = 1,
                    Group = new GroupDto
                    {
                        Id = 1,
                        Name = "group1",
                        GroupStatus = new GroupStatus(),
                        StartDate = DateTime.ParseExact("01.01.2021", _dateFormat, CultureInfo.InvariantCulture),
                        IsDeleted=false
                    },
                    StartDate = DateTime.ParseExact("28.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("28.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                },
                new HomeworkDto
                {
                    Id = 2,
                    Group = new GroupDto
                    {
                        Id = 2,
                        Name = "group2",
                        GroupStatus = new GroupStatus(),
                        StartDate = DateTime.ParseExact("02.01.2021", _dateFormat, CultureInfo.InvariantCulture),
                        IsDeleted=false
                    },
                    StartDate = DateTime.ParseExact("22.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("22.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                },
                new HomeworkDto
                {
                    Id = 3,
                    Group = new GroupDto
                    {
                        Id = 3,
                        Name = "group3",
                        GroupStatus = new GroupStatus(),
                        StartDate = DateTime.ParseExact("03.01.2021", _dateFormat, CultureInfo.InvariantCulture),
                        IsDeleted=false
                    },
                    StartDate = DateTime.ParseExact("23.10.2020", _dateFormat, CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact("23.10.2021", _dateFormat, CultureInfo.InvariantCulture)
                }
            };
        }
    }
}