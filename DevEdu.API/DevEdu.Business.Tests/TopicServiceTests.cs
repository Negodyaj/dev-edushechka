using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;
        private ITopicService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _sut = new TopicService(_topicRepoMock.Object, new TopicValidationHelper(_topicRepoMock.Object));
        }

        [Test]
        public async Task AddTopic_SimpleDto_TopicCreatedAsync()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).ReturnsAsync(expectedTopicId);


            //When
            var actualTopicId = await _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
        }

        [Test]
        public async Task AddTopicDto_TopicWithCreatedAsync()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = TopicData.GetTopicDto();

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).ReturnsAsync(expectedTopicId);
            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).ReturnsAsync(topicDto);


            //When
            var actualTopicId = await _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
        }

        [Test]
        public async Task DeleteTopic_IntTopicId_DeleteTopicAsync()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var expectedTopicId = 1;

            _topicRepoMock.Setup(x => x.DeleteTopicAsync(expectedTopicId));
            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).ReturnsAsync(topicDto);

            //When
            await _sut.DeleteTopicAsync(expectedTopicId);

            //Than
            _topicRepoMock.Verify(x => x.DeleteTopicAsync(expectedTopicId), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopicAsync(expectedTopicId), Times.Once);
        }

        [Test]
        public async Task GetTopic_IntTopicId_GetTopicAsync()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).ReturnsAsync(topicDto);

            //When
            var dto = await _sut.GetTopicAsync(topicId);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.GetTopicAsync(topicId), Times.Once);
        }

        [Test]
        public async Task GetAllTopics_NoEntries_ReturnedAllTopicsAsync()
        {
            //Given
            var expectedList = TopicData.GetListTopicDto();
            _topicRepoMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(expectedList);

            //When
            var actualList = await _sut.GetAllTopicsAsync();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _topicRepoMock.Verify(x => x.GetAllTopicsAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateTopic_TopicDto_ReturnUpdatedTopicDtoAsync()
        {
            //Given
            var topicDto = TopicData.GetTopicDto();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.UpdateTopicAsync(topicDto));
            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).ReturnsAsync(topicDto);

            //When
            var dto = await _sut.UpdateTopicAsync(topicId, topicDto);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.UpdateTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopicAsync(topicId), Times.Exactly(2));
        }
    }
}