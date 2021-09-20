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
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IStudentHomeworkRepository> _studentAnswerRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IHomeworkRepository> _homeworkRepoMock;
        private TaskService _sut;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentHomeworkRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _homeworkRepoMock = new Mock<IHomeworkRepository>();
            _sut = new TaskService(
                _taskRepoMock.Object,
                _courseRepoMock.Object,
                _studentAnswerRepoMock.Object,
                _groupRepoMock.Object,
                _homeworkRepoMock.Object,
                new TaskValidationHelper(
                    _taskRepoMock.Object,
                    _groupRepoMock.Object, _courseRepoMock.Object),
                new UserValidationHelper(
                    _userRepoMock.Object
                )
            );
        }

        [Test]
        public async Task AddTaskByTeacher_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var expectedGroupId = 10;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.AddTaskAsync(taskDto)).ReturnsAsync(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            var actualTask = await _sut.AddTaskByTeacherAsync(taskDto, homework, expectedGroupId, null, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
        }

        [Test]
        public async Task AddTaskByTeacher_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var expectedGroupId = 10;
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var tagsIds = new List<int> { 13, 15, 14 };
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.AddTaskAsync(taskDto)).ReturnsAsync(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTaskAsync(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            var actualTask = await _sut.AddTaskByTeacherAsync(taskDto, homework, expectedGroupId, tagsIds, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTaskAsync(taskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Exactly(2));
        }

        [Test]
        public void AddTaskByMethodist_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.AddTaskAsync(taskDto)).ReturnsAsync(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodistAsync(taskDto, coursesIds, null, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };
            var tagsIds = new List<int> { 13, 15, 14 };
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.AddTaskAsync(taskDto)).ReturnsAsync(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTaskAsync(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodistAsync(taskDto, coursesIds, tagsIds, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTaskAsync(taskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Exactly(2));
        }

        [Test]
        public void AddTaskByMethodist_WithoutCourses_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.AddTaskAsync(taskDto)).ReturnsAsync(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodistAsync(taskDto, null, null, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTaskAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdatedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(expectedTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            //When
            var actualTaskDto = _sut.UpdateTaskAsync(taskDto, taskId, userIdentityInfo).Result;

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTaskAsync(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskDto.Id), Times.Exactly(2));
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.UpdateTaskAsync(taskDto)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)));
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.UpdateTaskAsync(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTaskAsync(taskDto), Times.Never);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTeacherNotRelatedToTask_ThrownAuthorizationException()
        {
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.UpdateTaskAsync(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTaskAsync(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenMethodistNotRelatedToTask_ThrownAuthorizationException()
        {
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Methodist } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.UpdateTaskAsync(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTaskAsync(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_TaskId_DeleteTask()
        {
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };
            var expectedAffectedRows = 1;

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _taskRepoMock.Setup(x => x.DeleteTaskAsync(taskId)).ReturnsAsync(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.DeleteTaskAsync(taskId, userIdentityInfo).Result;

            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _taskRepoMock.Verify(x => x.DeleteTaskAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.DeleteTaskAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTaskAsync(taskId), Times.Never);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTeacherNotRelatedToTask_ThrownAuthorizationException()
        {
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.DeleteTaskAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTaskAsync(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenMethodistNotRelatedToTask_ThrownAuthorizationException()
        {
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Methodist } };

            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.DeleteTaskAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTaskAsync(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            //When
            var dto = _sut.GetTaskByIdAsync(taskId, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenUserNotRelatedToTask_ThrownAuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }


        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var courseDtos = TaskData.GetListOfCourses();
            var groupDtos = TaskData.GetListOfGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskIdAsync(taskId)).ReturnsAsync(courseDtos);
            taskDto.Courses = courseDtos;
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupDtos);
            //When
            var dto = _sut.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenUserNotRelatedToTask_ThrownAuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            _studentAnswerRepoMock.Setup(x => x.GetAllStudentHomeworkByTaskAsync(taskId)).ReturnsAsync(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            //When
            var dto = _sut.GetTaskWithAnswersByIdAsync(taskId, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetAllStudentHomeworkByTaskAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithAnswersByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenUserNotRelatedToTask_ThrownAuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithAnswersByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);
            taskDto.Groups = groupDtos;

            //When
            var dto = _sut.GetTaskWithGroupsByIdAsync(taskId, userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_WhenTaskDoesNotExist_ThrownEntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.ThrowsAsync(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_WhenUserNotRelatedToTask_ThrownAuthorizationException()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithGroup();
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(taskId)).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groupsByUser);

            Assert.ThrowsAsync(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = TaskData.GetListOfTasks();
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var sameGroupDtos = TaskData.GetListOfSameGroups();
            var userDto = UserData.GetUserDto();
            var userIdentityInfo = new UserIdentityInfo() { UserId = userId, Roles = new List<Role>() { Role.Teacher } };

            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskIdAsync(It.IsAny<int>())).ReturnsAsync(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(sameGroupDtos);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskDtos[0].Id)).ReturnsAsync(taskDtos[0]);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskDtos[1].Id)).ReturnsAsync(taskDtos[1]);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskDtos[2].Id)).ReturnsAsync(taskDtos[2]);
            _taskRepoMock.Setup(x => x.GetTasksAsync()).ReturnsAsync(taskDtos);

            //When
            var dtos = _sut.GetTasksAsync(userIdentityInfo).Result;

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasksAsync(), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }
    }
}