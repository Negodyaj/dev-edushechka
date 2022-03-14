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
                _courseRepoMock.Object,
                new CourseValidationHelper(_courseRepoMock.Object, _groupRepoMock.Object),
                new MaterialValidationHelper(_materialRepoMock.Object, _groupRepoMock.Object, _courseRepoMock.Object)
            );
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetAllMaterials_NoEntryForTeacherStudentOrTutorRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithCourses();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterialsAsync()).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetAllMaterialsAsync(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterialsAsync(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Exactly(groupsByUser.Count + 1));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Exactly(coursesByMaterial.Count));
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetAllMaterials_NoEntryForMethodistOrAdminRole_ListOfMaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterials = MaterialData.GetListOfMaterialsWithCourses();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(groupsByUser);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(coursesByMaterial);
            _materialRepoMock.Setup(x => x.GetAllMaterialsAsync()).ReturnsAsync(expectedMaterials);

            //When
            var actualMaterials = await _sut.GetAllMaterialsAsync(user);

            //Then
            Assert.AreEqual(expectedMaterials, actualMaterials);
            _materialRepoMock.Verify(x => x.GetAllMaterialsAsync(), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task GetMaterialByIdWithCoursesAndGroups_ExistingMaterialId_MaterialDtoWithCoursesAndGroupsReturnedAsync()
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCourses();
            var coursesByMaterial = CourseData.GetCoursesDtos();

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(coursesByMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdWithCoursesAsync(expectedMaterial.Id);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetMaterialByIdWithCoursesAndGroups_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithCourses();
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialByIdWithCoursesAsync(material.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetMaterialById_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByGroups_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCourses();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>())).ReturnsAsync(new List<CourseDto> { new() { Id = groupsByUser[0].Course.Id } });

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public async Task GetMaterialById_ExistingMaterialIdAccessibleForTeacherStudentOrTutorRoleByCourses_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCourses();
            var groupsByUser = GroupData.GetAnotherGroupDtos();
            var coursesByMaterial = CourseData.GetCoursesDtos();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(expectedMaterial.Id)).ReturnsAsync(coursesByMaterial);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(expectedMaterial.Id)).ReturnsAsync(groupsByUser);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.Methodist)]
        [TestCase(Role.Admin)]
        public async Task GetMaterialById_ExistingMaterialIdForMethodistOrAdminRole_MaterialDtoReturnedAsync(Role role)
        {
            //Given
            var expectedMaterial = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedMaterial);

            //When
            var actualMaterial = await _sut.GetMaterialByIdAsync(expectedMaterial.Id, user);

            //Then
            Assert.AreEqual(expectedMaterial, actualMaterial);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetMaterialById_NotExistingMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetMaterialByIdAsync(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Student)]
        [TestCase(Role.Tutor)]
        public void GetMaterialById_ExistingMaterialIdNotAccessibleForTeacherStudentOrTutorRoleByCourses_AuthorizationExceptionThrown(Role role)
        {
            //Given
            var material = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = It.IsAny<int>(), Roles = new List<Role> { role } };
            var expectedMessage = string.Format(ServiceMessages.AccessToMaterialDenied, user.UserId, material.Id);

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(material.Id)).ReturnsAsync(new List<CourseDto>());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(material.Id)).ReturnsAsync(new List<GroupDto>());
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(material.Id)).ReturnsAsync(material);

            //When
            var actual = Assert.ThrowsAsync<AuthorizationException>(() => _sut.GetMaterialByIdAsync(material.Id, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Once);
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
        public async Task UpdateMaterial_MaterialIdMaterialDtoForMethodistRoleHappyFlow_UpdatedMaterialDtoReturnedAsync()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithCourses();
            var expectedMaterial = MaterialData.GetUpdatedMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };

            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate.Courses);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToUpdate.Id)).ReturnsAsync(materialToUpdate);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(expectedMaterial.Id)).ReturnsAsync(expectedMaterial);

            //When
            var actual = await _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user);

            //Then
            Assert.AreEqual(expectedMaterial, actual);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialToUpdate.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateMaterialAsync(materialToUpdate.Id, materialToUpdate, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.UpdateMaterialAsync(materialToUpdate), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateMaterial_MaterialIdMaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToUpdate = MaterialData.GetMaterialDto();
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
        public async Task DeleteMaterial_MaterialIdForMethodistRoleHappyFlow_MaterialDeletedAsync()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { Role.Methodist } };

            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete);
            _courseRepoMock.Setup(x => x.GetCoursesByMaterialIdAsync(materialToDelete.Id)).ReturnsAsync(materialToDelete.Courses);

            //When
            await _sut.DeleteMaterialAsync(materialToDelete.Id, true, user);

            //Then
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Once);
        }

        [Test]
        public void DeleteMaterial_NotExistingMaterial_EntityNotFoundExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithCourses();
            var user = new UserIdentityInfo { UserId = 2, Roles = new List<Role> { (Role)It.IsAny<int>() } };
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialToDelete.Id);

            //When
            var actual = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteMaterialAsync(materialToDelete.Id, true, user));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Never);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }

        [Test]
        public void DeleteMaterial_MaterialDtoNotAccessibleToMethodistRole_AuthorizationExceptionThrown()
        {
            //Given
            var materialToDelete = MaterialData.GetMaterialDtoWithCourses();
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
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(It.IsAny<int>()), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesByMaterialIdAsync(It.IsAny<int>()), Times.Once);
            _materialRepoMock.Verify(x => x.DeleteMaterialAsync(It.IsAny<int>(), true), Times.Never);
        }
    }
}