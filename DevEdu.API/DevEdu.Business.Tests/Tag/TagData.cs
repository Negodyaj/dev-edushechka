

using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.Tag
{
    public class TagData
    {
        public const int expectedTagId = 13;
        public static List<TagDto> GetListTagData()
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
                },
                new TagDto
                {
                    Id = 13,
                    Name = "Tag"
                }
            };
        }
    }
}
