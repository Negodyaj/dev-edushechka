using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using DevEdu.Business.Tests.Data;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;
        private Mock<ITagRepository> _tagRepoMock;
        private TopicValidationHelper _topicValidationHelper;
        private Mock<ITagValidationHelper> _tagValidationHelperMock;
        private ITopicService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _topicValidationHelper = new TopicValidationHelper(_topicRepoMock.Object);
            _tagValidationHelperMock = new Mock<ITagValidationHelper>();
            _sut = new TopicService(
            _topicRepoMock.Object,
            _tagRepoMock.Object,
            new TopicValidationHelper(
                _topicRepoMock.Object),
            new TagValidationHelper(_tagRepoMock.Object)
            );
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));

            
            //When
            var actualTopicId = _sut.AddTopic(topicDto);

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
            var topicDto = TopicData.GetTopicDtoWithTags();

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()));
            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(topicDto);
            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(topicDto.Tags[0]);

           
            //When
            var actualTopicId = _sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }

        [Test]
        public void DeleteTopic_IntTopicId_DeleteTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var expectedTopicId = 1;

            _topicRepoMock.Setup(x => x.DeleteTopic(expectedTopicId));
            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(topicDto);

            //When
            _sut.DeleteTopic(expectedTopicId);

            //Than
            _topicRepoMock.Verify(x => x.DeleteTopic(expectedTopicId), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopic(expectedTopicId), Times.Once);
        }
        [Test]
        public void GetTopic_IntTopicId_GetTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.GetTopic(topicId)).Returns(topicDto);            

            //When
            var dto = _sut.GetTopic(topicId);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.GetTopic(topicId), Times.Once);           
        }

        [Test]
        public void GetAllTopics_NoEntries_ReturnedAllTopics()
        {
            //Given
            var expectedList = TopicData.GetListTopicDto();
            _topicRepoMock.Setup(x => x.GetAllTopics()).Returns(expectedList);
            
            //When
            var actualList = _sut.GetAllTopics();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _topicRepoMock.Verify(x => x.GetAllTopics(), Times.Once);           
        }

        [Test]
        public void UpdateTopic_TopicDto_ReturnUpdatedTopicDto()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.UpdateTopic(topicDto));
            _topicRepoMock.Setup(x => x.GetTopic(topicId)).Returns(topicDto);
            
            //When
            var dto = _sut.UpdateTopic(topicId, topicDto);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.UpdateTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopic(topicId), Times.Exactly(2));           
        }

        [Test]
        public void AddTagToTopic_WhenTopicNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", expectedTopicId)),
                () => _sut.AddTagToTopic(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public void AddTagToTopic_WhenTagNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(TopicData.GetTopicDtoWithTags);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "tag", expectedTagId)),
            () => _sut.AddTagToTopic(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public void DeleteTagFromTopic_IntTopicIdAndTagId_DeleteTagFromTopic()
        {
            //Given
            var topicId = 1;
            var tagId = 13;
            var expecectedAffectedRows = 1;

            _topicRepoMock.Setup(x => x.GetTopic(topicId)).Returns(TopicData.GetTopicDtoWithTags());
            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(TagData.GetTagDto());
            _topicRepoMock.Setup(x => x.DeleteTagFromTopic(topicId, tagId)).Returns(expecectedAffectedRows);

            //When
            var actualAffectedRows = _sut.DeleteTagFromTopic(topicId, tagId);

            //Than
            Assert.AreEqual(expecectedAffectedRows, actualAffectedRows);
            _topicRepoMock.Verify(x => x.DeleteTagFromTopic(topicId, tagId), Times.Once);
        }
    }
}