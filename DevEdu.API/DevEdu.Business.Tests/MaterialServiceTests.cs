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
using System.Threading.Tasks;

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
                new CourseValidationHelper(_courseRepoMock.Object, _groupRepoMock.Object),
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
        public async Task GetAllMaterials_NoEntryForTeacherStudentOrTutorRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterialsAsync()).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetAllMaterialsAsync(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterialsAsync(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(groupsByMaterial.Count));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 3));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetAllMaterials_NoEntryForMethodistOrAdminRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterialsAsync()).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetAllMaterialsAsync(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterialsAsync(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task GetMaterialByIdWithCoursesAndGroups_ExistingMaterialId_MaterialDtoWithCoursesAndGroupsReturnedAsync()
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByMaterialId = GroupData.GetGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByMaterialId);

            //When
            var actualMaterial = await _sut.GetMaterialByIdWithCoursesAndGroupsAsync(expectedMaterial.Id);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialByIdWithCoursesAndGroupsAsync(material.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByGroups_MaterialDtoWithTagsReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdWithTagsAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetMaterialByIdWithTags_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByCourses_MaterialDtoWithTagsReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdWithTagsAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetMaterialByIdWithTags_ExistingMaterialIdForMethodistOrAdminRole_MaterialDtoWithTagsReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdWithTagsAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialByIdWithTags_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialByIdWithTagsAsync(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialByIdWithTags_ExistingMaterialIdNotAccessibleForTeacherStudentOrTutorRoleByCourses_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };
            var expectedMessage = string.Format(ServiceMessages.AccessToMaterialDenied, user.UserId, material.Id);

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(material.Id)).ReturnsAsync(new List<CourseDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(material.Id)).ReturnsAsync(new List<GroupDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(material.Id)).ReturnsAsync(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(material.Id)).ReturnsAsync(material);

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.GetMaterialByIdWithTagsAsync(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetMaterialsByTagId_ExistingTagIdAccessibleForTeacherStudentOrTutorRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tag.Id)).ReturnsAsync(tag);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetMaterialsByTagIdAsync(It.IsAny<int>())).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetMaterialsByTagIdAsync(tag.Id, user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(groupsByMaterial.Count));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(groupsByUser.Count * 3));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetMaterialsByTagId_ExistingTagIdForMethodistOrAdminRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { role } };

            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tag.Id)).ReturnsAsync(tag);
            _materialRepoMock.Setup(x => x.GetMaterialsByTagIdAsync(tag.Id)).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetMaterialsByTagIdAsync(tag.Id, user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialsByTagId_NotExistingTagId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var tag = TagData.GetListTagData()[0];
            var user = new UserIdentityInfo() { UserId = It.IsAny<int>(), Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(tag), tag.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialsByTagIdAsync(tag.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialsByTagIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task AddMaterialWithGroups_MaterialDtoListOfGroupsAndListOfTags_MaterialWithTagsAndGroupsCreatedAsync(Role role)
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
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role)).
                    ReturnsAsync(usersInGroup[i]);
                _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tags[i])).ReturnsAsync(tagDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithGroupsAsync(materialToAdd, tags, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(tags.Count));
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Exactly(tags.Count));

        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task AddMaterialWithGroups_MaterialDtoListOfGroups_MaterialWithoutTagsCreatedAsync(Role role)
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
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithGroupsAsync(materialToAdd, null, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);

        }

        [Test]
        public void AddMaterialWithGroups_ListOfGroupsWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var groups = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(groups));
            var user = new UserIdentityInfo() { UserId = 2 };

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
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
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "group", groups[2]);
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
            }
            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
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
            var expectedMessage = string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroup, user.UserId, groups[2], role.ToString());

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
            }
            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, null, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
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
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", tags[2]);

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
                _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tags[i])).ReturnsAsync(tagDtos[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, tags, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Exactly(tags.Count));

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
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(tags));

            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role)).
                    ReturnsAsync(usersInGroup[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, tags, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);

        }

        [Test]
        public async Task AddMaterialWithCourses_MaterialDtoListOfCoursesAndListOfTags_MaterialWithTagsAndCoursesCreatedAsync()
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
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
                _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tags[i])).ReturnsAsync(tagDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithCoursesAsync(materialToAdd, tags, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(tags.Count));
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Exactly(tags.Count));
        }

        [Test]
        public async Task AddMaterialWithCourses_MaterialDtoAndListOfCourses_MaterialWithCoursesCreatedAsync()
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithCoursesAsync(materialToAdd, null, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(courses));
            var user = new UserIdentityInfo() { UserId = 2 };

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithCoursesAsync(materialToAdd, null, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Never);
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithNotExistingCourse_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = new List<CourseDto> { new CourseDto { Id = 1 }, new CourseDto { Id = 2 }, null };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "course", courses[2]);
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddMaterialWithCoursesAsync(materialToAdd, null, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
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
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", tags[2]);
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
                _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tags[i])).ReturnsAsync(tagDtos[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddMaterialWithCoursesAsync(materialToAdd, tags, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Exactly(tags.Count));
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfTagsWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDtoWithoutTags();
            var courses = new List<int>() { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();
            var tags = new List<int>() { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(tags));
            var user = new UserIdentityInfo() { UserId = 2 };

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithCoursesAsync(materialToAdd, tags, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
            _tagRepoMock.Verify(x => x.SelectTagByIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task UpdateMaterial_MaterialIdMaterialDtoForTeacherRoleHappyFlow_UpdatedMaterialDtoReturnedAsync()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate.Groups);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).ReturnsAsync(groupsByUser);

            //When
            var actual = await _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user);

            //Then
            Assert.AreEqual(expectedMaterial, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task UpdateMaterial_MaterialIdMaterialDtoForMethodistRoleHappyFlow_UpdatedMaterialDtoReturnedAsync()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate.Courses);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actual = await _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user);

            //Then
            Assert.AreEqual(expectedMaterial, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialToUpdate.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateMaterial_MaterialDtoNotAccessibleToTeacherRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToUpdate.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).
                ReturnsAsync(new List<GroupDto>() { new GroupDto { Id = 7 }, new GroupDto { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToUpdate.Id)).
                ReturnsAsync(new List<GroupDto>() { new GroupDto { Id = 8 }, new GroupDto { Id = 18 } });

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_MaterialIdMaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithTags();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToUpdate.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task DeleteMaterial_MaterialIdForTeacherRoleHappyFlow_MaterialDeletedAsync()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete.Courses);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).ReturnsAsync(groupsByUser);

            //When
            await _sut.DeleteMaterialAsync(materialToDelete.Id, true, user);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public async Task DeleteMaterial_MaterialIdForMethodistRoleHappyFlow_MaterialDeletedAsync()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete.Groups);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete.Courses);

            //When
            await _sut.DeleteMaterialAsync(materialToDelete.Id, true, user);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialToDelete.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteMaterialAsync(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void DeleteMaterial_MaterialDtoNotAccessibleToTeacherRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToDelete.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).
                ReturnsAsync(new List<GroupDto>() { new GroupDto { Id = 7 }, new GroupDto { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToDelete.Id)).
                ReturnsAsync(new List<GroupDto>() { new GroupDto { Id = 22 }, new GroupDto { Id = 45 } });

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.DeleteMaterialAsync(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void DeleteMaterial_MaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithTagsCoursesAndGroups();
            var user = new UserIdentityInfo() { UserId = 2, Roles = new List<Role>() { Role.Methodist } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToDelete.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.DeleteMaterialAsync(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public async Task AddTagToMaterial_WithMaterialIdAndTagId_AddedAsync()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;

            _materialRepoMock.Setup(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId)).ReturnsAsync(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId)).ReturnsAsync(new MaterialDto { Id = givenMaterialId });

            //When
            await _sut.AddTagToMaterialAsync(givenMaterialId, givenTagId);

            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId), Times.Once);
        }

        [Test]
        public async Task DeleteTagFromMaterial_WithMaterialIdAndTagId_DeletedAsync()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            _materialRepoMock.Setup(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId)).ReturnsAsync(new TagDto { Id = givenTagId });
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId)).ReturnsAsync(new MaterialDto { Id = givenMaterialId });
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId));

            //When
            await _sut.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId);

            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId), Times.Once);
        }

        [Test]
        public void AddTagToMaterial_TagIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId)).ReturnsAsync(new MaterialDto { Id = givenMaterialId });

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTagToMaterialAsync(givenMaterialId, givenTagId));

            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }

        [Test]
        public void AddTagToMaterial_MaterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId)).ReturnsAsync(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTagToMaterialAsync(givenMaterialId, givenTagId));

            //Then
            _materialRepoMock.Verify(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }

        [Test]
        public void DeleteTagFromMaterial_TagIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "tag", givenTagId);
            _materialRepoMock.Setup(x => x.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId));
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId)).ReturnsAsync(new MaterialDto { Id = givenMaterialId });

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId));

            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId), Times.Never); ;
            Assert.That(result.Message, Is.EqualTo(exp));
        }

        [Test]
        public void DeleteTagToMaterial_MaterialIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenMaterialId = 5;
            var givenTagId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", givenMaterialId);
            _materialRepoMock.Setup(x => x.AddTagToMaterialAsync(givenMaterialId, givenTagId));
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(givenTagId)).ReturnsAsync(new TagDto { Id = givenTagId }); ;
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(givenMaterialId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _sut.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId));

            //Then
            _materialRepoMock.Verify(x => x.DeleteTagFromMaterialAsync(givenMaterialId, givenTagId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
    }
}