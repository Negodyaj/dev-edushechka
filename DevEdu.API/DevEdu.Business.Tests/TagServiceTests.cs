using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TagServiceTests
    {
        private Mock<ITagRepository> _tagRepoMock;

        [SetUp]
        public void Setup()
        {
            _tagRepoMock = new Mock<ITagRepository>();
        }

        [Test]
        public void AddTag_TagDto_TagCreated()
        {
            //Given
            var tagDto = TagData.GetListTagData()[0];
            var expectedTagId = tagDto.Id;

            _tagRepoMock.Setup(x => x.AddTag(tagDto)).Returns(expectedTagId);

            var sut = new TagService(_tagRepoMock.Object);

            //When
            var actualTagId = sut.AddTag(tagDto);

            //Than
            Assert.AreEqual(expectedTagId, actualTagId);
            _tagRepoMock.Verify(x => x.AddTag(tagDto), Times.Once);
        }

        [Test]
        public void GetAllTags_NoEntries_ListTagDto()
        {
            //Given
            var expectedTagDtos = TagData.GetListTagData();

            _tagRepoMock.Setup(x => x.SelectAllTags()).Returns(expectedTagDtos);

            var sut = new TagService(_tagRepoMock.Object);

            //When
            var actualTagDtos = sut.GetAllTags();

            //Than
            Assert.AreEqual(expectedTagDtos, actualTagDtos);
            _tagRepoMock.Verify(x => x.SelectAllTags(), Times.Once);
        }
        [Test]
        public void GetTagById_Id_TagDto()
        {
            //Given
            var expectedTagDto = TagData.GetListTagData()[0];

            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            var sut = new TagService(_tagRepoMock.Object);

            //When
            var actualTagDto = sut.GetTagById(tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
        }
        [Test]
        public void UpdateTag_TagDto_Id_TagDto()
        {
            //Given
            var expectedTagDto = TagData.GetListTagData()[0];

            var tagId = expectedTagDto.Id;

            _tagRepoMock.Setup(x => x.SelectTagById(tagId)).Returns(expectedTagDto);

            var sut = new TagService(_tagRepoMock.Object);

            //When
            var actualTagDto = sut.UpdateTag(TagData.GetListTagData()[2], tagId);

            //Than
            Assert.AreEqual(expectedTagDto, actualTagDto);
            _tagRepoMock.Verify(x => x.UpdateTag(It.Is<TagDto>(dto => dto.Equals(TagData.GetListTagData()[2]))), Times.Once);
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Once);
        }
    }
}