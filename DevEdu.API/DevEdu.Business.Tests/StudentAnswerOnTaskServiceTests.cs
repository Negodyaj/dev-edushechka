using DevEdu.Business.Services;
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
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerDto)).Returns(StudentAnswerOnTaskData.ExpectedStudentAnswerId);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

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
           // var listAnswersDot = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;
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
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;
            int statusId = StudentAnswerOnTaskData.ReturnedStatus;
            var inputIds = StudentAnswerOnTaskData.GetTaskIdStudentIdAndStatusIdDto();

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(inputIds)).Returns((int)inputIds.TaskStatus);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualStatusId = sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, inputIds);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(inputIds), Times.Once);
        }

        [Test]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskStatusAcceptedPassed_CompletedDateChanged()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;
            int statusId = StudentAnswerOnTaskData.AcceptedStatus;
            var inputIds = StudentAnswerOnTaskData.GetTaskIdStudentIdAndStatusIdDto();

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(inputIds)).Returns((int)inputIds.TaskStatus);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(acceptedStatusDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualStatusId = sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, inputIds);
            var dto = sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId);

            // Then
            Assert.AreEqual(acceptedStatusDto.CompletedDate, dto.CompletedDate);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(inputIds), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }


        [Test]
        public void UpdateStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentAnswerOnTaskDto()
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = StudentAnswerOnTaskData.TaskId;
            int userId = StudentAnswerOnTaskData.UserId;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var inputIds = StudentAnswerOnTaskData.GetTaskIdStudentIdAndStatusIdDto();

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.UpdateStudentAnswerOnTask(onlyAnswer));
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(changedStudentAnswerDto);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            //sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(1, 1);
            var actualDto = sut.UpdateStudentAnswerOnTask(taskId, userId, onlyAnswer);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.UpdateStudentAnswerOnTask(onlyAnswer), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);

        }


        [Test]
        public void AddCommentOnStudentAnswer_ExistingTaskStudentIdAndcommentIdPassed_CommentWasAdded()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var answerAddedForTaskStudent = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDtoWithAddedComment();
            int taskStudentId = 1;
            int commentId = 4;

            _studentAnswerOnTaskRepoMock.Setup(x => x.AddCommentOnStudentAnswer(taskStudentId, commentId)).Returns(taskStudentId);

            var sut = new StudentAnswerOnTaskService(_studentAnswerOnTaskRepoMock.Object);

            // When
            var actualTaskStudentId = sut.AddCommentOnStudentAnswer(taskStudentId, commentId);

            // Then
            Assert.AreEqual(taskStudentId, actualTaskStudentId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddCommentOnStudentAnswer(taskStudentId, commentId), Times.Once);
        }

        
        [Test]
        public void GetAllAnswersByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto()
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            int userId = StudentAnswerOnTaskData.UserId;

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
