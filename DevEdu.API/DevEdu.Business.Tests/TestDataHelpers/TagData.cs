

using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class TagData
    {
        public static List<TagDto> GetListTagDto()
        {
            return new List<TagDto>
            {
                new TagDto
                {
                    Id = 13,
                    Name = "Tag",
                    IsDeleted = false
                },
                new TagDto
                {
                    Id = 15,
                    Name = "DevEdu",
                    IsDeleted = false
                }
            };
        }

        public static TagDto GetInputTagDto()
        {
            return new TagDto
            {
                Name = "Tag"
            };
        }

        public static TagDto GetTagDtoForUpdate()
        {
            return new TagDto
            {
                Id = 13,
                Name = "Tag"
            };
        }

        public static TagDto GetOutputTagDto()
        {
            return new TagDto
            {
                Id = 13,
                Name = "Tag",
                IsDeleted = false
            };
        }
    }
}
