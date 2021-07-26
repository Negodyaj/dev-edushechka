using DevEdu.Business.Services;
using DevEdu.Business.Tests.Data;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskServiceTests
    {
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerOnTaskRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;

        [SetUp]
        public void Setup()
        {
            _studentAnswerOnTaskRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
        }

        [Test]
        public void AddStudentAnswerOnTask_ExistingTaskIdAndStudentIdAndStudentAnswerOnTaskInputModelPassed_StudentAnswerWasAdded()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerDto)).Returns(StudentAnswerOnTaskData.ExpectedStudentAnswerId);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object, _groupRepoMock.Object);

            // When
            int actualAnswerId = sut.AddStudentAnswerOnTask(taskId, userId, studentAnswerDto);

            // Then

            Assert.AreEqual(StudentAnswerOnTaskData.ExpectedStudentAnswerId, actualAnswerId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnTask(studentAnswerDto), Times.Once);
        }

        [Test]
        public void GetAllStudentAnswersOnTask_ExistingTaskIdPassed_StudentAnswersGotList()
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = StudentAnswerOnTaskData.TaskId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTask(taskId)).Returns(studentAnswersList);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object, _groupRepoMock.Object);

            // When
            var dtoList = sut.GetAllStudentAnswersOnTask(taskId);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Once);
        }

        [Test]
        public void GetAllStudentAnswersOnTasks_TouchMethod_StudentAnswersGotList()
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTasks()).Returns(studentAnswersList);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object, _groupRepoMock.Object);

            // When
            var dtoList = sut.GetAllStudentAnswersOnTasks();

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTasks(), Times.Once);
        }

        [Test]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_ExistingTaskIdAndStudentIdPassed_StudentAnswerGot()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(dtoForTaskIdAndUserId)).Returns(studentAnswerDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object, _groupRepoMock.Object);

            // When
            var dto = sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(studentAnswerDto));

        }

    }
}
