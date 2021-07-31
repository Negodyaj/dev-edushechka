using System.Collections.Generic;
using DevEdu.Business.Constants;
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
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        //private Mock<IUserValidationHelper> _userValidationHelperMock;
        private TaskService sut;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            //_userValidationHelperMock = new Mock<IUserValidationHelper>();
            sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _groupRepoMock.Object, new TaskValidationHelper(_taskRepoMock.Object, _groupRepoMock.Object), new UserValidationHelper(_userRepoMock.Object));
        }

        [Test]
        public void AddTaskByTeacher_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var groupTask = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var expectedGroupId = 10;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByTeacher(taskDto, groupTask, expectedGroupId, new List<int>());

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
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var expectedGroupId = 10;
            var groupTask = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var tagsIds = new List<int> {13, 15, 14};

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByTeacher(taskDto, groupTask, expectedGroupId, tagsIds);

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
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByMethodist(taskDto, coursesIds, new List<int>());

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
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var coursesIds = new List<int> { 1 };
            var tagsIds = new List<int> { 13, 15, 14 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(taskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(taskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByMethodist(taskDto, coursesIds, tagsIds);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(taskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var roles = new List<Role>() { Role.Teacher };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(expectedTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto, taskId, userId, roles);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Exactly(2));
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var roles = new List<Role>() { Role.Teacher };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.UpdateTask(taskDto)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)));
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => sut.UpdateTask(taskDto, taskId, userId, roles));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTeacherNotRelatedToTask_AuthorizationException()
        {
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var roles = new List<Role>() { Role.Teacher };
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                          () => sut.UpdateTask(taskDto, taskId, userId, roles));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenMethodistNotRelatedToTask_AuthorizationException()
        {
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var roles = new List<Role>() { Role.Methodist };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                          () => sut.UpdateTask(taskDto, taskId, userId, roles));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var roles = new List<Role>() { Role.Teacher };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => sut.DeleteTask(taskId, userId, roles));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTeacherNotRelatedToTask_AuthorizationException()
        {
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var roles = new List<Role>() { Role.Teacher };
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                          () => sut.DeleteTask(taskId, userId, roles));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenMethodistNotRelatedToTask_AuthorizationException()
        {
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var roles = new List<Role>() { Role.Methodist };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                          () => sut.DeleteTask(taskId, userId, roles));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.DeleteTask(taskId), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var groupDtos = TaskData.GetListOfGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);

            //When
            var dto = sut.GetTaskById(taskId, userId, false);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.GetTaskById(taskId, userId, false));

            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)));
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() {new GroupDto() {Id = 876}};
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskById(taskId, userId, false));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }


        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var courseDtos = TaskData.GetListOfCourses();
            var groupDtos = TaskData.GetListOfGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(taskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);
            //When
            var dto = sut.GetTaskWithCoursesById(taskId, userId, false);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)), 
                () => sut.GetTaskWithCoursesById(taskId, userId, false));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithCoursesById(taskId, userId, false));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(taskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            //When
            var dto = sut.GetTaskWithAnswersById(taskId, userId, false);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)), 
                () => sut.GetTaskWithAnswersById(taskId, userId, false));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithAnswersById(taskId, userId, false));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var groupDtos = TaskData.GetListOfGroups();
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupDtos);
            taskDto.Groups = groupDtos;

            //When
            var dto = sut.GetTaskWithGroupsById(taskId, userId, false);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskId = 1;
            var userId = 10;
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", taskId)),
                () => sut.GetTaskWithCoursesById(taskId, userId, false));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsByIdd_WhenUserNotRelatedToTusk_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var taskId = 1;
            var userId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };
            var userDto = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithCoursesById(taskId, userId, false));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
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

            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(It.IsAny<int>())).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(sameGroupDtos);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[0].Id)).Returns(taskDtos[0]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[1].Id)).Returns(taskDtos[1]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[2].Id)).Returns(taskDtos[2]);
            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            //When
            var dtos = sut.GetTasks(userId, false);

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }
    }
}