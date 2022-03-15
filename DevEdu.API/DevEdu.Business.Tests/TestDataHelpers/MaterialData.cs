using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class MaterialData
    {
        public static MaterialDto GetMaterialDto() => new() { Id = 4, Link = "https://github.com/", Content = "Материал по ООП" };

        public static MaterialDto GetUpdatedMaterialDto() =>
            new()
            {
                Content = "Материал по Наследованию обновленный",
                Link = "https://github.com/4688",
                IsDeleted = false
            };

        public static List<MaterialDto> GetListOfMaterials() =>
            new()
            {
                new MaterialDto
                {
                    Id = 2,
                    Content = "Материал по ООП",
                    Link = "https://github.com/777",
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 3,
                    Content = "Материал по Наследованию",
                    Link = "https://github.com/9897",
                    IsDeleted = false
                },
                new MaterialDto
                {
                    Id = 5,
                    Content = "Материал по SOLID",
                    Link = "https://github.com/12054",
                    IsDeleted = false
                }
            };
    }
}