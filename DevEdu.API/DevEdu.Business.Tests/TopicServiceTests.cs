using System.Collections.Generic;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using Moq;
using DevEdu.Business.Tests.Data;
using DevEdu.Business.ValidationHelpers;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepository;
        private Mock<IGroupRepository> _groupRepository;
        private TopicValidationHelper _topicValidationHelper;
        private TopicService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepository = new Mock<ITopicRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _topicValidationHelper = new TopicValidationHelper(_topicRepository.Object);
            _sut = new TopicService(_topicRepository.Object, _topicValidationHelper);
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given            
            var topicDto = TopicData.GetTopicDto();
            var ExpectedTopicId = 42;
            _topicRepository.Setup(x => x.AddTopic(topicDto)).Returns(ExpectedTopicId);
            _topicRepository.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));           

            //When
            var actualTopicId = _sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(ExpectedTopicId, actualTopicId);
            _topicRepository.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepository.Verify(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()), Times.Never);            
        }

        [Test]
        public void AddTopic_DtoWithTags_TopicWithTagsCreated()
        {
            //Given                      
            var topicDto = TopicData.GetTopicWithTagsDto();
            var ExpectedTopicId = 42;

            _topicRepository.Setup(x => x.AddTopic(topicDto)).Returns(ExpectedTopicId);
            _topicRepository.Setup(x => x.AddTagToTopic(ExpectedTopicId, It.IsAny<int>()));            

            //When
            var actualTopicId = _sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(ExpectedTopicId, actualTopicId);
            _topicRepository.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepository.Verify(x => x.AddTagToTopic(ExpectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));            
        }
        [Test]
        public void DeleteTopic_IntTopicId_DeleteTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var topicId = 1;            

            _topicRepository.Setup(x => x.DeleteTopic(topicId));
            _topicRepository.Setup(x => x.GetTopic(topicId)).Returns(topicDto);

            //When
            _sut.DeleteTopic(topicId);

            //Than
            _topicRepository.Verify(x => x.DeleteTopic(topicId), Times.Once);
            _topicRepository.Verify(x => x.GetTopic(topicId), Times.Once);
        }
        [Test]
        public void GetTopic_IntTopicId_GetTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var topicId = 1;

            _topicRepository.Setup(x => x.GetTopic(topicId)).Returns(topicDto);            

            //When
            var dto = _sut.GetTopic(topicId);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepository.Verify(x => x.GetTopic(topicId), Times.Exactly(2));           
        }

        [Test]
        public void GetAllTopics_NoEntries_ReturnedAllTopics()
        {
            //Given
            var expectedList = TopicData.GetListTopicDto();
            _topicRepository.Setup(x => x.GetAllTopics()).Returns(expectedList);
            
            //When
            var actualList = _sut.GetAllTopics();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _topicRepository.Verify(x => x.GetAllTopics(), Times.Once);           
        }

        [Test]
        public void UpdateTopic_TopicDto_ReturnUpdatedTopicDto()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var topicId = 1;

            _topicRepository.Setup(x => x.UpdateTopic(topicDto));
            _topicRepository.Setup(x => x.GetTopic(topicId)).Returns(topicDto);
            
            //When
            var dto = _sut.UpdateTopic(topicId, topicDto);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepository.Verify(x => x.UpdateTopic(topicDto), Times.Once);
            _topicRepository.Verify(x => x.GetTopic(topicId), Times.Exactly(2));           
        }

        


    }
}