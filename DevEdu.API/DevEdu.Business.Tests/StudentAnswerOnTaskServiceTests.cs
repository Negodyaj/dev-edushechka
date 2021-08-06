using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskServiceTests
    {
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerOnTaskRepoMock;
        private Mock<ITaskRepository> _taskRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;
        private StudentAnswerOnTaskService _sut;

        [SetUp]
        public void Setup()
        {
            _studentAnswerOnTaskRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _taskRepository = new Mock<ITaskRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _userRepository = new Mock<IUserRepository>();
            _sut = new StudentAnswerOnTaskService(
                   _studentAnswerOnTaskRepoMock.Object, 
                   new StudentAnswerOnTaskValidationHelper(_studentAnswerOnTaskRepoMock.Object),
                   new UserValidationHelper(_userRepository.Object), 
                   new TaskValidationHelper(_taskRepository.Object, _groupRepository.Object)
                );
        }


        [Test]
        public void AddStudentAnswerOnTask_ExistingTaskIdAndStudentIdAndStudentAnswerOnTaskInputModelPassed_StudentAnswerWasAdded()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            var groupTaskDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            int taskId = 1;
            int userId = 1;
            int expectedStudentAnswerId = 1;


            _userRepository.Setup(x => x.SelectUserById(userId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupTaskDtos);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(groupsByUser);
            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerDto)).Returns(expectedStudentAnswerId);

            // When
            int actualAnswerId = _sut.AddStudentAnswerOnTask(taskId, userId, studentAnswerDto);

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
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTask(taskId)).Returns(studentAnswersList);
            _taskRepository.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            // When
            var dtoList = _sut.GetAllStudentAnswersOnTask(taskId);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Once);
            _taskRepository.Verify(x => x.GetTaskById(taskId), Times.Once);
        }


        [Test]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_ExistingTaskIdAndStudentIdPassed_StudentAnswerGot()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();
            int countEntry = 2;

            dtoForTaskIdAndUserId.Task.Id = taskId;
            dtoForTaskIdAndUserId.User.Id = userId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);

            // When
            var dto = _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(dtoForTaskIdAndUserId.Task.Id, dtoForTaskIdAndUserId.User.Id);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Exactly(countEntry));
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

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate)).Returns(statusId);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
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
            int countEntry = 3;

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime)).Returns(acceptedSatusId);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(acceptedStatusDto);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId);
            var dto = _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId);

            // Then
            Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), (dto.CompletedDate != null ? ((DateTime)dto.CompletedDate).ToString("dd.MM.yyyy HH:mm") : null));
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Exactly(countEntry));
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
            int countEntry = 2;

            _studentAnswerOnTaskRepoMock.Setup(x => x.UpdateStudentAnswerOnTask(onlyAnswer));
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(changedStudentAnswerDto);

            // When
            var actualDto = _sut.UpdateStudentAnswerOnTask(taskId, userId, onlyAnswer);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.UpdateStudentAnswerOnTask(onlyAnswer), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Exactly(countEntry));
        }

        [Test]
        public void GetAllAnswersByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto()
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            int userId = 1;
            var userDto = UserData.GetUserDto();

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllAnswersByStudentId(userId)).Returns(studentAnswersListDto);
            _userRepository.Setup(x => x.SelectUserById(userId)).Returns(userDto);

            // When
            var dto = _sut.GetAllAnswersByStudentId(userId);

            // Then
            Assert.AreEqual(studentAnswersListDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllAnswersByStudentId(userId), Times.Once);
            _userRepository.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteStudentAnswerOnTask_ExistingTaskIdAndStudentId_StudentAnswerWasDeleted()
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();
            int taskId = 1;
            int userId = 4;

            _taskRepository.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.DeleteStudentAnswerOnTask(taskId, userId)).Verifiable();
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);

            //When
            _sut.DeleteStudentAnswerOnTask(taskId, userId);
            var dto = _sut.GetAllStudentAnswersOnTask(taskId);

            //Than
            _studentAnswerOnTaskRepoMock.Verify(x => x.DeleteStudentAnswerOnTask(taskId, userId), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.AtLeastOnce);
            _taskRepository.Verify(x => x.GetTaskById(taskId), Times.Once);
        }



    }
}
