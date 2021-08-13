using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupData
    {
        public const int GroupId = 1;
        public const int MaterialId = 1;
        public const int RoleStudent = (int)Role.Student;
        public const int StatusGroup = 1;

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
                    StartDate = DateTime.MaxValue,
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
                    StartDate = DateTime.MaxValue,
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
                    StartDate = DateTime.MaxValue,
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

        public static List<Role> GetStudentRole()
        {
            return new List<Role>
            {
                Role.Student
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
                StartDate = DateTime.MaxValue,
                Timetable = "Понедельник",
                PaymentPerMonth = 1.0M,
                Students = null,
                Teachers = null,
                Tutors = null
            };
        }

        public static List<GroupDto> GetAnotherListGroupDtos()
        {
            return new List<GroupDto>
            {
                new GroupDto
                {
                    Id = 4,
                    Name = "Веселые ребята",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Древесные клен",
                        Description = "Курс для пней",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "С зимы по лето - по зиму и снова по лето",
                    PaymentPerMonth = 1010.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 5,
                    Name = "Окорок",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Как быстро бегать",
                        Description = "Курс для птиц",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Вечерние занятия с 19-00",
                    PaymentPerMonth = 2828.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 6,
                    Name = "Посмотри",
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "Делаем красиво",
                        Description = "качаем глаза",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Forming,
                    StartDate = DateTime.MaxValue,
                    Timetable = "Среда, четверг 14:00",
                    PaymentPerMonth = 1707.0M,
                    Students = null,
                    Teachers = null,
                    Tutors = null,
                    IsDeleted = false
                }
            };
        }
    }
}