using System.Collections.Generic;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
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
        private Mock<ITaskValidationHelper> _taskValidationHelperMock;
        private Mock<IUserValidationHelper> _userValidationHelperMock;
        private TaskService sut;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _taskValidationHelperMock = new Mock<ITaskValidationHelper>();
            _userValidationHelperMock = new Mock<IUserValidationHelper>();
            sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _groupRepoMock.Object, new TaskValidationHelper(_taskRepoMock.Object, _groupRepoMock.Object), _userValidationHelperMock.Object);
        }

        [Test]
        public void AddTaskByTeacher_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var groupTask = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var expectedGroupId = 10;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByTeacher(taskDto, groupTask, expectedGroupId, new List<int>());

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void AddTaskByTeacher_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedGroupId = 10;
            var groupTask = GroupTaskData.GetGroupTaskWithGroupAndTask();
            var tagsIds = new List<int> {13, 15, 14};

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByTeacher(taskDto, groupTask, expectedGroupId, tagsIds);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var coursesIds = new List<int> { 1 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByMethodist(taskDto, coursesIds, new List<int>());

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void AddTaskByMethodist_WithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var coursesIds = new List<int> { 1 };
            var tagsIds = new List<int> { 13, 15, 14 };

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            //When
            var actualTask = sut.AddTaskByMethodist(taskDto, coursesIds, tagsIds);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();

            _taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(expectedTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto, expectedTaskId, expectedUserId);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Exactly(2));
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();

            _taskRepoMock.Setup(x => x.UpdateTask(taskDto)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)),
                () => sut.UpdateTask(taskDto, expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Never);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)),
                () => sut.DeleteTask(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.DeleteTask(expectedTaskId), Times.Never);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var groupDtos = TaskData.GetListOfGroups();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);

            //When
            var dto = sut.GetTaskById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.GetTaskById(expectedTaskId, expectedUserId));

            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() {new GroupDto() {Id = 876}};

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }


        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var courseDtos = TaskData.GetListOfCourses();
            var groupDtos = TaskData.GetListOfGroups();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(expectedTaskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);
            //When
            var dto = sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)), 
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(expectedTaskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            //When
            var dto = sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)), 
                () => sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenUserNotRelatedToTask_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var groupDtos = TaskData.GetListOfGroups();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupDtos);
            taskDto.Groups = groupDtos;

            //When
            var dto = sut.GetTaskWithGroupsById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)),
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithGroupsByIdd_WhenUserNotRelatedToTusk_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDto();
            var expectedTaskId = 55;
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var groupsByUser = new List<GroupDto>() { new GroupDto() { Id = 876 } };

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(expectedTaskId)).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(groupsByUser);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }

       [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = TaskData.GetListOfTasks();
            var expectedUserId = 10;
            var groupDtos = TaskData.GetListOfGroups();
            var sameGroupDtos = TaskData.GetListOfSameGroups();

            _groupRepoMock.Setup(x => x.GetGroupsByTaskId(It.IsAny<int>())).Returns(groupDtos);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(expectedUserId)).Returns(sameGroupDtos);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[0].Id)).Returns(taskDtos[0]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[1].Id)).Returns(taskDtos[1]);
            _taskRepoMock.Setup(x => x.GetTaskById(taskDtos[2].Id)).Returns(taskDtos[2]);
            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            //When
            var dtos = sut.GetTasks(expectedUserId);

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(expectedUserId), Times.Once);
        }
    }
}