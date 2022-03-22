using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupData
    {
        public static UserIdentityInfo GetUserInfo()
        {
            return new UserIdentityInfo() { UserId = 1, Roles = new() { Role.Admin, Role.Manager } };
        }

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
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }

        public static GroupDto GetGroupDtoCompareException()
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
                EndDate = DateTime.MinValue,
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
                EndDate = DateTime.MaxValue,
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
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }

        public static List<GroupDto> GetGroupDtos()
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
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = new List<UserDto>()
                    {
                        new UserDto {
                            Id = 2,
                            Roles = new List<Role>()
                            {
                                Role.Teacher
                            }
                        },
                    },
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 2,
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
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = new List<UserDto>()
                    {
                        new UserDto {
                            Id = 2,
                            Roles = new List<Role>()
                            {
                                Role.Teacher
                            }
                        },
                    },
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 3,
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
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers =  new List<UserDto>()
                    {
                        new UserDto {
                            Id = 2,
                            Roles = new List<Role>()
                            {
                                Role.Teacher
                            }
                        },
                    },
                    Tutors = null,
                    IsDeleted = false
                }
            };
        }

        public static List<GroupDto> GetAnotherGroupDtos()
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
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 2,
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
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 5479.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                }
            };
        }

        public static List<GroupDto> GetAnotherListDtos()
        {
            return new List<GroupDto>
            {
                new GroupDto
                {
                    Id = 4,
                    Name = "Петровичи",
                    Course = new CourseDto
                    {
                        Id = 5,
                        Name = "Маляристов",
                        Description = "Курс для маляров",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Среда 10-20",
                    PaymentPerMonth = 5789.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 5,
                    Name = "Рукаделы",
                    Course = new CourseDto
                    {
                        Id = 6,
                        Name = "Гончарство",
                        Description = "Курс для Гончаристов",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MaxValue,
                    Timetable = "Понедельник 10-20",
                    PaymentPerMonth = 7894.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                }
            };
        }

        public static GroupDto GetAnotherGroupDto()
        {
            return new GroupDto
            {
                Id = 100,
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
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }
    }
}