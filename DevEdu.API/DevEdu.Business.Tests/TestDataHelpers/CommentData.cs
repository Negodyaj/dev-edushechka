using System;
using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    public static class CommentData
    {
        public static CommentDto GetCommentDto()
        {
            return new CommentDto
            {
                Id = 1,
                Lesson = new LessonDto
                {
                    Id = 1
                },
                Text = "comment1",
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Cat",
                    LastName = "Jack",
                    Email = "CatJack@meow.cat",
                    Photo = "Cat.jpg"
                },
                Date = DateTime.Parse("19.07.2021"),
                IsDeleted = false
            };
        }

        public static LessonDto GetLessonDto()
        {
            return new LessonDto
            {
                Id = 1
            };
        }

        public static StudentLessonDto GetStudentLessonDto()
        {
            return new StudentLessonDto
            {
                Id = 1
            };
        }

        public static StudentAnswerOnTaskDto GetStudentAnswerOnTaskDto()
        {
            return new StudentAnswerOnTaskDto
            {
                Id = 1,
                User=new UserDto
                {
                    Id = 10
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

        public static List<GroupDto> GetGroupsDto()
        {
            return new List<GroupDto>
            {
                new GroupDto
                {
                    Id = 10
                },

                new GroupDto
                {
                    Id = 20
                }
            };
        }
    }
}