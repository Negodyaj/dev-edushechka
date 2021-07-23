using System.Collections.Generic;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using Moq;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTopic_DtoWithTags_TopicWithTagsCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto
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

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }
    }
}