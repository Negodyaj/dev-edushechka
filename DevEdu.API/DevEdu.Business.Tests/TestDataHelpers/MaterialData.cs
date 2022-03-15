using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class MaterialData
    {
        public static MaterialDto GetMaterialDto() => new() { Id = 4, Content = "Материал по ООП" };

        public static MaterialDto GetMaterialDtoWithCoursesAndGroups() =>
            new()
            {
                Content = "Материал по Наследованию",
                Courses = new List<CourseDto>
                {
                    new() { Id = 1 },
                    new() { Id = 2 },
                    new() { Id = 3 }
                },
                Groups = new List<GroupDto>
                {
                    new() { Id = 1 },
                    new() { Id = 2 },
                    new() { Id = 3 }
                },
                IsDeleted = false
            };

        public static MaterialDto GetUpdatedMaterialDtoWithCoursesAndGroups() =>
            new()
            {
                Content = "Материал по Наследованию обновленный",
                Courses = new List<CourseDto>
                {
                    new() { Id = 1 },
                    new() { Id = 2 },
                    new() { Id = 3 }
                },
                Groups = new List<GroupDto>
                {
                    new() { Id = 1 },
                    new() { Id = 2 },
                    new() { Id = 3 }
                },
                IsDeleted = false
            };

        public static List<MaterialDto> GetListOfMaterialsWithCoursesAndGroups() =>
            new()
            {
                new MaterialDto
                {
                    Id = 2,
                    Content = "Материал по ООП",
                    Courses = new List<CourseDto>
                    {
                        new() { Id = 1 },
                        new() { Id = 2 },
                        new() { Id = 3 }
                    },
                    Groups = new List<GroupDto>
                    {
                        new() { Id = 1 },
                        new() { Id = 2 },
                        new() { Id = 3 }
                    },
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 3,
                    Content = "Материал по Наследованию",
                    Courses = new List<CourseDto>
                    {
                        new() { Id = 1 },
                        new() { Id = 2 },
                    },
                    Groups = new List<GroupDto>
                    {
                        new() { Id = 3 },
                        new() { Id = 5 },
                        new() { Id = 6 }
                    },
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 5,
                    Content = "Материал по SOLID",
                    Courses = new List<CourseDto>
                    {
                        new() { Id = 2 },
                        new() { Id = 3 }
                    },
                    Groups = new List<GroupDto>
                    {
                        new() { Id = 1 },
                        new() { Id = 3 },
                        new() { Id = 6 }
                    },
                    IsDeleted = false
                }
            };
    }
}