﻿using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DevEdu.Business.Tests
{
    public static class LessonData
    {
        private const string _dateFormat = "dd.MM.yyyy";

        public const int ExpectedStudentLeesontId = 42;
        public static int LessonId = 30;

        public static LessonDto GetAddedLessonDto()
        {
            return new LessonDto
            {
                Date = DateTime.ParseExact("06.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                TeacherComment = "Good",
                Teacher = new UserDto
                {
                    Id = 3
                },
                LinkToRecord = "http://link.com"
            };
        }

        public static LessonDto GetUpdatedLessonDto()
        {
            return new LessonDto
            {
                Id = LessonId,
                Date = DateTime.ParseExact("06.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                TeacherComment = "Good",
                LinkToRecord = "http://link.com"
            };
        }

        public static LessonDto GetSelectedLessonDto()  
        {
            return new LessonDto
            {
                Id = LessonId,
                Date = DateTime.ParseExact("06.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                TeacherComment = "Good",
                Teacher = new UserDto
                {
                    Id = 3
                },
                LinkToRecord = "http://link.com"
            };
        }

        public static List<LessonDto> GetLessons() 
        {
            return new List<LessonDto>{
                new LessonDto
                {
                    Id = 2,
                    Date = DateTime.ParseExact("06.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                    TeacherComment = "Good",
                    Teacher = new UserDto {
                        Id = 3,
                        FirstName = "Olga",
                        LastName = "Ivanovna",
                        Email = "olga@mail.ru",
                        Photo = " http://photo.jpg"
                    },
                    Topics = new List<TopicDto>()
                    {
                        new TopicDto{
                            Id = 4,
                            Name = "oop"
                        }
                    }
                },
                new LessonDto
                {
                    Id = 5,
                    Date = DateTime.ParseExact("12.07.2021", _dateFormat, CultureInfo.InvariantCulture),
                    TeacherComment = "Good",
                    Teacher = new UserDto {
                        Id = 3,
                        FirstName = "Olga",
                        LastName = "Ivanovna",
                        Email = "olga@mail.ru",
                        Photo = " http://photo.jpg"
                    },
                    Topics = new List<TopicDto>()
                    {
                        new TopicDto{
                            Id = 6,
                            Name = "service"
                        }, 
                        new TopicDto{
                            Id = 7,
                            Name = "tests"
                        }
                    }
                }
            };
        }

        public static List<StudentLessonDto> GetAttendances() 
        {
            return new List<StudentLessonDto> {
                new StudentLessonDto
                {
                    Id = 46,
                    Lesson = new LessonDto { Id = LessonId },
                    Feedback = "ok",
                    IsPresent = true,
                    AbsenceReason = null,
                    User = new UserDto
                    {
                        Id = 12,
                        FirstName = "Petr",
                        LastName = "Petrovich",
                        Email = "petr@mail.ru",
                        Photo = "http://petr.jpg"
                    }
                },
                new StudentLessonDto
                {
                    Id = 50,
                    Lesson = new LessonDto { Id = LessonId },
                    Feedback = "ok",
                    IsPresent = false,
                    AbsenceReason = "ill",
                    User = new UserDto
                    {
                        Id = 18,
                        FirstName = "Ivan",
                        LastName = "Ivanovich",
                        Email = "ivan@mail.ru",
                        Photo = "http://ivan.jpg"
                    }
                }
            };
        }

        public static StudentLessonDto GetStudentLessonDto()
        {
            return new StudentLessonDto
            {
                Id = 42,
                User = new UserDto { Id = 42 },
                Lesson = new LessonDto { Id = 30 },
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

        public static LessonDto GetLessonDto()
        {
            return new LessonDto
            {
                Id = 30
            };
        }
        public static UserDto GetUserDto()
        {
            return new UserDto
            {
                Id = 42
            };
        }
    }
}
