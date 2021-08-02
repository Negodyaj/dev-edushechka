using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class MaterialData
    {
        public static MaterialDto GetMaterialDtoWithoutTags() =>
            new MaterialDto
            {
                Id = 1,
                Content = "Материал по ООП"
            };

        public static MaterialDto GetMaterialDtoWithTags() =>
            new MaterialDto
            {
                Content = "Материал по ООП",
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                }
            };

        public static MaterialDto GetAnotherMaterialDtoWithTags() =>
            new MaterialDto
            {
                Content = "Материал по Наследованию",
                Tags = new List<TagDto>
                {
                    new TagDto {Id = 1},
                    new TagDto {Id = 2},
                    new TagDto {Id = 3}
                }
            };

        public static List<MaterialDto> GetListOfMaterials() =>
            new List<MaterialDto>
            {
                new MaterialDto
                {
                    Content = "Материал по ООП",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 1},
                        new TagDto {Id = 2},
                        new TagDto {Id = 3}
                    }
                },
                new MaterialDto
                {
                    Content = "Материал по Наследованию",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 5},
                        new TagDto {Id = 4},
                        new TagDto {Id = 1}
                    }
                },
                new MaterialDto
                {
                    Content = "Материал по SOLID",
                    Tags = new List<TagDto>
                    {
                        new TagDto {Id = 1},
                        new TagDto {Id = 4},
                        new TagDto {Id = 6}
                    }
                }
            };

        public static List<CourseDto> GetListOfCourses() =>
            new List<CourseDto>
            {
                new CourseDto {Id = 1},
                new CourseDto {Id = 2},
                new CourseDto {Id = 3}
            };

        public static List<GroupDto> GetListOfGroups() =>
            new List<GroupDto>
            {
                new GroupDto {Id = 1},
                new GroupDto {Id = 2},
                new GroupDto {Id = 3}
            };
    }
}
