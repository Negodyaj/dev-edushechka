using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        GroupService _sut;
        private Mock<IUserRepository> _userRepoMock;
        private IGroupValidationHelper _groupValidationHelper;
        private IUserValidationHelper _userValidationHelper;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);
            _sut = new GroupService(_groupRepoMock.Object);
        }

        [Test]
        public void AddGroup_NotParams_ReturnGroupId()
        {
            //Given            
            var groupDto = GroupData.GetGroupDto();
            var groupId = GroupData.GroupId;

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).Returns(groupId);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupId = sut.AddGroup(groupDto);

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

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            //When
            var actualGroupDto = sut.GetGroup(groupId);

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

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupDtos = sut.GetGroups();

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

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupDto = sut.UpdateGroup(groupId, groupDto);

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
            
            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupDto = sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, groupStatus), Times.Once);
        }

        [Test]
        public void AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.AddGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.RemoveGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;
            const int expectedGroupTaskId = 42;

            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).Returns(expectedGroupTaskId);

            //When
            var actualGroupTaskId = _sut.AddTaskToGroup(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(expectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            //When
            var dto = _sut.GetGroupTask(groupId, taskId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            //When
            var actualGroupTaskDto = _sut.UpdateGroupTask(groupId, taskId, groupTaskDto);

            //Then
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _groupRepoMock.Verify(x => x.UpdateGroupTask(groupTaskDto), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void DeleteGroupTask_IntGroupIdAndTaskId_DeleteGroupTask()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            //When
            _sut.DeleteTaskFromGroup(groupId, taskId);

            //Then
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            const int groupId = 1;

            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetTasksByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
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

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            sut.AddUserToGroup(groupId, userId, roleId);

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, roleId), Times.Once);
        }

        [Test]
        public void AddUserToGroup_UserDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            UserDto user = default;
            var userId = 0;
            var roleId = 0;

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<EntityNotFoundException>(() => sut.AddUserToGroup(groupId, userId, roleId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddUserToGroup_UserDoesntHaveRole_ValidationExceptionThrown()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = 0;

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<ValidationException>(() => sut.AddUserToGroup(groupId, userId, roleId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddUserToGroup_GroupDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            GroupDto group = default;
            var groupId = 0;
            var userId = 0;
            var roleId = 0;

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws<EntityNotFoundException>(() => sut.AddUserToGroup(groupId, userId, roleId));

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

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleId)).Returns(UserData.GetListUsersDto());

            //When
            sut.DeleteUserFromGroup(groupId, userId);

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(userId, groupId), Times.Once);
        }

        [Test]
        public void DeleteUserFromGroup_GroupDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            GroupDto group = default; 
            var groupId = 0;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws< EntityNotFoundException>(()=> sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteUserFromGroup_UserDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            UserDto user = default;
            var userId = 0;

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            Assert.Throws<EntityNotFoundException>(() => sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteUserFromGroup_UserDoesntHaveRole_ValidationExceptionThrown()
        {
            //Given
            var group = GroupData.GetGroupDto();
            var groupId = group.Id;
            var user = UserData.GetUserDtoOutOfList();
            var userId = user.Id;
            var roleId = (int)user.Roles[0];

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object, _groupValidationHelper, _userValidationHelper);

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(user);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleId)).Returns(UserData.GetListUsersDto());

            //When
            Assert.Throws<ValidationException>(() => sut.DeleteUserFromGroup(groupId, userId));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}