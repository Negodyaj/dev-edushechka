using DevEdu.DAL.Models;
using System.Collections.Generic;


namespace DevEdu.Business.Tests.Data
{
    public static class TopicData
    {
        public const int ExpectedTopicId = 42;
        public const int TopicId = 1;


        public static TopicDto GetTopicDto()
        {
            return new TopicDto
            {
                Name = "Topic1",
                Duration = 5,               
            };
        }

        public static TopicDto GetTopicWithTagsDto()
        {
            return new TopicDto
            {               
                Name = "Topic1",
                Duration = 5,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };
        }
        public static List<TopicDto> GetListTopicDto()
        {
            return new List<TopicDto>
            {
                new TopicDto
                {
                    Id = 6,
                    Name = "Topic1",
                    Duration = 5,
                },
                new TopicDto
                {
                    Id = 7,
                    Name = "Topic2",
                    Duration = 2,
                },
                new TopicDto
                {
                    Id = 8,
                    Name = "Topic3",
                    Duration = 9,
                },

            };
        }

        public static List<int> GetListTopicId()
        {
            return new List<int> { 6, 7, 8 };
        }
    }
}
