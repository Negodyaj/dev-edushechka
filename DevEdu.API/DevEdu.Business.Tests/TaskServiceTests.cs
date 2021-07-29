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
        private Mock<TaskValidationHelper> _taskHelper;
        private Mock<TagValidationHelper> _tagHelper;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _taskHelper = new Mock<TaskValidationHelper>();
            _tagHelper = new Mock<TagValidationHelper>();
        }


        [Test]
        public void AddTask_SimpleDtoWithoutTags_TaskCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithoutTags();

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(TaskData.expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(TaskData.expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTask_DtoWithTags_TaskWithTagsCreated()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            _taskRepoMock.Setup(x => x.AddTask(taskDto)).Returns(TaskData.expectedTaskId);
            _taskRepoMock.Setup(x => x.AddTagToTask(TaskData.expectedTaskId, It.IsAny<int>()));

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var actualTaskId = sut.AddTask(taskDto);

            //Than
            Assert.AreEqual(TaskData.expectedTaskId, actualTaskId);
            _taskRepoMock.Verify(x => x.AddTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.AddTagToTask(TaskData.expectedTaskId, It.IsAny<int>()), Times.Exactly(taskDto.Tags.Count));
        }

        [Test]
        public void GetTaskById_IntTaskId_ReturnedTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dto = sut.GetTaskById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesById_IntTaskId_ReturnedTaskDtoWithCourses()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            var courseDtos = TaskData.GetListOfCourses();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dto = sut.GetTaskWithCoursesById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithAnswersById_IntTaskId_ReturnedTaskDtoWithStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dto = sut.GetTaskWithAnswersById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTaskWithCoursesAndAnswersById_IntTaskId_ReturnedTaskDtoWithCoursesAndStudentAnswers()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();

            var courseDtos = TaskData.GetListOfCourses();

            var studentAnswersDtos = TaskData.GetListOfStudentAnswers();

            _taskRepoMock.Setup(x => x.GetTaskById(TaskData.expectedTaskId)).Returns(taskDto);
            _courseRepoMock.Setup(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId)).Returns(courseDtos);
            taskDto.Courses = courseDtos;
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId)).Returns(studentAnswersDtos);
            taskDto.StudentAnswers = studentAnswersDtos;

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dto = sut.GetTaskWithCoursesAndAnswersById(TaskData.expectedTaskId);

            //Than
            Assert.AreEqual(taskDto, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(TaskData.expectedTaskId), Times.Once);
            _courseRepoMock.Verify(x => x.GetCoursesToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswersToTaskByTaskId(TaskData.expectedTaskId), Times.Once);
        }

        [Test]
        public void GetTasks_NoEntry_ReturnedTaskDtos()
        {
            //Given
            var taskDtos = TaskData.GetListOfTasks();

            _taskRepoMock.Setup(x => x.GetTasks()).Returns(taskDtos);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dtos = sut.GetTasks();

            //Than
            Assert.AreEqual(taskDtos, dtos);
            _taskRepoMock.Verify(x => x.GetTasks(), Times.Once);
        }

        [Test]
        public void UpdateTask_TaskDto_ReturnUpdateTaskDto()
        {
            //Given
            var taskDto = TaskData.GetTaskDtoWithTags();
            var expectedTaskDto = TaskData.GetAnotherTaskDtoWithTags();

            //_taskRepoMock.Setup(x => x.UpdateTask(taskDto));
            _taskRepoMock.Setup(x => x.GetTaskById(taskDto.Id)).Returns(expectedTaskDto);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var actualTaskDto = sut.UpdateTask(taskDto);

            //Then
            Assert.AreEqual(expectedTaskDto, actualTaskDto);
            _taskRepoMock.Verify(x => x.UpdateTask(taskDto), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskDto.Id), Times.Once);
        }
        [Test]
        public void GetGroupsByTaskId_IntTaskId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithGroup();
            const int taskId = GroupTaskData.TaskId;

            _taskRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupTaskList);

            var sut = new TaskService(_taskRepoMock.Object, _courseRepoMock.Object, _studentAnswerRepoMock.Object, _taskHelper.Object, _tagHelper.Object);

            //When
            var dto = sut.GetGroupsByTaskId(taskId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _taskRepoMock.Verify(x => x.GetGroupsByTaskId(taskId), Times.Once);
        }
    }
}