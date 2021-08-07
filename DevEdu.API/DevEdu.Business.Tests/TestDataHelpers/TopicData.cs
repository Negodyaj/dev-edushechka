using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class TopicData
    {
        public static TopicDto GetTopicDtoWithoutTags()
        {
            return new TopicDto { Id = 1, Name = "Topic1", Duration = 5 };
        }

        public static TopicDto GetTopicDtoWithTags()
        {
            return new TopicDto
            {
                Id = 1,
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
                {   Id = 1,
                    Name = "Topic1",
                    Duration = 5,
                },
                new TopicDto
                {
                    Id = 2,
                    Name = "Topic2",
                    Duration = 2,
                },
                new TopicDto
                {
                    Id = 3,
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