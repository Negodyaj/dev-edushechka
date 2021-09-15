﻿using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class RatingData
    {
        public static List<StudentRatingDto> GetListOfStudentRatingDto()
        {

            return new List<StudentRatingDto>
                {
                    new StudentRatingDto
                    {
                        Id = 1,
                        Group = new GroupDto { Id = 1 },
                        User = UserData.GetStudentUserDto(),
                        RatingType = GetRatingTypeDtos()[0],
                        Rating = 80,
                        ReportingPeriodNumber = 2
                    },
                    new StudentRatingDto
                    {
                        Id = 2,
                        Group = new GroupDto { Id = 1 },
                        User = UserData.GetStudentUserDto(),
                        RatingType = GetRatingTypeDtos()[1],
                        Rating = 50,
                        ReportingPeriodNumber = 2
                    },
                    new StudentRatingDto
                    {
                        Id = 3,
                        Group = new GroupDto { Id = 1 },
                        User = new UserDto
                        {
                            Id = 10
                        },
                        RatingType = GetRatingTypeDtos()[1],
                        Rating = 50,
                        ReportingPeriodNumber = 2
                    }
            };
        }

        public static StudentRatingDto GetInputStudentRatingDto()
        {

            return new StudentRatingDto
            {
                Group = new GroupDto { Id = 1 },
                User = UserData.GetStudentUserDto(),
                RatingType = GetRatingTypeDtos()[0],
                Rating = 80,
                ReportingPeriodNumber = 2
            };
        }

        public static StudentRatingDto GetOutputStudentRatingDto()
        {

            return new StudentRatingDto
            {
                Id = 1,
                Group = new GroupDto { Id = 1 },
                User = UserData.GetStudentUserDto(),
                RatingType = GetRatingTypeDtos()[0],
                Rating = 80,
                ReportingPeriodNumber = 2
            };
        }

        public static StudentRatingDto GetStudentRatingDtoForUpdate()
        {

            return new StudentRatingDto
            {
                Id = 1,
                Rating = 80,
                ReportingPeriodNumber = 2
            };
        }

        public static StudentRatingDto GetStudentRatingDto()
        {

            return new StudentRatingDto
            {
                Id = 3,
                Group = new GroupDto { Id = 1 },
                User = new UserDto
                {
                    Id = 10
                },
                RatingType = GetRatingTypeDtos()[1],
                Rating = 50,
                ReportingPeriodNumber = 2
            };
        }

        public static List<RatingTypeDto> GetRatingTypeDtos()
        {
            return new List<RatingTypeDto>
            {
                new RatingTypeDto
                {
                    Id = 1,
                    Name = "оценка преподавателя",
                    Weight = 20
                },
                new RatingTypeDto
                {
                    Id = 2,
                    Name = "оценка посещаемости",
                    Weight = 30
                }
            };
        }

        public static UserIdentityInfo GetTeacherIdentityInfo()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role> { Role.Teacher }
            };
        }

        public static UserIdentityInfo GetTeacherOutOfGroupIdentityInfo()
        {
            return new UserIdentityInfo
            {
                UserId = 4,
                Roles = new List<Role> { Role.Teacher }
            };
        }

        public static UserIdentityInfo GetAdminIdentityInfo()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role> { Role.Admin }
            };
        }

        public static UserIdentityInfo GetManagerIdentityInfo()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role> { Role.Manager }
            };
        }
    }
}