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

namespace DevEdu.Business.Tests
{
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ITagRepository> _tagRepoMock;
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
            _tagRepoMock = new Mock<ITagRepository>();
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
                    _groupRepoMock.Object),
                new UserValidationHelper(
                    _userRepoMock.Object
                )
            );
        }

        [Test]
        public void AddTaskByTeacher_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var expectedGroupId = 10;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = _sut.AddTaskByTeacher(taskDto, homework, expectedGroupId, null);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void AddTaskByTeacher_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var expectedGroupId = 10;
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var tagsIds = new List<int> { 13, 15, 14 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = _sut.AddTaskByTeacher(taskDto, homework, expectedGroupId, tagsIds);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(taskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodist(taskDto, coursesIds, null);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };
            var tagsIds = new List<int> { 13, 15, 14 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodist(taskDto, coursesIds, tagsIds);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(taskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithoutCourses_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var taskId = 1;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = _sut.AddTaskByMethodist(taskDto, null, null);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(expectedTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            //When
            var actualTaskDto = _sut.UpdateTask(taskDto, taskId, userIdentityInfo);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Exactly(2));
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
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)));
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.UpdateTask(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.UpdateTask(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.UpdateTask(taskDto, taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);
            _taskRepoMock.Setup(x => x.DeleteTask(taskId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.DeleteTask(taskId, userIdentityInfo);

            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Once);
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

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.DeleteTask(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.DeleteTask(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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

            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                          () => _sut.DeleteTask(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            //When
            var dto = _sut.GetTaskById(taskId, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(taskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);
            //When
            var dto = _sut.GetTaskWithCoursesById(taskId, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(taskId), Times.Once);
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

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithCoursesById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithCoursesById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);
            _studentAnswerRepoMock.Setup(x => x.GetAllStudentHomeworkByTask(taskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            //When
            var dto = _sut.GetTaskWithAnswersById(taskId, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetAllStudentHomeworkByTask(taskId), Times.Once);
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

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithAnswersById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithAnswersById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);
            taskDto.Groups = groupDtos;

            //When
            var dto = _sut.GetTaskWithGroupsById(taskId, userIdentityInfo);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => _sut.GetTaskWithCoursesById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            Assert.Throws(Is.TypeOf<AuthorizationException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)),
                () => _sut.GetTaskWithCoursesById(taskId, userIdentityInfo));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
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
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(It.IsAny<int>())).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(sameGroupDtos);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[0].Id)).Returns(taskDtos[0]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[1].Id)).Returns(taskDtos[1]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[2].Id)).Returns(taskDtos[2]);
            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            //When
            var dtos = _sut.GetTasks(userIdentityInfo);

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }
    }
}