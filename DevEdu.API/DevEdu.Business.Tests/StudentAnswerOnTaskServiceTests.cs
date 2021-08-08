using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskServiceTests
    {
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerOnTaskRepoMock;

        [SetUp]
        public void Setup()
        {
            _studentAnswerOnTaskRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
        }


        [Test]
        public void AddStudentAnswerOnTask_ExistingTaskIdAndStudentIdAndStudentAnswerOnTaskInputModelPassed_StudentAnswerWasAdded()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            int expectedStudentAnswerId = 1;

            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerDto)).Returns(expectedStudentAnswerId);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            int actualAnswerId = sut.AddStudentAnswerOnTask(taskId, userId, studentAnswerDto);

            // Then

            Assert.AreEqual(expectedStudentAnswerId, actualAnswerId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnTask(studentAnswerDto), Times.Once);
        }


        [Test]
        public void GetAllStudentAnswersOnTask_ExistingTaskIdPassed_StudentAnswersGotList()
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = 1;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTask(taskId)).Returns(studentAnswersList);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var dtoList = sut.GetAllStudentAnswersOnTask(taskId);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Once);
        }


        [Test]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_ExistingTaskIdAndStudentIdPassed_StudentAnswerGot()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();

            dtoForTaskIdAndUserId.Task.Id = taskId;
            dtoForTaskIdAndUserId.User.Id = userId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var dto = sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(dtoForTaskIdAndUserId.Task.Id, dtoForTaskIdAndUserId.User.Id);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }

        [Test]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndStatusIdPassed_StatusChangeded()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            int statusId = (int)TaskStatus.Returned;
            DateTime CompletedDate = default;

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate)).Returns(statusId);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualStatusId = sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate), Times.Once);
        }

        [Test]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskStatusAcceptedPassed_CompletedDateChanged()
        {
            // Given
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            int taskId = 1;
            int userId = 1;
            int acceptedSatusId = (int)TaskStatus.Accepted;
            DateTime dateNow = DateTime.Now;
            var dateString = dateNow.ToString("dd.MM.yyyy HH:mm");
            DateTime dateTime = Convert.ToDateTime(dateString);

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime)).Returns(acceptedSatusId);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(acceptedStatusDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualStatusId = sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId);
            var dto = sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId);

            // Then
            Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), (dto.CompletedDate != null ? ((DateTime)dto.CompletedDate).ToString("dd.MM.yyyy HH:mm") : null));
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }


        [Test]
        public void UpdateStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentAnswerOnTaskDto()
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();

            _studentAnswerOnTaskRepoMock.Setup(x => x.UpdateStudentAnswerOnTask(onlyAnswer));
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(changedStudentAnswerDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualDto = sut.UpdateStudentAnswerOnTask(taskId, userId, onlyAnswer);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.UpdateStudentAnswerOnTask(onlyAnswer), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }

        [Test]
        public void GetAllAnswersByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto()
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            int userId = 1;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllAnswersByStudentId(userId)).Returns(studentAnswersListDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var dto = sut.GetAllAnswersByStudentId(userId);

            // Then
            Assert.AreEqual(studentAnswersListDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllAnswersByStudentId(userId), Times.Once);
        }

    }
}
