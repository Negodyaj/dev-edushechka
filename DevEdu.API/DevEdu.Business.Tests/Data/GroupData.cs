using System;
using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class GroupData
    {
        public const int ExpectedAffectedRows = 1;
        public const int GroupId = 1;
        public const int MaterialId = 1;
        public const int RoleStudent = (int)Role.Student;
        public const int StatusGroup = 1;
        public static List<GroupDto> GetListOfGroupDto()
        {
            return new List<GroupDto> {

                new GroupDto
                {
                    Id = 1,
                    Name = "group1",
                    GroupStatus = new GroupStatus(),
                    StartDate = DateTime.Parse("01.01.2021"),
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 2,
                    Name = "group2",
                    GroupStatus = new GroupStatus(),
                    StartDate = DateTime.Parse("02.01.2021"),
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 3,
                    Name = "group3",
                    GroupStatus = new GroupStatus(),
                    StartDate = DateTime.Parse("03.01.2021"),
                    IsDeleted = false
                }
            };
        }
    }
}