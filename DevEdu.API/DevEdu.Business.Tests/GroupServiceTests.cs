using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

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
        public async Task AddGroup_NotParams_ReturnGroupId()
        {
            //Given            
            var groupDto = GroupData.GetGroupDto();
            var groupId = GroupData.GroupId;

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).ReturnsAsync(groupId);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupId = await sut.AddGroup(groupDto);

            //Then
            Assert.AreEqual(groupId, actualGroupId);
            _groupRepoMock.Verify(x => x.AddGroup(groupDto), Times.Once);
        }

        [Test]
        public async Task GetGroupWithListStudents_ByIdAndByRoleStudent_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var roleStudent = GroupData.RoleStudent;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = GroupData.GetUserForGroup();
            var userId = GroupData.UserId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent)).Returns(studentDtos);
            groupDto.Students = studentDtos;

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userRepository: _userRepoMock.Object);

            //When
            var actualGroupDto = await sut.GetGroup(groupId, userId);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent), Times.Once);
        }

        [Test]
        public async Task GetGroups_NotParams_ReturnListGroupDto()
        {
            //Given
            var groupDtos = GroupData.GetGroupsDto();

            _groupRepoMock.Setup(x => x.GetGroups()).ReturnsAsync(groupDtos);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDtos = await sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroups(), Times.Once);
        }

        [Test]
        public async Task DeleteGroup_ById_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.DeleteGroup(groupId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            await sut.DeleteGroup(groupId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteGroup(groupId), Times.Once);
        }

        [Test]
        public async Task UpdateGroupNameAndTimetable_ByIdAndGroupDto_ReturnGroupDto()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            groupDto.Id = groupId;
            var updGroupDto = GroupData.GetUpdGroupDto();
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.UpdateGroup(groupDto)).ReturnsAsync(updGroupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = await sut.UpdateGroup(groupId, groupDto, userId);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupDto), Times.Once);
        }

        [Test]
        public async Task ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, (int)groupStatus)).ReturnsAsync(groupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = await sut.ChangeGroupStatus(groupId, groupStatus);

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
            var userId = GroupData.UserIdAuthorization;

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, lessonHelper: _lessonHelper.Object);

            //When
            sut.AddGroupToLesson(groupId, lessonId, userId);

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
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _lessonHelper.Setup(x => x.CheckLessonExistence(lessonId));
            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, lessonHelper: _lessonHelper.Object);

            //When
            sut.RemoveGroupFromLesson(groupId, lessonId, userId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _lessonHelper.Verify(x => x.CheckLessonExistence(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }


        [Test]
        public async Task AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _materialHelper.Setup(x => x.CheckMaterialExistence(materialId));
            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).ReturnsAsync(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, materialHelper: _materialHelper.Object);

            //When
            var actualAffectedRows = await sut.AddGroupMaterialReference(groupId, materialId, userId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);

            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _materialHelper.Verify(x => x.CheckMaterialExistence(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _materialHelper.Setup(x => x.CheckMaterialExistence(materialId));
            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).ReturnsAsync(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, materialHelper: _materialHelper.Object);

            //When
            var actualAffectedRows = await sut.RemoveGroupMaterialReference(groupId, materialId, userId);

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
            var currentUserId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _userHelper.Setup(x => x.CheckUserExistence(userId));
            _groupRepoMock.Setup(x => x.AddUserToGroup(groupId, userId, (int)role));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userHelper: _userHelper.Object);

            //When
            sut.AddUserToGroup(groupId, userId, role, currentUserId);

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
            var currentUserId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _userHelper.Setup(x => x.CheckUserExistence(userId));
            _groupRepoMock.Setup(x => x.DeleteUserFromGroup(groupId, userId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, userHelper: _userHelper.Object);

            //When
            sut.DeleteUserFromGroup(groupId, userId, currentUserId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _userHelper.Verify(x => x.CheckUserExistence(userId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, userId), Times.Never);
        }


        [Test]
        public async Task AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).ReturnsAsync(GroupTaskData.ExpectedGroupTaskId);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var actualGroupTaskId = await sut.AddTaskToGroup(groupId, taskId, groupTaskDto, userId);

            //Than
            Assert.AreEqual(GroupTaskData.ExpectedGroupTaskId, actualGroupTaskId);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public async Task GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var dto = await sut.GetGroupTask(groupId, taskId, userId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public async Task UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            var actualGroupTaskDto = await sut.UpdateGroupTask(groupId, taskId, groupTaskDto, userId);

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
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _taskHelper.Setup(x => x.CheckTaskExistence(taskId));
            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, taskHelper: _taskHelper.Object);

            //When
            sut.DeleteTaskFromGroup(groupId, taskId, userId);

            //Then
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _taskHelper.Verify(x => x.CheckTaskExistence(taskId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public async Task GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            var groupId = GroupTaskData.GroupId;
            var userId = GroupData.UserIdAuthorization;

            _groupHelper.Setup(x => x.CheckGroupExistence(groupId));
            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).ReturnsAsync(groupTaskList);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var dto = await sut.GetTasksByGroupId(groupId, userId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupHelper.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
    }
}