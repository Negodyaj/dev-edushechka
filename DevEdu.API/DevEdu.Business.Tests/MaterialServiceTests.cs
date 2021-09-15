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
    public class MaterialServiceTests
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
        public void GetAllMaterials_NoEntryForTeacherStudentOrTutorRole_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
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
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 3));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public void GetAllMaterials_NoEntryForMethodistOrAdminRole_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
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
            var groupsByMaterialId = GroupData.GetGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();

            _materialRepoMock.Setup(x => x.GetMaterialById(expectedMaterial.Id)).Returns(expectedMaterial);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(expectedMaterial.Id)).Returns(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(expectedMaterial.Id)).Returns(groupsByMaterialId);

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
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(material), material.Id);

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
        public void GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByGroups_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(expectedMaterial.Id)).Returns(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedMaterial.Id)).Returns(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialById(expectedMaterial.Id)).Returns(expectedMaterial);

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
        public void GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByCourses_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(expectedMaterial.Id)).Returns(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedMaterial.Id)).Returns(groupsByUser);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(expectedMaterial.Id)).Returns(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialById(expectedMaterial.Id)).Returns(expectedMaterial);

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
        public void GetMaterialByIdWithTags_ExistingMaterialIdForMethodistOrAdminRole_MaterialDtoWithTagsReturned(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
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
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(material), material.Id);

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
        public void GetMaterialByIdWithTags_ExistingMaterialIdNotAccessibleForTeacherStudentOrTutorRoleByCourses_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };
            var expectedMessage = string.Format(ServiceMessages.AccessToMaterialDeniedMessage, user.UserId, material.Id);

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(material.Id)).Returns(new List<CourseDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(material.Id)).Returns(new List<GroupDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(material.Id)).Returns(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialById(material.Id)).Returns(material);

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
        public void GetMaterialsByTagId_ExistingTagIdAccessibleForTeacherStudentOrTutorRole_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagById(tag.Id)).Returns(tag);
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
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 3));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public void GetMaterialsByTagId_ExistingTagIdForMethodistOrAdminRole_ListOfMaterialDtoReturned(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagById(tag.Id)).Returns(tag);
            _materialRepoMock.Setup(x => x.GetMaterialsByTagId(tag.Id)).Returns(expectedMaterials);

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
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(tag), tag.Id);

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

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_MaterialDtoListOfGroupsAndListOfTags_MaterialWithTagsAndGroupsCreated(Role role)
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var tags = new List<int>() { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();
            var tagDtos = TagData.GetListOfTags();

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
                _tagRepoMock.Setup(x => x.SelectTagById(tags[i])).Returns(tagDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterial(materialToAdd)).Returns(expectedId);

            //When
            int actualId = _sut.AddMaterialWithGroups(materialToAdd, tags, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(tags.Count));
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Exactly(tags.Count));

        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_MaterialDtoListOfGroups_MaterialWithoutTagsCreated(Role role)
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterial(materialToAdd)).Returns(expectedId);

            //When
            int actualId = _sut.AddMaterialWithGroups(materialToAdd, null, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);

        }

        [Test]
        public void AddMaterialWithGroups_ListOfGroupsWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvidedMessage, nameof(groups));
            var user = new UserIdentityInfo() { UserId = 2 };

            //When
            var actual = Assert.Throws<ValidationException>(
                () => _sut.AddMaterialWithGroups(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_ListOfGroupsWithNotExistingGroup_EntityNotFoundExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var groupDtos = new List<GroupDto> { new GroupDto { Id = 1 }, new GroupDto { Id = 2 }, null };
            var usersInGroup = UserData.GetListsOfUsersInGroup();
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "group", groups[2]);
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
            }
            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddMaterialWithGroups(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_UserDoesNotBelongToGroup_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetAnotherListsOfUsersInGroup();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };
            var expectedMessage = string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroupMessage, user.UserId, groups[2], role.ToString());

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
            }
            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.AddMaterialWithGroups(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_ListOfTagsWithNotExistingTag_EntityNotFoundExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var tags = new List<int>() { 1, 2, 3 };
            var tagDtos = new List<TagDto> { new TagDto { Id = 1 }, new TagDto { Id = 2 }, null };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "tag", tags[2]);

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
                _tagRepoMock.Setup(x => x.SelectTagById(tags[i])).Returns(tagDtos[i]);
            }

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddMaterialWithGroups(materialToAdd, tags, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Exactly(tags.Count));

        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_ListOfTagsWithDuplicateValues_ValidationExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 3 };
            var tags = new List<int>() { 1, 2, 2 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvidedMessage, nameof(tags));

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroup(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groups[i], (int)role)).
                    Returns(usersInGroup[i]);
            }

            //When
            var actual = Assert.Throws<ValidationException>(
                () => _sut.AddMaterialWithGroups(materialToAdd, tags, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);

        }

        [Test]
        public void AddMaterialWithCourses_MaterialDtoListOfCoursesAndListOfTags_MaterialWithTagsAndCoursesCreated()
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var tags = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var tagDtos = TagData.GetListOfTags();
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourse(courses[i])).Returns(courseDtos[i]);
                _tagRepoMock.Setup(x => x.SelectTagById(tags[i])).Returns(tagDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterial(materialToAdd)).Returns(expectedId);

            //When
            int actualId = _sut.AddMaterialWithCourses(materialToAdd, tags, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(tags.Count));
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Exactly(tags.Count));
        }

        [Test]
        public void AddMaterialWithCourses_MaterialDtoAndListOfCourses_MaterialWithCoursesCreated()
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourse(courses[i])).Returns(courseDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterial(materialToAdd)).Returns(expectedId);

            //When
            int actualId = _sut.AddMaterialWithCourses(materialToAdd, null, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Once);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvidedMessage, nameof(courses));
            var user = new UserIdentityInfo() { UserId = 2 };

            //When
            var actual = Assert.Throws<ValidationException>(
                () => _sut.AddMaterialWithCourses(materialToAdd, null, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithNotExistingCourse_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = new List<CourseDto> { new CourseDto { Id = 1 }, new CourseDto { Id = 2 }, null };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "course", courses[2]);
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourse(courses[i])).Returns(courseDtos[i]);
            }

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddMaterialWithCourses(materialToAdd, null, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfTagsWithNotExistingTag_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var tags = new List<int>() { 1, 2, 3 };
            var tagDtos = new List<TagDto> { new TagDto { Id = 1 }, new TagDto { Id = 2 }, null };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "tag", tags[2]);
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourse(courses[i])).Returns(courseDtos[i]);
                _tagRepoMock.Setup(x => x.SelectTagById(tags[i])).Returns(tagDtos[i]);
            }

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddMaterialWithCourses(materialToAdd, tags, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Exactly(tags.Count));
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfTagsWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var tags = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvidedMessage, nameof(tags));
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourse(courses[i])).Returns(courseDtos[i]);
            }

            //When
            var actual = Assert.Throws<ValidationException>(
                () => _sut.AddMaterialWithCourses(materialToAdd, tags, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterial(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReference(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourse(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagById(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterial(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateMaterial_MaterialIdMaterialDtoForTeacherRoleHappyFlow_UpdatedMaterialDtoReturned()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToUpdate.Id)).Returns(materialToUpdate);
            _materialRepoMock.Setup(x => x.GetMaterialById(expectedMaterial.Id)).Returns(expectedMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToUpdate.Id)).Returns(materialToUpdate.Groups);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.UserId)).Returns(groupsByUser);

            //When
            var actual = _sut.UpdateMaterial(materialToUpdate.Id, materialToUpdate, user);

            //Then
            Assert.AreEqual(expectedMaterial, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialToUpdate), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_MaterialIdMaterialDtoForMethodistRoleHappyFlow_UpdatedMaterialDtoReturned()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToUpdate.Id)).Returns(materialToUpdate.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(materialToUpdate.Id)).Returns(materialToUpdate.Courses);
            _materialRepoMock.Setup(x => x.GetMaterialById(materialToUpdate.Id)).Returns(materialToUpdate);
            _materialRepoMock.Setup(x => x.GetMaterialById(expectedMaterial.Id)).Returns(expectedMaterial);

            //When
            var actual = _sut.UpdateMaterial(materialToUpdate.Id, materialToUpdate, user);

            //Then
            Assert.AreEqual(expectedMaterial, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialToUpdate), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "material", materialToUpdate.Id);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.UpdateMaterial(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateMaterial_MaterialDtoNotAccessibleToTeacherRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDeniedMessage, user.UserId, materialToUpdate.Id);

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToUpdate.Id)).Returns(materialToUpdate);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.UserId)).
                Returns(new List<GroupDto>() { new GroupDto { Id = 7 }, new GroupDto { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToUpdate.Id)).
                Returns(new List<GroupDto>() { new GroupDto { Id = 8 }, new GroupDto { Id = 18 } });

            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.UpdateMaterial(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_MaterialIdMaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTags();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDeniedMessage, user.UserId, materialToUpdate.Id);

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToUpdate.Id)).Returns(materialToUpdate);

            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.UpdateMaterial(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterial(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void DeleteMaterial_MaterialIdForTeacherRoleHappyFlow_MaterialDeleted()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToDelete.Id)).Returns(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToDelete.Id)).Returns(materialToDelete.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(materialToDelete.Id)).Returns(materialToDelete.Courses);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.UserId)).Returns(groupsByUser);

            //When
            _sut.DeleteMaterial(materialToDelete.Id, true, user);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(2));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterial(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteMaterial_MaterialIdForMethodistRoleHappyFlow_MaterialDeleted()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToDelete.Id)).Returns(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToDelete.Id)).Returns(materialToDelete.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialId(materialToDelete.Id)).Returns(materialToDelete.Courses);

            //When
            _sut.DeleteMaterial(materialToDelete.Id, true, user);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterial(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "material", materialToDelete.Id);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteMaterial(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.DeleteMaterial(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void DeleteMaterial_MaterialDtoNotAccessibleToTeacherRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDeniedMessage, user.UserId, materialToDelete.Id);

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToDelete.Id)).Returns(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.UserId)).
                Returns(new List<GroupDto>() { new GroupDto { Id = 7 }, new GroupDto { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialId(materialToDelete.Id)).
                Returns(new List<GroupDto>() { new GroupDto { Id = 22 }, new GroupDto { Id = 45 } });

            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.DeleteMaterial(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterial(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void DeleteMaterial_MaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDeniedMessage, user.UserId, materialToDelete.Id);

            _materialRepoMock.Setup(x => x.GetMaterialById(materialToDelete.Id)).Returns(materialToDelete);

            //When
            var actual = Assert.Throws<AuthorizationException>(
                () => _sut.DeleteMaterial(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialId(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialId(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterial(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void AddTagToMaterial_WithMaterialIdAndTagId_Added()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            //When
            _sut.AddTagToMaterial(givenMaterialId, givenTagId);
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
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            //When
            _sut.DeleteTagFromMaterial(givenMaterialId, givenTagId);
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Once);
        }

        [Test]
        public void AddTagToMaterial_TagIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });

            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTagToMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterial(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }

        [Test]
        public void AddTagToMaterial_MaterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId));

            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTagToMaterial(givenMaterialId, givenTagId));
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
            var exp = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId)).Returns(new MaterialDto { Id = givenMaterialId });

            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Never); ;
            Assert.That(result.Message, Is.EqualTo(exp));
        }

        [Test]
        public void DeleteTagToMaterial_MaterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterial(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagById(givenTagId)).Returns(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialById(givenMaterialId));

            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.DeleteTagFromMaterial(givenMaterialId, givenTagId));
            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterial(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
    }
}