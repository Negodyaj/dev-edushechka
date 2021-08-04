using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private GroupService _sut;
        private MaterialValidationHelper _materialValidationHelper;
        private GroupValidationHelper _groupValidationHelper;
        private UserValidationHelper _userValidationHelper;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _materialRepoMock = new Mock<IMaterialRepository>();
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _materialValidationHelper = new MaterialValidationHelper(_materialRepoMock.Object);
            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);
            _sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _materialValidationHelper, _userValidationHelper);
        }

        [Test]
        public void AddGroup_NotParams_ReturnGroupId()
        {
            //Given            
            var groupDto = GroupData.GetGroupDto();
            var groupId = GroupData.GroupId;

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).Returns(groupId);

            //When
            var actualGroupId = _sut.AddGroup(groupDto);

            //Then
            Assert.AreEqual(groupId, actualGroupId);
            _groupRepoMock.Verify(x => x.AddGroup(groupDto), Times.Once);
        }

        [Test]
        public void GetGroupWithListStudents_ByIdAndByRoleStudent_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var roleStudent = GroupData.RoleStudent;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = GroupData.GetUserForGroup();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(groupDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleStudent)).Returns(studentDtos);
            groupDto.Students = studentDtos;

            //When
            var actualGroupDto = _sut.GetGroup(groupId);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, roleStudent), Times.Once);
        }

        [Test]
        public void GetGroups_NotParams_ReturnListGroupDto()
        {
            //Given
            var groupDtos = GroupData.GetGroupsDto();

            _groupRepoMock.Setup(x => x.GetGroups()).Returns(groupDtos);

            //When
            var actualGroupDtos = _sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroups(), Times.Once);
        }

        [Test]
        public void UpdateGroupNameAndTimetable_ByIdAndGroupDto_ReturnGroupDto()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            var updGroupDto = GroupData.GetUpdGroupDto();

            _groupRepoMock.Setup(x => x.UpdateGroup(groupId, groupDto)).Returns(updGroupDto);

            //When
            var actualGroupDto = _sut.UpdateGroup(groupId, groupDto);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupId, groupDto), Times.Once);
        }

        [Test]
        public void ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, groupStatus)).Returns(groupDto);

            //When
            var actualGroupDto = _sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, groupStatus), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddMaterialToGroup_ExistingGroupIdAndMaterialIdPassed_MaterialAddedToGroup(Enum role)
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId));

            //When
            _sut.AddGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);

            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteMaterialFromGroup_ExistingGroupIdAndMaterialIdPassed_MaterialRemoveFromGroup(Enum role)
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId));

            //When
            _sut.RemoveGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);

            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddMaterialToGroup_WhenGroupIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), group.Id);
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));

        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddMaterialToGroup_WhenMaterialIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).Returns(GroupData.GetGroupDto());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddMaterialToGroup_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, user.Id, group.Id);

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).Returns(GroupData.GetAnotherGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(material.Id)).Returns(MaterialData.GetMaterialDtoWithoutTags);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.Id)).Returns(GroupData.GetGroupsDto);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.AddGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(material.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteMaterialFromGroup_WhenGroupIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), group.Id);
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.RemoveGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteMaterialFromGroup_WhenMaterialIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).Returns(GroupData.GetGroupDto());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.RemoveGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteMaterialFromGroup_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var group = GroupData.GetGroupDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, user.Id, group.Id);

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).Returns(GroupData.GetAnotherGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(material.Id)).Returns(MaterialData.GetMaterialDtoWithoutTags);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(user.Id)).Returns(GroupData.GetGroupsDto);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.RemoveGroupMaterialReference(group.Id, material.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(material.Id), Times.Once);
        }
        [Test]
        public void AddGroupToLesson_IntGroupIdAndLessonId_AddLessonToGroup()
        {
            //Given
            const int groupId = 1;
            const int lessonId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.AddGroupToLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public void RemoveGroupFromLesson_IntGroupIdAndLessonId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = 1;
            const int lessonId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.RemoveGroupFromLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public void AddUserToGroup_GroupId_UserId_RoleId_UserAddedToGroup()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            _sut.AddUserToGroup(groupId, userId, roleId);

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, roleId), Times.Once);
        }

        [Test]
        public void AddUserToGroup_UserDoesntExist_EntityNotFoundException()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            UserDto user = default;
            var userId = 0;
            var roleId = 0;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.AddUserToGroup(groupId, userId, roleId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddUserToGroup_UserDoesntHaveRole_ValidationException()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = 0;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<ValidationException>(() => _sut.AddUserToGroup(groupId, userId, roleId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddUserToGroup_GroupDoesntExist_EntityNotFoundException()
        {
            //Given
            GroupDto group = default;
            var groupId = 0;
            var userId = 0;
            var roleId = 0;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.AddUserToGroup(groupId, userId, roleId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteUserFromGroup_GroupId_UserId_UserDeletedFromGroup()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleId)).Returns(UserData.GetListUsersDto());

            //When
            _sut.DeleteUserFromGroup(groupId, userId);

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(userId, groupId), Times.Once);
        }

        [Test]
        public void DeleteUserFromGroup_GroupDoesntExist_EntityNotFoundException()
        {
            //Given
            GroupDto group = default; 
            var groupId = 0;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws< EntityNotFoundException>(()=> _sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteUserFromGroup_UserDoesntExist_EntityNotFoundException()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            UserDto user = default;
            var userId = 0;
            var roleId = 0;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteUserFromGroup_UserDoesntHaveRole_ValidationException()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDtoOutOfList();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleId)).Returns(UserData.GetListUsersDto());

            //When
            Assert.Throws<ValidationException>(() => _sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}