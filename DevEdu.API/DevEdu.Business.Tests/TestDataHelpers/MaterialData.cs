using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public static class MaterialData
    {
        public static MaterialDto GetMaterialDtoWithoutTags() => 
            new MaterialDto { Content = "Материал по ООП" };

        public static MaterialDto GetMaterialDtoWithTags() =>
            new MaterialDto 
            { 
                Content = "Материал по ООП",
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                },
                IsDeleted = false
            };

        public static MaterialDto GetMaterialDtoWithTagsCoursesAndGroups() =>
            new MaterialDto
            {
                Content = "Материал по Наследованию",
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                },
                Courses = new List<CourseDto>
                    {
                        new CourseDto {Id = 1},
                        new CourseDto {Id = 2},
                        new CourseDto {Id = 3}
                    },
                Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 2},
                        new GroupDto {Id = 3}
                    },
                IsDeleted = false
            };
        public static MaterialDto GetUpdatedMaterialDtoWithTagsCoursesAndGroups() =>
            new MaterialDto
            {
                Content = "Материал по Наследованию обновленный",
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                },
                Courses = new List<CourseDto>
                    {
                        new CourseDto {Id = 1},
                        new CourseDto {Id = 2},
                        new CourseDto {Id = 3}
                    },
                Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 2},
                        new GroupDto {Id = 3}
                    },
                IsDeleted = false
            };

        public static List<MaterialDto> GetListOfMaterialsWithTagsCoursesAndGroups() =>
            new List<MaterialDto>
            {
                new MaterialDto
                {
                    Id = 2,
                    Content = "Материал по ООП",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 1},
                        new TagDto {Id = 2},
                        new TagDto {Id = 3}
                    },
                    Courses = new List<CourseDto>
                    {
                        new CourseDto {Id = 1},
                        new CourseDto {Id = 2},
                        new CourseDto {Id = 3}
                    },
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 2},
                        new GroupDto {Id = 3}
                    },
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 3,
                    Content = "Материал по Наследованию",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 5},
                        new TagDto {Id = 4},
                        new TagDto {Id = 1}
                    },
                    Courses = new List<CourseDto>
                    {
                        new CourseDto {Id = 1},
                        new CourseDto {Id = 2},
                    },
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 3},
                        new GroupDto {Id = 5},
                        new GroupDto {Id = 6}
                    },
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 5,
                    Content = "Материал по SOLID",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 1},
                        new TagDto {Id = 4},
                        new TagDto {Id = 6}
                    },
                    Courses = new List<CourseDto>
                    {
                        new CourseDto {Id = 2},
                        new CourseDto {Id = 3}
                    },
                    Groups = new List<GroupDto>
                    {
                        new GroupDto {Id = 1},
                        new GroupDto {Id = 3},
                        new GroupDto {Id = 6}
                    },
                    IsDeleted = false
                }
            };
    }
}
