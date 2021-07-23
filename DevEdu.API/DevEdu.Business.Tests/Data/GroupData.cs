using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupData
    {
        public const int ExpectedAffectedRows = 1;
        public const int GroupId = 1;
        public const int MaterialId = 1;

        public static GroupDto GetGroupDto()
        {
            return new GroupDto
            {
                Id = 1,
                Name = "Котейка",
                Course = new CourseDto
                {
                    Id = 1,
                    Name = "Ололошки",
                    Description = "Курс для котиков",
                    Groups = null,
                    IsDeleted = false
                },
                GroupStatus = GroupStatus.Formed,
                StartDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Users = new List<UserDto>
                {
                    new UserDto
                    {
                        Id = 1,
                        FirstName = "Котафей",
                        LastName = "Котофеевич",
                        Patronymic = "Собака",
                        Email = "kots@ya.ru",
                        Username = "KotVsDog",
                        Password = "GlobalWar",
                        RegistrationDate = DateTime.MinValue,
                        ContractNumber = "000#Cat",
                        BirthDate = DateTime.MaxValue,
                        GitHubAccount = @"ZLoo.Git.Hub",
                        Photo = @"url/worldMap",
                        PhoneNumber = "666-6666-666",
                        ExileDate = DateTime.MaxValue,
                        City = City.SaintPetersburg,
                        Roles = new List<Role>
                        {
                            Role.Manager,
                            Role.Methodist,
                            Role.Student
                        },
                        IsDeleted = false
                    },
                    new UserDto
                    {
                        Id = 1,
                        FirstName = "Котафей",
                        LastName = "Котофеевич",
                        Patronymic = "Собака",
                        Email = "kots@ya.ru",
                        Username = "KotVsDog",
                        Password = "GlobalWar",
                        RegistrationDate = DateTime.MinValue,
                        ContractNumber = "000#Cat",
                        BirthDate = DateTime.MaxValue,
                        GitHubAccount = @"ZLoo.Git.Hub",
                        Photo = @"url/worldMap",
                        PhoneNumber = "666-6666-666",
                        ExileDate = DateTime.MaxValue,
                        City = City.SaintPetersburg,
                        Roles = new List<Role>
                        {
                            Role.Manager,
                            Role.Methodist,
                            Role.Student
                        },
                        IsDeleted = false
                    },
                    new UserDto
                    {
                        Id = 1,
                        FirstName = "Котафей",
                        LastName = "Котофеевич",
                        Patronymic = "Собака",
                        Email = "kots@ya.ru",
                        Username = "KotVsDog",
                        Password = "GlobalWar",
                        RegistrationDate = DateTime.MinValue,
                        ContractNumber = "000#Cat",
                        BirthDate = DateTime.MaxValue,
                        GitHubAccount = @"ZLoo.Git.Hub",
                        Photo = @"url/worldMap",
                        PhoneNumber = "666-6666-666",
                        ExileDate = DateTime.MaxValue,
                        City = City.SaintPetersburg,
                        Roles = new List<Role>
                        {
                            Role.Manager,
                            Role.Methodist,
                            Role.Student
                        },
                        IsDeleted = false
                    },
                },
                IsDeleted = false
            };
        }

        public static List<GroupDto> GetGroupsDto()
        {
            return new List<GroupDto>
            {
                new GroupDto
                {
                    Id = 1,
                    Name = "Котейка",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Ололошки",
                        Description = "Курс для котиков",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Formed,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Users = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 1,
                    Name = "Котейка",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Ололошки",
                        Description = "Курс для котиков",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Formed,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Users = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 1,
                    Name = "Котейка",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Ололошки",
                        Description = "Курс для котиков",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Formed,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Users = null,
                    IsDeleted = false
                }                
            };
        }
    }
}