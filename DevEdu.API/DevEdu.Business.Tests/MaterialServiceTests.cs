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
        private Mock<IUserRepository> _userRepoMock;
        private MaterialService _sut;

        [SetUp]
        public void SetUp()
        {
            _materialRepoMock = new Mock<IMaterialRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            _sut = new MaterialService(
                _materialRepoMock.Object,
                _courseRepoMock.Object,
                _groupRepoMock.Object,
                new GroupValidationHelper(_groupRepoMock.Object),
                new CourseValidationHelper(_courseRepoMock.Object, _groupRepoMock.Object),
                new MaterialValidationHelper(_materialRepoMock.Object, _groupRepoMock.Object, _courseRepoMock.Object),
                new UserValidationHelper(_userRepoMock.Object)
            );
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetAllMaterials_NoEntryForTeacherStudentOrTutorRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

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
            var expectedMaterials = MaterialData.GetListOfMaterialsWithCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

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
            var expectedMaterial = MaterialData.GetMaterialDtoWithCoursesAndGroups();
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
            var material = MaterialData.GetMaterialDtoWithCoursesAndGroups();
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
        public async Task GetMaterialById_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByGroups_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var groupsByMaterial = GroupData.GetGroupDtos();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

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
        public async Task GetMaterialById_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByCourses_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetMaterialById_ExistingMaterialIdForMethodistOrAdminRole_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialById_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialByIdAsync(material.Id, user));

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
        public void GetMaterialById_ExistingMaterialIdNotAccessibleForTeacherStudentOrTutorRoleByCourses_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };
            var expectedMessage = string.Format(ServiceMessages.AccessToMaterialDenied, user.UserId, material.Id);

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(material.Id)).ReturnsAsync(new List<CourseDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(material.Id)).ReturnsAsync(new List<GroupDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(material.Id)).ReturnsAsync(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(material.Id)).ReturnsAsync(material);

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(() => _sut.GetMaterialByIdAsync(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task AddMaterialWithGroups_MaterialDtoListOfGroups_MaterialAndGroupsCreatedAsync(Role role)
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDto();
            var groups = new List<int> { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();

            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role)).
                    ReturnsAsync(usersInGroup[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithGroupsAsync(materialToAdd, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task AddMaterialWithGroups_MaterialDtoListOfGroups_MaterialCreatedAsync(Role role)
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDto();
            var groups = new List<int> { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetListsOfUsersInGroup();

            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithGroupsAsync(materialToAdd, groups, user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
        }

        [Test]
        public void AddMaterialWithGroups_ListOfGroupsWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDto();
            var groups = new List<int> { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(groups));
            var user = new UserIdentityInfo { UserId = 2 };

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_ListOfGroupsWithNotExistingGroup_EntityNotFoundExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDto();
            var groups = new List<int> { 1, 2, 3 };
            var groupDtos = new List<GroupDto> { new() { Id = 1 }, new() { Id = 2 }, null };
            var usersInGroup = UserData.GetListsOfUsersInGroup();
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "group", groups[2]);
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { role } };

            for (int i = 0; i < groups.Count; i++)
            {
                _groupRepoMock.Setup(x => x.GetGroupAsync(groups[i])).ReturnsAsync(groupDtos[i]);
                _userRepoMock
                    .Setup(x => x.GetUsersByGroupIdAndRoleAsync(groups[i], (int)role))
                    .ReturnsAsync(usersInGroup[i]);
            }
            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void AddMaterialWithGroups_UserDoesNotBelongToGroup_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDto();
            var groups = new List<int> { 1, 2, 3 };
            var groupDtos = GroupData.GetGroupDtos();
            var usersInGroup = UserData.GetAnotherListsOfUsersInGroup();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { role } };
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
                () => _sut.AddMaterialWithGroupsAsync(materialToAdd, groups, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.Exactly(groups.Count));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(groups.Count));
        }

        [Test]
        public async Task AddMaterialWithCourses_MaterialDtoListOfCourses_MaterialAndCoursesCreatedAsync()
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDto();
            var courses = new List<int> { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithCoursesAsync(materialToAdd, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
        }

        [Test]
        public async Task AddMaterialWithCourses_MaterialDtoAndListOfCourses_MaterialWithCoursesCreatedAsync()
        {
            //Given
            var expectedId = 5;
            var materialToAdd = MaterialData.GetMaterialDto();
            var courses = new List<int> { 1, 2, 3 };
            var courseDtos = CourseData.GetCoursesDtos();

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }
            _materialRepoMock.Setup(x => x.AddMaterialAsync(materialToAdd)).ReturnsAsync(expectedId);

            //When
            int actualId = await _sut.AddMaterialWithCoursesAsync(materialToAdd, courses);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Once);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(courses.Count));
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithDuplicateValues_ValidationExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDto();
            var courses = new List<int> { 1, 2, 2 };
            var expectedMessage = string.Format(ServiceMessages.DuplicateValuesProvided, nameof(courses));

            //When
            var actual = Assert.ThrowsAsync<ValidationException>(
                () => _sut.AddMaterialWithCoursesAsync(materialToAdd, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddMaterialWithCourses_ListOfCoursesWithNotExistingCourse_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToAdd = MaterialData.GetMaterialDto();
            var courses = new List<int> { 1, 2, 3 };
            var courseDtos = new List<CourseDto> { new() { Id = 1 }, new() { Id = 2 }, null };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "course", courses[2]);

            for (int i = 0; i < courses.Count; i++)
            {
                _courseRepoMock.Setup(x => x.GetCourseAsync(courses[i])).ReturnsAsync(courseDtos[i]);
            }

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
               async () => await _sut.AddMaterialWithCoursesAsync(materialToAdd, courses));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.AddMaterialAsync(materialToAdd), Times.Never);
            _courseRepoMock.Verify(x => x.AddCourseMaterialReferenceAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCourseAsync(It.IsAny<int>()), Times.Exactly(courses.Count));
        }

        [Test]
        public async Task UpdateMaterial_MaterialIdMaterialDtoForTeacherRoleHappyFlow_UpdatedMaterialDtoReturnedAsync()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Teacher } };

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
            var materialToUpdate = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };

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
            var materialToUpdate = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { (Role)It.IsAny<int>() } };
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
            var materialToUpdate = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToUpdate.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).
                ReturnsAsync(new List<GroupDto> { new() { Id = 7 }, new() { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToUpdate.Id)).
                ReturnsAsync(new List<GroupDto> { new() { Id = 8 }, new() { Id = 18 } });

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
            var materialToUpdate = MaterialData.GetMaterialDto();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };
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
            var materialToDelete = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Teacher } };

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
            var materialToDelete = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };

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
            var materialToDelete = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { (Role)It.IsAny<int>() } };
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
            var materialToDelete = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Teacher } };
            var expectedMessage = string.
                    Format(ServiceMessages.AccessToMaterialDenied, user.UserId, materialToDelete.Id);

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(user.UserId)).
                ReturnsAsync(new List<GroupDto> { new() { Id = 7 }, new() { Id = 17 } });
            _groupRepoMock.Setup(x => x.GetGroupsByMaterialIdAsync(materialToDelete.Id)).
                ReturnsAsync(new List<GroupDto> { new() { Id = 22 }, new() { Id = 45 } });

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
            var materialToDelete = MaterialData.GetMaterialDtoWithCoursesAndGroups();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };
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
    }
}