using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
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
        private Mock<ITaskValidationHelper> _taskValidationHelperMock;
        private Mock<IUserValidationHelper> _userValidationHelperMock;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _taskValidationHelperMock = new Mock<ITaskValidationHelper>();
            _userValidationHelperMock = new Mock<IUserValidationHelper>();
        }


        [Test]
        public void AddTask_SimpleDtoWithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();
            var expectedTaskId = 55;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var actualTask = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void AddTask_DtoWithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskId = 55;

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()));
            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var actualTask = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(taskDto, actualTask);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(expectedTaskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskRepoMock.Setup(x => x.GetTaskById(expectedTaskId)).Returns(taskDto);
            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var dto = sut.GetTaskById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.GetTaskById(expectedTaskId, expectedUserId));

            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Never);
        }

        [Test]
        public void GetTaskById_WhenUserNotRelatedToTusk_AuthorizationException()
        {
            //Given
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId)).Throws(
                new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));


            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }


        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var courseDtos = TaskData.GetListOfCourses();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(expectedTaskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var dto = sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)), 
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Never);
        }

        [Test]
        public void GetTaskWithCoursesById_WhenUserNotRelatedToTusk_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Returns(taskDto);
            _taskValidationHelperMock.Setup(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId)).Throws(
                new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));


            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithCoursesById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Returns(taskDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(expectedTaskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var dto = sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)), 
                () => sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Never);
        }

        [Test]
        public void GetTaskWithAnswersById_WhenUserNotRelatedToTusk_AuthorizationException()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskId = 55;
            var expectedUserId = 10;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Returns(taskDto);
            _taskValidationHelperMock.Setup(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId)).Throws(
                new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));


            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            AuthorizationException ex = Assert.Throws<AuthorizationException>(
                () => sut.GetTaskWithAnswersById(expectedTaskId, expectedUserId));
            Assert.That(ex.Message, Is.EqualTo(string.Format(ServiceMessages.EntityDoesntHaveAcessToMessage, "user", expectedUserId, "task", expectedTaskId)));

            _taskRepoMock.Verify(x => x.GetTaskById(expectedTaskId), Times.Never);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistenceWithValidation(expectedTaskId, expectedUserId), Times.Once);
        }

        [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = TaskData.GetListOfTasks();

            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var dtos = sut.GetTasks();

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void GetTasks_WhenDoesNotExistAnyTask_EntityNotFoundException()
        {
            _taskRepoMock.Setup(x => x.GetTasks()).Throws(new EntityNotFoundException(ServiceMessages.TasksNotFoundMessage));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(ServiceMessages.TasksNotFoundMessage), () => sut.GetTasks());
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();
            var expectedTaskId = 55;

            _taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskDto.Id)).Returns(expectedTaskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto, expectedTaskId);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
        }

        [Test]
        public void UpdateTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskId = 55;

            _taskRepoMock.Setup(x => x.UpdateTask(taskDto)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)),
                () => sut.UpdateTask(taskDto, expectedTaskId));

            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
        }

        [Test]
        public void DeleteTask_WhenTaskDoesNotExist_EntityNotFoundException()
        {
            var expectedTaskId = 55;

            _taskValidationHelperMock.Setup(x => x.CheckTaskExistence(expectedTaskId)).Throws(
                new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                    .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "task", expectedTaskId)),
                () => sut.DeleteTask(expectedTaskId));

            _taskValidationHelperMock.Verify(x => x.CheckTaskExistence(expectedTaskId), Times.Once);
            _taskRepoMock.Verify(x => x.DeleteTask(expectedTaskId), Times.Never);
        }


        [Test]
        public void GetGroupsByTaskId_IntTaskId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupList = GroupData.GetListOfGroupDto();
            const int taskId = GroupTaskData.TaskId;

            _taskRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupList);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskValidationHelperMock.Object, _userValidationHelperMock.Object);

            //When
            var dto = sut.GetGroupsByTaskId(taskId);

            //Than
            Assert.AreEqual(groupList, dto);
            _taskRepoMock.Verify(x => x.GetGroupsByTaskId(taskId), Times.Once);
        }
    }
}