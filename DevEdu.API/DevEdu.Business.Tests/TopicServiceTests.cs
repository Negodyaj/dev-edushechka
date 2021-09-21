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
        private Mock<ITagRepository> _tagRepoMock;
        private ITopicService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _sut = new TopicService(
            _topicRepoMock.Object,
            _tagRepoMock.Object,
            new TopicValidationHelper(
                _topicRepoMock.Object),
            new TagValidationHelper(_tagRepoMock.Object)
            );
        }

        [Test]
        public async Task AddTopic_SimpleDtoWithoutTags_TopicCreatedAsync()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).ReturnsAsync(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopicAsync(It.IsAny<int>(), It.IsAny<int>()));


            //When
            var actualTopicId = await _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task AddTopic_DtoWithTags_TopicWithTagsCreatedAsync()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = TopicData.GetTopicDtoWithTags();

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).ReturnsAsync(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopicAsync(expectedTopicId, It.IsAny<int>()));
            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).ReturnsAsync(topicDto);
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(It.IsAny<int>())).ReturnsAsync(topicDto.Tags[0]);


            //When
            var actualTopicId = await _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }

        [Test]
        public async Task DeleteTopic_IntTopicId_DeleteTopicAsync()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
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
            var topicDto = TopicData.GetTopicDtoWithoutTags();
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
            var topicDto = TopicData.GetTopicDtoWithoutTags();
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

        [Test]
        public void AddTagToTopic_WhenTopicNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", expectedTopicId)),
                async () => await _sut.AddTagToTopicAsync(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public void AddTagToTopic_WhenTagNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).ReturnsAsync(TopicData.GetTopicDtoWithTags);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "tag", expectedTagId)),
            async () => await _sut.AddTagToTopicAsync(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public async Task DeleteTagFromTopic_IntTopicIdAndTagId_DeleteTagFromTopicAsync()
        {
            //Given
            var topicId = 1;
            var tagId = 13;
            var expecectedAffectedRows = 1;

            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).ReturnsAsync(TopicData.GetTopicDtoWithTags());
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(TagData.GetTagDto());
            _topicRepoMock.Setup(x => x.DeleteTagFromTopicAsync(topicId, tagId)).ReturnsAsync(expecectedAffectedRows);

            //When
            var actualAffectedRows = await _sut.DeleteTagFromTopicAsync(topicId, tagId);

            //Than
            Assert.AreEqual(expecectedAffectedRows, actualAffectedRows);
            _topicRepoMock.Verify(x => x.DeleteTagFromTopicAsync(topicId, tagId), Times.Once);
        }
    }
}