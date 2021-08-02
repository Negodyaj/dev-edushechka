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

            _tagRepoMock.Setup(x => x.AddTag(inputTagDto)).Returns(expectedTagId);

            var sut = new TagService(_tagRepoMock.Object, _tagValidationHelper);

            //When
            var actualTag = sut.AddTag(inputTagDto);

            //Than
            Assert.AreEqual(expectedTagDto, actualTag);
            _tagRepoMock.Verify(x => x.AddTag(expectedTagDto), Times.Once);
        }

        [Test]
        public void DeleteTag_TagId_TagDeleted()
        {
            //Given
            var tagDto = TagData.GetOutputTagDto();
            var tagId = tagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(tagDto);

            //When
            _sut.DeleteTag(tagId);

            //Than
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
            _tagRepoMock.Verify(x => x.DeleteTag(tagId), Times.Once);
        }

        [Test]
        public void DeleteTag_TagDoesntExist_EntityNotFoundException()
        {
            //Given
            TagDto tagDto = default;
            var tagId = TagData.GetOutputTagDto().Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(tagDto);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.DeleteTag(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
            _tagRepoMock.Verify(x => x.DeleteTag(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateTag_TagDto_Id_TagDto()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var inputTagDto = TagData.GetTagDtoForUpdate();

            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            //When
            var actualTagDto = _sut.UpdateTag(inputTagDto, tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.UpdateTag(It.Is<TagDto>(dto => dto.Equals(expectedTagDto))), Times.Once);
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Exactly(2));
        }

        [Test]
        public void UpdateTag_TagDoesntExist_EntityNotFoundException()
        {
            //Given
            TagDto expectedTagDto = default;
            var inputTagDto = TagData.GetTagDtoForUpdate();
            var tagId = TagData.GetOutputTagDto().Id;


            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.UpdateTag(inputTagDto, tagId));

            //Than
            _tagRepoMock.Verify(x => x.UpdateTag(It.IsAny<TagDto>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
        }

        [Test]
        public void GetAllTags_NoEntries_ListTagDto()
        {
            //Given
            var expectedTagDtos = TagData.GetListTagDto();

            _tagRepoMock.Setup(x => x.SelectAllTags()).Returns(expectedTagDtos);

            //When
            var actualTagDtos = _sut.GetAllTags();

            //Than
            Assert.AreEqual(expectedTagDtos, actualTagDtos);
            _tagRepoMock.Verify(x => x.SelectAllTags(), Times.Once);
        }

        [Test]
        public void GetTagById_Id_TagDto()
        {
            //Given
            var expectedTagDto = TagData.GetOutputTagDto();
            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            //When
            var actualTagDto = _sut.GetTagById(tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
        }

        [Test]
        public void GetTagById_TagDoesntExist_EntityNotFoundException()
        {
            //Given
            TagDto expectedTagDto = default;
            var tagId = 0;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.GetTagById(tagId));

            //Than
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
        }        
    }
}