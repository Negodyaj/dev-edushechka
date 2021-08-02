using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace DevEdu.Business.Tests
{
    public static class TopicData
    {
        public static List<TopicDto> GetTopics()
        {
            return new List<TopicDto>{
                new TopicDto
                {
                    Id = 6,
                    Name = "Cycles"
                },
                new TopicDto
                {
                    Id = 7,
                    Name = "OOP"
                }
            };
        }
    }
}
