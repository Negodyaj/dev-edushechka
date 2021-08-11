using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class UserData
    {
        public const int expectedUserId = 33;

        public static UserDto GetUserDto()
        {
            return new UserDto
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Adminov",
                Patronymic = "Adminovich",
                Email = "admin@admin.ad",
                Username = "Admin01",
                Password = "qwerty12345",
                ContractNumber = "admin01",
                City = (City)1,
                BirthDate = DateTime.Today,
                GitHubAccount = "admin/admin.git",
                Photo = "https://localhost:IsAdmin/admin",
                PhoneNumber = "adminPhoneNumber",
                Roles = new List<Role>
                {
                    Role.Student,
                    Role.Admin
                }
            };
        }

        public static UserDto GetAnotherUserDto()
        {
            return new UserDto
            {
                Id = 2,
                FirstName = "Student",
                LastName = "Studentov",
                Patronymic = "Studentovich",
                Email = "student@student.st",
                Username = "Student01",
                Password = "qwerty12345",
                ContractNumber = "Student01",
                City = (City)1,
                BirthDate = DateTime.Today,
                GitHubAccount = "IsStudent/IsStudent.git",
                Photo = "https://localhost:IsStudent",
                PhoneNumber = "StudentPhoneNumber",
                Roles = new List<Role> { Role.Student }
            };
        }
        public static UserDto GetTeacherDto()
        {
            return new UserDto
            {
                Id = 3,
                FirstName = "Olga",
                LastName = "Ivanovna",
                Email = "olga@mail.ru",
                Photo = " http://photo.jpg"
            };
        }

        public static List<UserDto> GetListUsersDto()
        {
            return new List<UserDto>
            {
                new UserDto
                {
                    Id = 1,
                    FirstName = "Admin",
                        LastName = "Adminov",
                        Patronymic = "Adminovich",
                        Email = "admin@admin.ad",
                        Username = "Admin01",
                        Password = "qwerty12345",
                        ContractNumber = "admin01",
                        City = (City)1,
                        BirthDate = DateTime.Today,
                        GitHubAccount = "admin/admin.git",
                        Photo = "https://localhost:IsAdmin/admin",
                        PhoneNumber = "adminPhoneNumber",
                        Roles = new List<Role>
                        {
                            Role.Student,
                            Role.Admin
                        }
                },
                new UserDto
                {
                    Id = 2,
                    FirstName = "Student",
                    LastName = "Studentov",
                    Patronymic = "Studentovich",
                    Email = "student@student.st",
                    Username = "Student01",
                    Password = "qwerty12345",
                    ContractNumber = "Student01",
                    City = (City)1,
                    BirthDate = DateTime.Today,
                    GitHubAccount = "IsStudent/IsStudent.git",
                    Photo = "https://localhost:IsStudent",
                    PhoneNumber = "StudentPhoneNumber",
                    Roles = new List<Role> { Role.Student }
                },
                new UserDto
                {
                    Id = 3,
                    FirstName = "Manager",
                    LastName = "Managerov",
                    Patronymic = "Managerovich",
                    Email = "Manager@manager.mn",
                    Username = "SManager01",
                    Password = "qwerty12345",
                    ContractNumber = "Manager01",
                    City = (City)1,
                    BirthDate = DateTime.Today,
                    GitHubAccount = "Manager/Manager.git",
                    Photo = "https://localhost:Manager",
                    PhoneNumber = "ManagerPhoneNumber",
                    Roles = new List<Role> { Role.Manager }
                }
            };
        }

        public static UserDto GetUserDtoOutOfList()
        {
            return new UserDto
            {
                Id = 10,
                FirstName = "Student",
                LastName = "Studentov",
                Patronymic = "Studentovich",
                Email = "student@student.st",
                Username = "Student01",
                Password = "qwerty12345",
                ContractNumber = "Student01",
                City = (City)1,
                BirthDate = DateTime.Today,
                GitHubAccount = "Student/Student.git",
                Photo = "https://localhost:Student",
                PhoneNumber = "StudentPhoneNumber",
                Roles = new List<Role> { Role.Student }
            };
        }

        public static List<List<UserDto>> GetListsOfUsersInGroup()
        {
            return new List<List<UserDto>>
            {
                new List<UserDto>
                {
                    new UserDto {Id = 2},
                    new UserDto {Id = 3},
                    new UserDto {Id = 4},
                },
                new List<UserDto>
                {
                    new UserDto {Id = 2},
                    new UserDto {Id = 5},
                    new UserDto {Id = 6},
                },
                new List<UserDto>
                {
                    new UserDto {Id = 2},
                    new UserDto {Id = 7},
                    new UserDto {Id = 8},
                }
            };
        }

        public static List<List<UserDto>> GetAnotherListsOfUsersInGroup()
        {
            return new List<List<UserDto>>
            {
                new List<UserDto>
                {
                    new UserDto {Id = 2},
                    new UserDto {Id = 3},
                    new UserDto {Id = 4},
                },
                new List<UserDto>
                {
                    new UserDto {Id = 2},
                    new UserDto {Id = 5},
                    new UserDto {Id = 6},
                },
                new List<UserDto>
                {
                    new UserDto {Id = 9},
                    new UserDto {Id = 7},
                    new UserDto {Id = 8},
                }
            };
        }
    }
}