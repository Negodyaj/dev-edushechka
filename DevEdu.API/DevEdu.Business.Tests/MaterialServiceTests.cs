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
    public class MaterialServiceTests
    {
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IMaterialRepository> _materialRepoMock;
        private MaterialService _sut;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();

            _sut = new MaterialService(
                _materialRepoMock.Object,
                new MaterialValidationHelper(_materialRepoMock.Object),
                new CourseValidationHelper(_courseRepoMock.Object, _groupRepoMock.Object)
            );
        }

        [Test]
        public async Task UpdateMaterialAsyncTest()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDto();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDto();

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(new MaterialDtoWithCourseId());
            _materialRepoMock.Setup(x => x.UpdateMaterialAsync(It.IsAny<MaterialDto>()));

            //When
            await _sut.UpdateMaterialAsync(materialToUpdate.Id, expectedMaterial);

            //Then
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(expectedMaterial), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterialAsyncNegativeNotFoundExceptionTest()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDto();
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialToUpdate.Id);
            
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync((MaterialDtoWithCourseId)null);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task DeleteMaterialAsyncTest()
        {
            //Given
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync(new MaterialDtoWithCourseId());

            //When
            await _sut.DeleteMaterialAsync(5);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteOrRestoreMaterialAsync(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", 5);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync((MaterialDtoWithCourseId)null);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteMaterialAsync(5));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.DeleteOrRestoreMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public async Task RestoreMaterialAsyncTest()
        {
            //Given
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync(new MaterialDtoWithCourseId());

            //When
            await _sut.RestoreMaterialAsync(5);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteOrRestoreMaterialAsync(It.IsAny<int>(), false), Times.Once);
        }

        [Test]
        public void RestoreMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", 5);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync((MaterialDtoWithCourseId)null);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.RestoreMaterialAsync(5));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.DeleteOrRestoreMaterialAsync(It.IsAny<int>(), false), Times.Never);
        }
    }
}