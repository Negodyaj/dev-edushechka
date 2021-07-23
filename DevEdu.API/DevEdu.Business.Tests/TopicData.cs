using System.Collections.Generic;
using DevEdu.DAL.Models;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public static class TopicData
    {
        public const int expectedTopicId = 55;

        public static TopicDto GetTopicDtoWithoutTags()
        {
            return new TopicDto { Name = "Topic1", Duration = 5 };
        }

        public static TopicDto GetTopicDtoWithTags()
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
    }
}