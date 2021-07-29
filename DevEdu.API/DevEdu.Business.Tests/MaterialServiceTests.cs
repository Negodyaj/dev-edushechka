using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
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
        private Mock<ITagRepository> _tagRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _materialValidationHelperMock = new Mock<IMaterialValidationHelper>();
            _tagValidationHelperMock = new Mock<ITagValidationHelper>();
            _tagRepositoryMock = new Mock<ITagRepository>();
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
        public void AddTagToMaterial_WithMaterialIdAndTagId_Added()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var tags = MaterialData.GetTags();
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            sut.AddTagToMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Once);
        }
        [Test]
        public void DeleteTagFromMaterial_WithMaterialIdAndTagId_Deleted()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            sut.DeleteTagFromMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Once);
        }
        [Test]
        public void AddTagToMaterial_TagIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => 
            sut.AddTagToMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void AddTagToMaterial_MeterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId));
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.AddTagToMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void DeleteTagFromMaterial_TagIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Never);;
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void DeleteTagToMaterial_MeterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepositoryMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId));
            var sut = new MaterialService(_materialRepoMock.Object,
                                          _courseRepoMock.Object,
                                          _groupRepoMock.Object,
                                          new MaterialValidationHelper(_materialRepoMock.Object),
                                          new TagValidationHelper(_tagRepositoryMock.Object));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }

    }
}