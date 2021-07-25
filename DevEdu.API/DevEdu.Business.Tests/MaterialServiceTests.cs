using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class MaterialServiceTests
    {
        private Mock<IMaterialRepository> _materialRepositoryMock;
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<IGroupRepository> _groupRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
        }
        [Test]
        public void AddTagToMaterial_WithMaterialIdAndTopicId_Added()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 11;
            _materialRepositoryMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            var sut = new MaterialService(_materialRepositoryMock.Object, _courseRepositoryMock.Object, _groupRepositoryMock.Object);
            //When
            sut.AddTagToMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepositoryMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Once);
        }
        [Test]
        public void DeleteTagFromMaterial_WithMaterialIdAndTopicId_Deleted()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 11;
            _materialRepositoryMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            var sut = new MaterialService(_materialRepositoryMock.Object, _courseRepositoryMock.Object, _groupRepositoryMock.Object);
            //When
            sut.DeleteTagFromMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepositoryMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Once);
        }


    }
}
