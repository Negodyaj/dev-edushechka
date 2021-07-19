﻿using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.Group
{
    public class GroupServiceExpecteds
    {
        public static IEnumerable AddGroup()
        {
            yield return new object[]
            {
                new GroupDto
                {
                    Id = 1, 
                    Course = new CourseDto
                    { 
                        Id = 1,
                        Name = "www",
                        Description = "w",
                        Groups = null,
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Active,
                    StartDate = System.DateTime.MaxValue,
                    Timetable = "rrr",
                    PaymentPerMonth = 1,
                    IsDeleted = false
                },
                new GroupDto
                {
                    Id = 1,
                    Course = new CourseDto
                    {
                        Id = 1,
                        Name = "www",
                        Description = "w",
                        Groups = new List<GroupDto>(),
                        IsDeleted = false
                    },
                    GroupStatus = GroupStatus.Active,
                    StartDate = System.DateTime.MaxValue,
                    Timetable = "rrr",
                    PaymentPerMonth = 1,
                    IsDeleted = false
                }                
            };
        }
        
        public static IEnumerable GetGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable GetGroups()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable UpdateGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable AddGroupMaterialReference()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable RemoveGroupMaterialReference()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }
    }
}