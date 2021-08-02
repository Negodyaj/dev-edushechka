using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IUserRepository> _userRepoMock;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
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

            var sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object);

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
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.AddGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.RemoveGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).Returns(GroupTaskData.ExpectedGroupTaskId);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupTaskId = sut.AddTaskToGroup(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(GroupTaskData.ExpectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var dto = sut.GetGroupTask(groupId, taskId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupTaskDto = sut.UpdateGroupTask(groupId, taskId, groupTaskDto);

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
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            sut.DeleteTaskFromGroup(groupId, taskId);

            //Then
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            const int groupId = GroupTaskData.GroupId;

            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).Returns(groupTaskList);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var dto = sut.GetTasksByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }

        [Test]
        public void AddUserToGroup_GroupId_UserId_RoleId_UserAddedToGroup()
        {
            //Given
            int groupId = 0;
            int userId = 0;
            int roleId = 0;

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            sut.AddUserToGroup(groupId, userId, roleId);

            //Than
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, roleId), Times.Once);
        }

        [Test]
        public void DeleteUserFromGroup_GroupId_UserId_RoleId_UserDeletedFromGroup()
        {
            //Given
            int groupId = 0;
            int userId = 0;

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            sut.DeleteUserFromGroup(groupId, userId);

            //Than
            _groupRepoMock.Verify(x => x.DeleteUserFromGroup(groupId, userId), Times.Once);
        }
    }
}