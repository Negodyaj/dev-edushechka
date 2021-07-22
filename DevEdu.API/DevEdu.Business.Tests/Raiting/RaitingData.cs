
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class RaitingData
    {
        public const int expectedStudentRaitingId = 13;

        public static List<StudentRaitingDto> GetListOfStudentRaitingDto()
        {

            return new List<StudentRaitingDto>
                {
                    new StudentRaitingDto
                    {
                        Id = 13,
                        Group = new GroupDto { Id = 1 },
                        User = UserData.GetAnotherUserDto(),
                        RaitingType = GetRaitingTypeDtos()[0],
                        Raiting = 80,
                        ReportingPeriodNumber = 2
                    },
                    new StudentRaitingDto
                    {
                        Id = 14,
                        Group = new GroupDto { Id = 1 },
                        User = UserData.GetAnotherUserDto(),
                        RaitingType = GetRaitingTypeDtos()[1],
                        Raiting = 50,
                        ReportingPeriodNumber = 2
                    },
                    new StudentRaitingDto
                    {
                        Id = 13,
                        Raiting = 80,
                        ReportingPeriodNumber = 2
                    }
            };
        }

        public static List<RaitingTypeDto> GetRaitingTypeDtos()
        {
            return new List<RaitingTypeDto>
            {
                new RaitingTypeDto
                {
                    Id = 1,
                    Name = "оценка преподавателя",
                    Weight = 20
                },
                new RaitingTypeDto
                {
                    Id = 2,
                    Name = "оценка посещаемости",
                    Weight = 30
                }
            };
        }
    }
}
