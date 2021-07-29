using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupData
    {
        public const int ExpectedAffectedRows = 1;
        public const int GroupId = 1;
        public const int LessonId = 1;
        public const int TaskId = 1;
        public const int UserId = 1;
        public const int MaterialId = 1;
        public const Role RoleStudent = Role.Student;
        public static string Answer = $"Group {GroupId} remove  Lesson Id:{LessonId}";
        public const GroupStatus StatusGroup = GroupStatus.Forming;

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
                GroupStatus = GroupStatus.Forming,
                StartDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }

        public static GroupDto GetUpdGroupDto()
        {
            return new GroupDto
            {
                Id = 1,
                Name = "Пончики",
                Course = new CourseDto
                {
                    Id = 1,
                    Name = "Ололошки",
                    Description = "Курс для котиков",
                    Groups = null,
                    IsDeleted = false
                },
                GroupStatus = GroupStatus.Forming,
                StartDate = DateTime.MinValue,
                Timetable = "Вторник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }
        public static GroupDto GetGroupDtoToUpdNameAndTimetable()
        {
            return new GroupDto
            {
                Id = 1,
                Name = "Котейка",
                Course = null,
                GroupStatus = GroupStatus.Forming,
                StartDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
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
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
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
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
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
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                }                
            };
        }

        public static List<UserDto> GetUserForGroup()
        {
            return new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    FirstName = "Котафей",
                    LastName = "Котофеевич",
                    Email = "kots@ya.ru",
                    RegistrationDate = DateTime.MinValue,
                    BirthDate = DateTime.MaxValue,
                    Photo = @"url/worldMap",
                    ExileDate = DateTime.MaxValue,
                    City = City.SaintPetersburg,
                    Patronymic = null,
                    Username = null,
                    Password = null,
                    ContractNumber = null,
                    GitHubAccount = null,
                    PhoneNumber = null,
                    Roles = null,
                    IsDeleted = false
                }                
            };
        }
    }
}