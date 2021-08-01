using System.Collections.Generic;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using Moq;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;
        private Mock<ITagRepository> _tagRepoMock;
        private Mock<ITopicValidationHelper> _topicValidationHelperMock;
        private Mock<ITagValidationHelper> _tagValidationHelperMock;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _topicValidationHelperMock = new Mock<ITopicValidationHelper>();
            _tagValidationHelperMock = new Mock<ITagValidationHelper>();
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object, _topicValidationHelperMock.Object, _tagValidationHelperMock.Object);

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
            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(topicDto);
            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(topicDto.Tags[0]);

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object, _topicValidationHelperMock.Object, _tagValidationHelperMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }

        [Test]
        public void AddTagToTopic_WhenTopicNotFound_EntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            _topicValidationHelperMock.Setup(x => x.CheckTopicExistence(expectedTopicId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", expectedTopicId)));

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object, new TopicValidationHelper(_topicRepoMock.Object), new TagValidationHelper(_tagRepoMock.Object));

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.AddTagToTopic(expectedTopicId, expectedTagId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", expectedTopicId)));

            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, expectedTagId), Times.Never);
            //_topicValidationHelperMock.Verify(x => x.CheckTopicExistence(expectedTopicId), Times.Once);
            //_tagValidationHelperMock.Verify(x => x.CheckTagExistence(expectedTagId), Times.Never);
        }

        [Test]
        public void AddTagToTopic_WhenTagNotFound_EntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            _tagValidationHelperMock.Setup(x => x.CheckTagExistence(expectedTagId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "tag", expectedTagId)));
            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(TopicData.GetTopicDtoWithTags);

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object, new TopicValidationHelper(_topicRepoMock.Object), new TagValidationHelper(_tagRepoMock.Object));

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.AddTagToTopic(expectedTopicId, expectedTagId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "tag", expectedTagId)));

            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, expectedTagId), Times.Never);
            //_topicValidationHelperMock.Verify(x => x.CheckTopicExistence(expectedTopicId), Times.Once);
            //_tagValidationHelperMock.Verify(x => x.CheckTagExistence(expectedTagId), Times.Once);
        }
    }
}