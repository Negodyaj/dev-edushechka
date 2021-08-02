using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using DevEdu.Business.Tests.Data;

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

            var topicDto = TopicData.GetTopicDto();
            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(TopicData.ExpectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(TopicData.ExpectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTopic_DtoWithTags_TopicWithTagsCreated()
        {
            //Given
           
            var topicDto = TopicData.GetTopicWithTagsDto();


            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(TopicData.ExpectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(TopicData.ExpectedTopicId, It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(TopicData.ExpectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(TopicData.ExpectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }
        [Test]
        public void DeleteTopic_IntTopicId_DeleteTopic()
        {
            //Given
            const int topicId = TopicData.TopicId;

            _topicRepoMock.Setup(x => x.DeleteTopic(topicId));

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            sut.DeleteTopic(topicId);

            //Than
            _topicRepoMock.Verify(x => x.DeleteTopic(topicId), Times.Once);
        }
        [Test]
        public void GetTopic_IntTopicId_GetTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            const int topicId = TopicData.TopicId;

            _topicRepoMock.Setup(x => x.GetTopic(topicId)).Returns(topicDto);

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var dto = sut.GetTopic(topicId);

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
            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var actualList = sut.GetAllTopics();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _topicRepoMock.Verify(x => x.GetAllTopics(), Times.Once);           
        }

        [Test]
        public void UpdateTopic_TopicDto_ReturnUpdatedTopicDto()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            const int topicId = TopicData.TopicId;

            _topicRepoMock.Setup(x => x.UpdateTopic(topicDto));
            _topicRepoMock.Setup(x => x.GetTopic(topicId)).Returns(topicDto);

            var sut = new TopicService(_topicRepoMock.Object);

            //When
            var dto = sut.UpdateTopic(topicId, topicDto);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.UpdateTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopic(topicId), Times.Once);
        }

        


    }
}