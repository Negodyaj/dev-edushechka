using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    class MaterialServiceTests
    {
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
        }

        [Test]
        public void AddMaterial_MaterialDtoWithoutTags_MaterialCreated()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithoutTags();

            _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(MaterialData.expectedId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actualId = sut.AddMaterial(materialData);

            //Then
            Assert.AreEqual(MaterialData.expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterial_MaterialDtoWithTags_MaterialWithTagsCreated()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(MaterialData.expectedId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(MaterialData.expectedId, It.IsAny<int>()));

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actualId = sut.AddMaterial(materialData);

            //Then
            Assert.AreEqual(MaterialData.expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(MaterialData.expectedId, It.IsAny<int>()), Times.Exactly(3));
        }

        [Test]
        public void GetAllMaterials_NoEntry_MaterialsWithTagsReturned()
        {
            //Given
            var materialsData = MaterialData.GetListOfMaterials();

            _materialRepoMock.Setup(x => x.GetAllMaterials()).Returns(materialsData);

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actualList = sut.GetAllMaterials();

            //Then
            Assert.AreEqual(materialsData, actualList);
            _materialRepoMock.Verify(x => x.GetAllMaterials(), Times.Once);
        }

        [Test]
        public void GetMaterialById_MaterialId_MaterialWithTagsReturned()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.GetMaterialById(MaterialData.expectedId)).Returns(materialData);

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actual = sut.GetMaterialById(MaterialData.expectedId);

            //Then
            Assert.AreEqual(materialData, actual);
            _materialRepoMock.Verify(x => x.GetMaterialById(MaterialData.expectedId), Times.Once);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_MaterialId_MaterialWithTagsCoursesAndGroupsReturned()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithTags();
            var coursesData = MaterialData.GetListOfCourses();
            var groupsData = MaterialData.GetListOfGroups();

            _materialRepoMock.Setup(x => x.GetMaterialById(MaterialData.expectedId)).Returns(materialData);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(MaterialData.expectedId)).Returns(coursesData);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(MaterialData.expectedId)).Returns(groupsData);

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actual = sut.GetMaterialByIdWithCoursesAndGroups(MaterialData.expectedId);

            //Then
            Assert.AreEqual(materialData, actual);
            _materialRepoMock.Verify(x => x.GetMaterialById(MaterialData.expectedId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(MaterialData.expectedId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(MaterialData.expectedId), Times.Once);
        }

        [Test]
        public void UpdateMaterial_MaterialIdAndMaterialDto_UpdatedMaterialWithTagsReturned()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithTags();
            var expectedMaterialData = MaterialData.GetAnotherMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.UpdateMaterial(materialData));
            _materialRepoMock.Setup(x => x.GetMaterialById(materialData.Id)).Returns(expectedMaterialData);

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actual = sut.UpdateMaterial(materialData);

            //Then
            Assert.AreEqual(expectedMaterialData, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialData.Id), Times.Once);
        }

        [Test]
        public void GetMaterialsByTagId_TagId_MaterialsWithTagsReturned()
        {
            //Given
            var materialsData = MaterialData.GetListOfMaterials();

            _materialRepoMock.Setup(x => x.GetMaterialsByTagId(MaterialData.tagId)).Returns(materialsData);

            var sut = new MaterialService(_materialRepoMock.Object, _courseRepoMock.Object, _groupRepoMock.Object);

            //When
            var actualList = sut.GetMaterialsByTagId(MaterialData.tagId);

            //Then
            Assert.AreEqual(materialsData, actualList);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagId(MaterialData.tagId), Times.Once);
        }
    }
}
