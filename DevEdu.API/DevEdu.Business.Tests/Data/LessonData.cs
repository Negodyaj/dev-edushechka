using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DevEdu.Business.Tests
{
    public static class LessonData
    {
        public const int ExpectedStudentLeesontId = 42;
        public static int LessonId = 30;
        public static int UserId = 42;

        public static StudentLessonDto GetStudentLessonDto()
        {
            return new StudentLessonDto
            {
                Id = 42,
                User = new UserDto { Id = UserId },
                Lesson= new LessonDto { Id=LessonId},
                Feedback = "feedback",
                IsPresent = true,
                AbsenceReason = ""
            };
        }

        public static List<StudentLessonDto> GetListStudentDto()
        {
            return new List<StudentLessonDto>
            {
                new StudentLessonDto
                {
                Id = 42,
                Feedback = "feedback",
                IsPresent = true,
                AbsenceReason = ""
                },
                 new StudentLessonDto
                {
                Id = 42,
                Feedback = "feedback2",
                IsPresent = true,
                AbsenceReason = ""
                },
                  new StudentLessonDto
                {
                Id = 42,
                Feedback = "feedback3",
                IsPresent = false,
                AbsenceReason = "Slept"
                },
            };
        }

    }
}
