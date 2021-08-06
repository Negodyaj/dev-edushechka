using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    class MaterialServiceTests
    {
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<ITagRepository> _tagRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private MaterialService _sut;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            _sut = new MaterialService(
                _materialRepoMock.Object,
                _courseRepoMock.Object,
                _groupRepoMock.Object,
                new GroupValidationHelper(_groupRepoMock.Object),
                new TagValidationHelper(_tagRepoMock.Object),
                new CourseValidationHelper(_courseRepoMock.Object),
                new MaterialValidationHelper(
                    _materialRepoMock.Object,
                    _groupRepoMock.Object,
                    _courseRepoMock.Object),
                new UserValidationHelper(_userRepoMock.Object)
                );
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetAllMaterials_NoEntryForTeacherStudentOrTutor_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupsDtos();
            var groupsByUser = GroupData.GetAnotherGroupsDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterials()).Returns(expectedMaterials);

            //When
            var actualMaterials = _sut.GetAllMaterials(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterials(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(groupsByMaterial.Count));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 2));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public void GetAllMaterials_NoEntryForMethodistOrAdmin_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupsDtos();
            var groupsByUser = GroupData.GetAnotherGroupsDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterials()).Returns(expectedMaterials);

            //When
            var actualMaterials = _sut.GetAllMaterials(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterials(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_ExistingMaterialId_MaterialDtoWithCoursesAndGroupsReturned()
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByMaterialId = GroupData.GetGroupsDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();

            _materialRepoMock.Setup(x => x.GetMaterialById(It.IsAny<int>())).Returns(expectedMaterial);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(groupsByMaterialId);

            //When
            var actualMaterial = _sut.GetMaterialByIdWithCoursesAndGroups(expectedMaterial.Id);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetMaterialByIdWithCoursesAndGroups(material.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorByGroups_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTags();
            var groupsByMaterial = GroupData.GetGroupsDtos();
            var groupsByUser = GroupData.GetAnotherGroupsDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialById(It.IsAny<int>())).Returns(expectedMaterial);

            //When
            var actualMaterial = _sut.GetMaterialByIdWithTags(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorByCourses_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTags();
            var groupsByUser = GroupData.GetAnotherGroupsDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(groupsByUser);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialById(It.IsAny<int>())).Returns(expectedMaterial);

            //When
            var actualMaterial = _sut.GetMaterialByIdWithTags(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public void GetMaterialByIdWithTags_ExistingMaterialIdForMethodistOrAdmin_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTags();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _materialRepoMock.Setup(x => x.GetMaterialById(It.IsAny<int>())).Returns(expectedMaterial);

            //When
            var actualMaterial = _sut.GetMaterialByIdWithTags(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialByIdWithTags_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTags();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetMaterialByIdWithTags(material.Id, user)); 

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialByIdWithTags_ExistingMaterialIdNotAccessibleForTeacherStudentOrTutorByCourses_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTags();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };
            var expectedMessage = string.Format(ServiceMessages.AccessToMaterialDenied, user.UserId, material.Id);

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(new List<CourseDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(new List<GroupDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialById(It.IsAny<int>())).Returns(material);

            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.GetMaterialByIdWithTags(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialsByTagId_ExistingTagIdAccessibleForTeacherStudentOrTutor_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var groupsByMaterial = GroupData.GetGroupsDtos();
            var groupsByUser = GroupData.GetAnotherGroupsDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(tag);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(It.IsAny<int>())).Returns(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(It.IsAny<int>())).Returns(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(It.IsAny<int>())).Returns(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetMaterialsByTagId(It.IsAny<int>())).Returns(expectedMaterials);

            //When
            var actualMaterials = _sut.GetMaterialsByTagId(tag.Id, user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(groupsByMaterial.Count));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 2));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public void GetMaterialsByTagId_ExistingTagIdForMethodistOrAdmin_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(tag);
            _materialRepoMock.Setup(x => x.GetMaterialsByTagId(It.IsAny<int>())).Returns(expectedMaterials);

            //When
            var actualMaterials = _sut.GetMaterialsByTagId(tag.Id, user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialsByTagId_NotExistingTagId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTags();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(tag), tag.Id);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetMaterialsByTagId(tag.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
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




        //[Test]
        //public void UpdateMaterial_MaterialIdAndMaterialDto_UpdatedMaterialWithTagsReturned()
        //{
        //    //Given
        //    var materialData = MaterialData.GetMaterialDtoWithTags();
        //    var expectedMaterialData = MaterialData.GetAnotherMaterialDtoWithTags();

        //    _materialRepoMock.Setup(x => x.UpdateMaterial(materialData));
        //    _materialRepoMock.Setup(x => x.GetMaterialById(materialData.Id)).Returns(expectedMaterialData);

        //    //When
        //    var actual = _sut.UpdateMaterial(materialData.Id, materialData);

        //    //Then
        //    Assert.AreEqual(expectedMaterialData, actual);
        //    _materialRepoMock.Verify(x => x.UpdateMaterial(materialData), Times.Once);
        //    _materialRepoMock.Verify(x => x.GetMaterialById(materialData.Id), Times.Once);
        //}

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