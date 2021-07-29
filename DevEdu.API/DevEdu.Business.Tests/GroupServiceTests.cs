using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
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
        private Mock<IGroupValidationHelper> _groupHelper;
        private Mock<ILessonValidationHelper> _lessonHelper;
        private Mock<IMaterialValidationHelper> _materialHelper;
        private Mock<IUserValidationHelper> _userHelper;
        private Mock<ITaskValidationHelper> _taskHelper;


        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _groupHelper = new Mock<IGroupValidationHelper>();
            _lessonHelper = new Mock<ILessonValidationHelper>();
            _materialHelper = new Mock<IMaterialValidationHelper>();
            _userHelper = new Mock<IUserValidationHelper>();
            _taskHelper = new Mock<ITaskValidationHelper>();
        }

        [Test]
        public void AddGroup_NotParams_ReturnGroupId()
        {
            //Given            
            var groupDto = GroupData.GetGroupDto();
            var groupId = GroupData.GroupId;

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).Returns(groupId);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

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

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(groupDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent)).Returns(studentDtos);
            groupDto.Students = studentDtos;

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userRepository: _userRepoMock.Object);

            //When
            var actualGroupDto = sut.GetGroup(groupId);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent), Times.Once);
        }

        [Test]
        public void GetGroups_NotParams_ReturnListGroupDto()
        {
            //Given
            var groupDtos = GroupData.GetGroupsDto();

            _groupRepoMock.Setup(x => x.GetGroups()).Returns(groupDtos);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDtos = sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroups(), Times.Once);
        }

        [Test]
        public void DeleteGroup_ById_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.DeleteGroup(groupId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            sut.DeleteGroup(groupId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteGroup(groupId), Times.Once);
        }

        [Test]
        public void UpdateGroupNameAndTimetable_ByIdAndGroupDto_ReturnGroupDto()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            groupDto.Id = groupId;
            var updGroupDto = GroupData.GetUpdGroupDto();

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.UpdateGroup(groupDto)).Returns(updGroupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = sut.UpdateGroup(groupId, groupDto);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupDto), Times.Once);
        }

        [Test]
        public void ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, (int)groupStatus)).Returns(groupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, (int)groupStatus), Times.Once);
        }

        [Test]
        public void AddGroupToLesson_ByGroupIdAndLessonId_ReturnString()
        {
            //Given
            var groupId = GroupData.GroupId;
            var lessonId = GroupData.LessonId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _lessonHelper.Setup(x => x.CheckLessonExistence(lessonId));
            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, lessonHelper: _lessonHelper.Object);

            //When
            sut.AddGroupToLesson(groupId, lessonId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _lessonHelper.Verify(x => x.CheckLessonExistence(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public void RemoveGroupFromLesson_ByGroupIdAndLessonId_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var lessonId = GroupData.LessonId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _lessonHelper.Setup(x => x.CheckLessonExistence(lessonId));
            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, lessonHelper: _lessonHelper.Object);

            //When
            sut.RemoveGroupFromLesson(groupId, lessonId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _lessonHelper.Verify(x => x.CheckLessonExistence(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }


        [Test]
        public void AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _materialHelper.Setup(x => x.CheckMaterialExistence(materialId));
            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, materialHelper: _materialHelper.Object);

            //When
            var actualAffectedRows = sut.AddGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);

            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _materialHelper.Verify(x => x.CheckMaterialExistence(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _materialHelper.Setup(x => x.CheckMaterialExistence(materialId));
            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, materialHelper: _materialHelper.Object);

            //When
            var actualAffectedRows = sut.RemoveGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _materialHelper.Verify(x => x.CheckMaterialExistence(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void AddUserToGroup_ByGroupIdAndLessonIdAndRole_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var userId = GroupData.UserId;
            var role = GroupData.RoleStudent;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _userHelper.Setup(x => x.CheckUserExistence(userId));
            _groupRepoMock.Setup(x => x.AddUserToGroup(groupId, userId, (int)role));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userHelper: _userHelper.Object);

            //When
            sut.AddUserToGroup(groupId, userId, role);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _userHelper.Verify(x => x.CheckUserExistence(userId), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, (int)role), Times.Once);
        }

        [Test]
        public void DeleteUserFromGroup_ByGroupIdAndUserId_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var userId = GroupData.UserId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _userHelper.Setup(x => x.CheckUserExistence(userId));
            _groupRepoMock.Setup(x => x.DeleteUserFromGroup(groupId, userId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userHelper: _userHelper.Object);

            //When
            sut.DeleteUserFromGroup(groupId, userId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _userHelper.Verify(x => x.CheckUserExistence(userId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, userId), Times.Never);
        }


        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).Returns(GroupTaskData.ExpectedGroupTaskId);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var actualGroupTaskId = sut.AddTaskToGroup(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(GroupTaskData.ExpectedGroupTaskId, actualGroupTaskId);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var dto = sut.GetGroupTask(groupId, taskId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var actualGroupTaskDto = sut.UpdateGroupTask(groupId, taskId, groupTaskDto);

            //Then
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
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

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            sut.DeleteTaskFromGroup(groupId, taskId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            const int groupId = GroupTaskData.GroupId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).Returns(groupTaskList);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var dto = sut.GetTasksByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
    }
}