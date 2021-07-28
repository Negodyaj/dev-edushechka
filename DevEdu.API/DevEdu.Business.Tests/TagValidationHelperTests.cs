using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TagValidationHelperTests
    {
        private Mock<ITagRepository> _tagRepoMock;

        [SetUp]
        public void Setup()
        {
            _tagRepoMock = new Mock<ITagRepository>();
        }

        [Test]
        public void CheckTagExistence_TagId_EntityNotFoundException()
        {
            //Given
            TagDto tagDto = default;

            var tagId = 0;

            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(tagDto);

            var sut = new TagValidationHelper(_tagRepoMock.Object);

            //When

            //Than
            Assert.Throws<EntityNotFoundException>(() => sut.CheckTagExistence(tagId));
            _tagRepoMock.Verify(x => x.SelectTagById(tagId), Times.Exactly(1));
        }
    }
}
