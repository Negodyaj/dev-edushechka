using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

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
        public void AddTag_TagDto_TagCreated()
        {
            //Given
            var inputTagDto = TagData.GetInputTagDto();
            var expectedTagDto = TagData.GetOutputTagDto();
            var expectedTagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.AddTagAsync(inputTagDto)).ReturnsAsync(expectedTagId);

            var sut = new TagService(_tagRepoMock.Object, _tagValidationHelper);

            //When
            var actualTag = sut.AddTagAsync(inputTagDto);

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
            Assert.Throws<EntityNotFoundException>(() => _sut.DeleteTagAsync(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
            _tagRepoMock.Verify(x => x.DeleteTagAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateTag_TagDto_Id_TagDtoUpdatedAndReturned()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var inputTagDto = TagData.GetTagDtoForUpdate();

            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            var actualTagDto = _sut.UpdateTagAsync(inputTagDto, tagId);

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
            Assert.Throws<EntityNotFoundException>(() => _sut.UpdateTagAsync(inputTagDto, tagId));

            //Than
            _tagRepoMock.Verify(x => x.UpdateTagAsync(It.IsAny<TagDto>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
        }

        [Test]
        public void GetAllTags_NoEntries_ListTagDtoReturned()
        {
            //Given
            var expectedTagDtos = TagData.GetListTagData();

            _tagRepoMock.Setup(x => x.SelectAllTagsAsync()).ReturnsAsync(expectedTagDtos);

            //When
            var actualTagDtos = _sut.GetAllTagsAsync();

            //Than
            Assert.AreEqual(expectedTagDtos, actualTagDtos);
            _tagRepoMock.Verify(x => x.SelectAllTagsAsync(), Times.Once);
        }

        [Test]
        public void GetTagById_Id_TagDtoReturned()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).ReturnsAsync(expectedTagDto);

            //When
            var actualTagDto = _sut.GetTagByIdAsync(tagId);

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
            Assert.Throws<EntityNotFoundException>(() => _sut.GetTagByIdAsync(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(tagId), Times.Once);
        }
    }
}