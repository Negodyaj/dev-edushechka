using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IGroupValidationHelper> _groupHelper;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IUserValidationHelper> _userHelper;
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<ILessonValidationHelper> _lessonHelper;
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<IMaterialValidationHelper> _materialHelper;
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ITaskValidationHelper> _taskHelper;


        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _groupHelper = new Mock<IGroupValidationHelper>();
            _userRepoMock = new Mock<IUserRepository>();
            _userHelper = new Mock<IUserValidationHelper>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _lessonHelper = new Mock<ILessonValidationHelper>();
            _materialRepoMock = new Mock<IMaterialRepository>();
            _materialHelper = new Mock<IMaterialValidationHelper>();
            _taskRepoMock = new Mock<ITaskRepository>();
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
            var groupId2 = GroupData.GroupId + 1;
            var roleStudent = GroupData.RoleStudent;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = UserData.GetListUsersDto();
            var userId = UserData.expectedUserId;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId2));
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent)).Returns(studentDtos);

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                userRepository: _userRepoMock.Object,
                userHelper: _userHelper.Object
            );

            //When
            var actualGroupDto = await sut.GetGroup(groupId, userId);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId2), Times.Never);
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
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.DeleteGroup(groupId));

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            await sut.DeleteGroup(groupId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
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
            var userId = UserData.expectedUserId;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.UpdateGroup(groupDto)).ReturnsAsync(updGroupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = await sut.UpdateGroup(groupId, groupDto, userId);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupDto), Times.Once);
        }

        [Test]
        public async Task ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, (int)groupStatus)).ReturnsAsync(groupDto);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var actualGroupDto = await sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, (int)groupStatus), Times.Once);
        }

        [Test]
        public async Task AddGroupToLesson_ByGroupIdAndLessonId_ReturnString()
        {
            //Given
            var groupId = GroupData.GroupId;
            var lessonId = LessonData.LessonId;
            var userId = UserData.expectedUserId;
            var groupDto = GroupData.GetGroupDto();
            var lessonDto = LessonData.GetSelectedLessonDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId));

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                lessonRepository: _lessonRepoMock.Object,
                lessonHelper: _lessonHelper.Object
            );

            //When
            await sut.AddGroupToLesson(groupId, lessonId, userId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public async Task RemoveGroupFromLesson_ByGroupIdAndLessonId_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDto();
            var lessonId = LessonData.LessonId;
            var lessonDto = LessonData.GetSelectedLessonDto();
            var userId = UserData.expectedUserId;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId));

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                lessonRepository: _lessonRepoMock.Object,
                lessonHelper: _lessonHelper.Object
            );

            //When
            await sut.RemoveGroupFromLesson(groupId, lessonId, userId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }


        [Test]
        public async Task AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            const int materialId = GroupData.MaterialId;
            var userId = UserData.expectedUserId;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).ReturnsAsync(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object, materialRepository: _materialRepoMock.Object, materialHelper: _materialHelper.Object);

            //When
            var actualAffectedRows = await sut.AddGroupMaterialReference(groupId, materialId, userId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);

            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            var groupId = GroupData.GroupId;
            var materialId = GroupData.MaterialId;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            var userId = UserData.expectedUserId;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).ReturnsAsync(GroupData.ExpectedAffectedRows);

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                materialRepository: _materialRepoMock.Object,
                materialHelper: _materialHelper.Object
            );

            //When
            var actualAffectedRows = await sut.RemoveGroupMaterialReference(groupId, materialId, userId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task AddUserToGroup_ByGroupIdAndLessonIdAndRole_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDto();
            var userDto = UserData.GetUserDto();
            var userId = UserData.expectedUserId;
            var role = GroupData.RoleStudent;
            var currentUserId = GroupData.UserIdAuthorization;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto); ;
            _groupRepoMock.Setup(x => x.AddUserToGroup(groupId, userId, (int)role));

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                userRepository: _userRepoMock.Object,
                userHelper: _userHelper.Object
            );

            //When
            await sut.AddUserToGroup(groupId, userId, role, currentUserId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Never);
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, (int)role), Times.Once);
        }

        [Test]
        public async Task DeleteUserFromGroup_ByGroupIdAndUserId_ReturnVoid()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDto();
            var userId = UserData.expectedUserId;
            var userDto = UserData.GetUserDto();
            var currentUserId = GroupData.UserIdAuthorization;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.DeleteUserFromGroup(groupId, userId));

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                userRepository: _userRepoMock.Object,
                userHelper: _userHelper.Object
            );

            //When
            await sut.DeleteUserFromGroup(groupId, userId, currentUserId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, userId), Times.Never);
        }


        [Test]
        public async Task AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            var groupDto = GroupData.GetGroupDto();
            const int taskId = GroupTaskData.TaskId;
            var taskDto = TaskData.GetTaskDtoWithTags();
            var userId = GroupData.UserIdAuthorization;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).ReturnsAsync(GroupTaskData.ExpectedGroupTaskId);

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                taskRepository: _taskRepoMock.Object,
                taskHelper: _taskHelper.Object
            );

            //When
            var actualGroupTaskId = await sut.AddTaskToGroup(groupId, taskId, groupTaskDto, userId);

            //Than
            Assert.AreEqual(GroupTaskData.ExpectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public async Task GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var groupId = GroupData.GroupId;
            const int taskId =TaskData.expectedTaskId;
            var userId = GroupData.UserIdAuthorization;
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                taskRepository: _taskRepoMock.Object,
                taskHelper: _taskHelper.Object
            );

            //When
            var dto = await sut.GetGroupTask(groupId, taskId, userId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
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
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                taskRepository: _taskRepoMock.Object,
                taskHelper: _taskHelper.Object
            );

            //When
            var actualGroupTaskDto = await sut.UpdateGroupTask(groupId, taskId, groupTaskDto, userId);

            //Then
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
            _groupRepoMock.Verify(x => x.UpdateGroupTask(groupTaskDto), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public async Task DeleteGroupTask_IntGroupIdAndTaskId_DeleteGroupTask()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;
            var userId = GroupData.UserIdAuthorization;
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            var sut = new GroupService
            (
                _groupRepoMock.Object,
                _groupHelper.Object,
                taskRepository: _taskRepoMock.Object,
                taskHelper: _taskHelper.Object
            );

            //When
            await sut.DeleteTaskFromGroup(groupId, taskId, userId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public async Task GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            var groupId = GroupTaskData.GroupId;
            var userId = GroupData.UserIdAuthorization;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).ReturnsAsync(groupTaskList);

            var sut = new GroupService(_groupRepoMock.Object, _groupHelper.Object);

            //When
            var dto = await sut.GetTasksByGroupId(groupId, userId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
    }
}