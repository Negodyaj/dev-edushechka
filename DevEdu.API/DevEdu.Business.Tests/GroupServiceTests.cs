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
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
        private GroupValidationHelper _groupHelper;
        private UserValidationHelper _userHelper;
        private LessonValidationHelper _lessonHelper;
        private MaterialValidationHelper _materialHelper;
        private TaskValidationHelper _taskHelper;
        private GroupService _sut;


        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _materialRepoMock = new Mock<IMaterialRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _groupHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _userHelper = new UserValidationHelper(_userRepoMock.Object);
            _lessonHelper = new LessonValidationHelper(_lessonRepoMock.Object);
            _materialHelper = new MaterialValidationHelper(_materialRepoMock.Object);
            _taskHelper = new TaskValidationHelper(_taskRepoMock.Object);
            _sut = new
            (
                _groupRepoMock.Object,
                _groupHelper,
                _userRepoMock.Object,
                _userHelper,
                _lessonRepoMock.Object,
                _lessonHelper,
                _materialRepoMock.Object,
                _materialHelper,
                _taskRepoMock.Object,
                _taskHelper
            );
        }

        [Test]
        public async Task AddGroup_NotParams_GroupAdded()
        {
            //Given            
            var groupId = 2;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).ReturnsAsync(groupId);

            //When
            var actualGroupId = await _sut.AddGroup(groupDto);

            //Then
            Assert.AreEqual(groupId, actualGroupId);
            _groupRepoMock.Verify(x => x.AddGroup(groupDto), Times.Once);
        }

        [Test]
        public async Task GetGroupWithListStudents_ByIdAndByRoleStudent_ReturnGroupDto()
        {
            //Given            
            var groupId = 2;
            var groupId2 = 3;
            var roleStudent = GroupData.RoleStudent;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = UserData.GetListUsersDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId2));
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)roleStudent)).Returns(studentDtos);

            //When
            var actualGroupDto = await _sut.GetGroup(groupId, userInfo);

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

            //When
            var actualGroupDtos = await _sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroups(), Times.Once);
        }

        [Test]
        public async Task DeleteGroup_ById_ReturnVoid()
        {
            //Given
            var groupId = 2;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.DeleteGroup(groupId));

            //When
            await _sut.DeleteGroup(groupId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteGroup(groupId), Times.Once);
        }

        [Test]
        public async Task UpdateGroupNameAndTimetable_ByIdAndGroupDto_ReturnGroupDto()
        {
            //Given
            var groupId = 1;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            groupDto.Id = groupId;
            var updGroupDto = GroupData.GetUpdGroupDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.UpdateGroup(groupDto)).ReturnsAsync(updGroupDto);

            //When
            var actualGroupDto = await _sut.UpdateGroup(groupId, groupDto, userInfo);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupDto), Times.Once);
        }

        [Test]
        public async Task ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = 3;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, (int)groupStatus)).ReturnsAsync(groupDto);

            //When
            var actualGroupDto = await _sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, (int)groupStatus), Times.Once);
        }

        [Test]
        public async Task AddGroupToLesson_ByGroupIdAndLessonId_ReturnString()
        {
            //Given
            var groupId = 1;
            var lessonId = 2;
            var userInfo = GroupData.GetUserInfo();
            var groupDto = GroupData.GetGroupDto();
            var lessonDto = LessonData.GetSelectedLessonDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId));

            //When
            await _sut.AddGroupToLesson(groupId, lessonId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public async Task RemoveGroupFromLesson_ByGroupIdAndLessonId_ReturnVoid()
        {
            //Given
            var groupId = 1;
            var lessonId = 2;
            var groupDto = GroupData.GetGroupDto();
            var lessonDto = LessonData.GetSelectedLessonDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId));

            //When
            await _sut.RemoveGroupFromLesson(groupId, lessonId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }


        [Test]
        public async Task AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            var groupId = 2;
            var materialId = 2;
            var expectedAffectedRows = 3;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).ReturnsAsync(expectedAffectedRows);

            //When
            var actualAffectedRows = await _sut.AddGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);

            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Never);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            var groupId = 2;
            var materialId = 2;
            var expectedAffectedRows = 3;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).ReturnsAsync(expectedAffectedRows);

            //When
            var actualAffectedRows = await _sut.RemoveGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task AddUserToGroup_ByGroupIdAndLessonIdAndRole_ReturnVoid()
        {
            //Given
            var groupId = 2;
            var userId = 3;
            var groupDto = GroupData.GetGroupDto();
            var userDto = UserData.GetUserDto();
            var role = GroupData.RoleStudent;
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto); ;
            _groupRepoMock.Setup(x => x.AddUserToGroup(groupId, userId, (int)role));

            //When
            await _sut.AddUserToGroup(groupId, userId, role, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Never);
            _groupRepoMock.Verify(x => x.AddUserToGroup(groupId, userId, (int)role), Times.Once);
        }

        [Test]
        public async Task DeleteUserFromGroup_ByGroupIdAndUserId_ReturnVoid()
        {
            //Given
            var groupId = 2;
            var userId = 2;
            var groupDto = GroupData.GetGroupDto();
            var userDto = UserData.GetUserDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.DeleteUserFromGroup(groupId, userId));

            //When
            await _sut.DeleteUserFromGroup(groupId, userId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Never);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, userId), Times.Never);
        }


        [Test]
        public async Task AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupId = 1;
            var taskId = 2;
            var expectedGroupTaskId = 3;
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).ReturnsAsync(expectedGroupTaskId);

            //When
            var actualGroupTaskId = await _sut.AddTaskToGroup(groupId, taskId, groupTaskDto, userInfo);

            //Than
            Assert.AreEqual(expectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public async Task GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupId = 2;
            var taskId = 2;
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var userInfo = GroupData.GetUserInfo();
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            //When
            var dto = await _sut.GetGroupTask(groupId, taskId, userInfo);

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
            var groupId = 2;
            var taskId = 3;
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            var userInfo = GroupData.GetUserInfo();
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).ReturnsAsync(groupTaskDto);

            //When
            var actualGroupTaskDto = await _sut.UpdateGroupTask(groupId, taskId, groupTaskDto, userInfo);

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
            var groupId = 1;
            var taskId = 3;
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            var userInfo = GroupData.GetUserInfo();
            var groupDto = GroupData.GetGroupDto();
            var taskDto = TaskData.GetTaskDtoWithTags();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            //When
            await _sut.DeleteTaskFromGroup(groupId, taskId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Never);
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public async Task GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupId = 2;
            var userInfo = GroupData.GetUserInfo();
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).ReturnsAsync(groupTaskList);

            //When
            var dto = await _sut.GetTasksByGroupId(groupId, userInfo);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Never);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
    }
}