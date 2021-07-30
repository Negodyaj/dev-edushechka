using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
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
        private Mock<IGroupValidationHelper> _groupValidationHelper;
        private Mock<ITagValidationHelper> _tagValidationHelper; 
        private MaterialService _sut;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _groupValidationHelper = new Mock<IGroupValidationHelper>();
            _tagValidationHelper = new Mock<ITagValidationHelper>();
            _sut = new MaterialService(
                _materialRepoMock.Object, 
                _courseRepoMock.Object, 
                _groupRepoMock.Object,
                _groupValidationHelper.Object,
                _tagValidationHelper.Object);
        }

        //[Test]
        //public void AddMaterial_MaterialDtoWithoutTags_MaterialCreated()
        //{
        //    //Given
        //    var expectedId = 66;
        //    var materialData = MaterialData.GetMaterialDtoWithoutTags();

        //    _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(expectedId);
        //    _materialRepoMock.Setup(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()));

        //    //When
        //    var actualId = _sut.AddMaterial(materialData);

        //    //Then
        //    Assert.AreEqual(expectedId, actualId);
        //    _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
        //    _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        //}

        //[Test]
        //public void AddMaterial_MaterialDtoWithTags_MaterialWithTagsCreated()
        //{
        //    //Given
        //    var expectedId = 66;
        //    var materialData = MaterialData.GetMaterialDtoWithTags();

        //    _materialRepoMock.Setup(x => x.AddMaterial(materialData)).Returns(expectedId);
        //    _materialRepoMock.Setup(x => x.AddTagToMaterial(expectedId, It.IsAny<int>()));

        //    //When
        //    var actualId = _sut.AddMaterial(materialData);

        //    //Then
        //    Assert.AreEqual(expectedId, actualId);
        //    _materialRepoMock.Verify(x => x.AddMaterial(materialData), Times.Once);
        //    _materialRepoMock.Verify(x => x.AddTagToMaterial(expectedId, It.IsAny<int>()), Times.Exactly(materialData.Tags.Count));
        //}

        [Test]
        public void GetAllMaterials_NoEntry_MaterialsWithTagsReturned()
        {
            //Given
            var materialsData = MaterialData.GetListOfMaterials();

            _materialRepoMock.Setup(x => x.GetAllMaterials()).Returns(materialsData);

            //When
            var actualList = _sut.GetAllMaterials();

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

            //When
            var actual = _sut.GetMaterialById(materialId);

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

            //When
            var actual = _sut.GetMaterialByIdWithCoursesAndGroups(materialId);

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

            //When
            var actual = _sut.UpdateMaterial(materialData.Id, materialData);

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

            //When
            var actualList = _sut.GetMaterialsByTagId(tagId);

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

            //When
            _sut.AddTagToMaterial(givenMaterialId, givenTagId);
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

            //When
            _sut.DeleteTagFromMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Once);
        }
    }
}