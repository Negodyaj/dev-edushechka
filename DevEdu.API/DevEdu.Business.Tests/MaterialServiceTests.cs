using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class MaterialServiceTests
    {
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IMaterialValidationHelper> _materialValidationHelperMock;
        private Mock<ITagValidationHelper> _tagValidationHelperMock;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _materialValidationHelperMock = new Mock<IMaterialValidationHelper>();
            _tagValidationHelperMock = new Mock<ITagValidationHelper>();
        }

        [Test]
        public void AddMaterial_MaterialDtoWithoutTags_MaterialCreated()
        {
            //Given
            var expectedId = 66;
            var materialData = MaterialData.GetMaterialDtoWithoutTags();

            _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(expectedId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new MaterialService(_materialRepoMock.Object, 
                                          _courseRepoMock.Object, 
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actualId = sut.AddMaterial(materialData);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterial_MaterialDtoWithTags_MaterialWithTagsCreated()
        {
            //Given
            var expectedId = 66;
            var materialData = MaterialData.GetMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(expectedId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(expectedId, It.IsAny<int>()));

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actualId = sut.AddMaterial(materialData);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(expectedId, It.IsAny<int>()), Times.Exactly(materialData.Tags.Count));
        }

        [Test]
        public void GetAllMaterials_NoEntry_MaterialsWithTagsReturned()
        {
            //Given
            var materialsData = MaterialData.GetListOfMaterials();

            _materialRepoMock.Setup(x => x.GetAllMaterials()).Returns(materialsData);

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

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
            var materialId = 66;
            var materialData = MaterialData.GetMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialData);

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actual = sut.GetMaterialById(materialId);

            //Then
            Assert.AreEqual(materialData, actual);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_MaterialId_MaterialWithTagsCoursesAndGroupsReturned()
        {
            //Given
            var materialId = 66;
            var materialData = MaterialData.GetMaterialDtoWithTags();
            var coursesData = MaterialData.GetListOfCourses();
            var groupsData = MaterialData.GetListOfGroups();

            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialData);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(materialId)).Returns(coursesData);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialId)).Returns(groupsData);

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actual = sut.GetMaterialByIdWithCoursesAndGroups(materialId);

            //Then
            Assert.AreEqual(materialData, actual);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(materialId), Times.Once);
        }

        [Test]
        public void UpdateMaterial_MaterialIdAndMaterialDto_UpdatedMaterialWithTagsReturned()
        {
            //Given
            var materialData = MaterialData.GetMaterialDtoWithTags();
            var expectedMaterialData = MaterialData.GetAnotherMaterialDtoWithTags();

            _materialRepoMock.Setup(x => x.UpdateMaterial(materialData));
            _materialRepoMock.Setup(x => x.GetMaterialById(materialData.Id)).Returns(expectedMaterialData);

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actual = sut.UpdateMaterial(materialData.Id, materialData);

            //Then
            Assert.AreEqual(expectedMaterialData, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialData), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialData.Id), Times.Once);
        }

        [Test]
        public void GetMaterialsByTagId_TagId_MaterialsWithTagsReturned()
        {
            //Given
            var tagId = 66;
            var materialsData = MaterialData.GetListOfMaterials();

            _materialRepoMock.Setup(x => x.GetMaterialsByTagId(tagId)).Returns(materialsData);

            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);

            //When
            var actualList = sut.GetMaterialsByTagId(tagId);

            //Then
            Assert.AreEqual(materialsData, actualList);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagId(tagId), Times.Once);
        }

        [Test]
        public void AddTagToMaterial_WithMaterialIdAndTopicId_Added()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 11;
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);
            //When
            sut.AddTagToMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Once);
        }

        [Test]
        public void DeleteTagFromMaterial_WithMaterialIdAndTopicId_Deleted()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 11;
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          _materialValidationHelperMock.Object,
                                          _tagValidationHelperMock.Object);
            //When
            sut.DeleteTagFromMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Once);
        }
    }
}