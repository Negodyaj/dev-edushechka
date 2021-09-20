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
    public class TagServiceTests
    {
        private Mock<ITagRepository> _tagRepoMock;
        private ITagValidationHelper _tagValidationHelper;
        private ITagService _sut;

        [SetUp]
        public void Setup()
        {
            _tagRepoMock = new Mock<ITagRepository>();
            _tagValidationHelper = new TagValidationHelper(_tagRepoMock.Object);
            _sut = new TagService(_tagRepoMock.Object, _tagValidationHelper);
        }

        [Test]
        public async System.Threading.Tasks.Task AddTag_TagDto_TagCreatedAsync()
        {
            //Given
            var inputTagDto = TagData.GetInputTagDto();
            var expectedTagDto = TagData.GetOutputTagDto();
            var expectedTagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.AddTagAsync(inputTagDto)).ReturnsAsync(expectedTagId);

            //When
            var actualTag = await _sut.AddTagAsync(inputTagDto);

            //Than
            Assert.AreEqual(expectedTagDto, actualTag);
            _tagRepoMock.Verify(x => x.AddTagAsync(expectedTagDto), Times.Once);
        }

        [Test]
        public void DeleteTag_TagId_TagDeleted()
        {
            //Given
            var tagDto = TagData.GetOutputTagDto();
            var tagId = tagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(tagDto);

            //When
            _sut.DeleteTagAsync(tagId);

            //Than
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
            _tagRepoMock.Verify(x => x.DeleteTagAsync(tagId), Times.Once);
        }

        [Test]
        public void DeleteTag_TagDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            TagDto tagDto = default;
            var tagId = TagData.GetOutputTagDto().Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(tagDto);

            //When
            Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.DeleteTagAsync(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
            _tagRepoMock.Verify(x => x.DeleteTagAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task UpdateTag_TagDto_Id_TagDtoUpdatedAndReturnedAsync()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var inputTagDto = TagData.GetTagDtoForUpdate();

            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            var actualTagDto =await _sut.UpdateTagAsync(inputTagDto, tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.UpdateTagAsync(It.Is<TagDto>(dto => dto.Equals(expectedTagDto))), Times.Once);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Exactly(2));
        }

        [Test]
        public void UpdateTag_TagDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            TagDto expectedTagDto = default;
            var inputTagDto = TagData.GetTagDtoForUpdate();
            var tagId = TagData.GetOutputTagDto().Id;


            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.UpdateTagAsync(inputTagDto, tagId));

            //Than
            _tagRepoMock.Verify(x => x.UpdateTagAsync(It.IsAny<TagDto>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
        }

        [Test]
        public async Task GetAllTags_NoEntries_ListTagDtoReturnedAsync()
        {
            //Given
            var expectedTagDtos = TagData.GetListTagData();

            _tagRepoMock.Setup(x => x.SelectAllTagsAsync()).ReturnsAsync(expectedTagDtos);

            //When
            var actualTagDtos = await _sut.GetAllTagsAsync();

            //Than
            Assert.AreEqual(expectedTagDtos, actualTagDtos);
            _tagRepoMock.Verify(x => x.SelectAllTagsAsync(), Times.Once);
        }

        [Test]
        public async Task GetTagById_Id_TagDtoReturnedAsync()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            var actualTagDto = await _sut.GetTagByIdAsync(tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
        }

        [Test]
        public void GetTagById_TagDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            TagDto expectedTagDto = default;
            var tagId = 0;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetTagByIdAsync(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
        }
    }
}