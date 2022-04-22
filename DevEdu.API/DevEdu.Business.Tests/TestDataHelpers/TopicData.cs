using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public static class TopicData
    {
        public static TopicDto GetTopicDto()
        {
            return new TopicDto { Id = 1, Name = "Topic1", Duration = 5 };
        }

        public static TopicDto GetAnotherTopicDto()
        {
            return new TopicDto { Id = 4, Name = "Topic4", Duration = 5 };
        }

        public static List<TopicDto> GetListTopicDto()
        {
            return new List<TopicDto>
            {
                new()
                {
                    Id = 1,
                    Name = "Topic1",
                    Duration = 5,
                },
                new()
                {
                    Id = 2,
                    Name = "Topic2",
                    Duration = 2,
                },
                new()
                {
                    Id = 3,
                    Name = "Topic3",
                    Duration = 9,
                }
            };
        }

        public static List<int> GetListTopicId()
        {
            return new List<int> { 6, 7, 8 };
        }
    }
}