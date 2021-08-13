using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
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
                   new StudentAnswerOnTaskValidationHelper(_studentAnswerOnTaskRepoMock.Object, _groupRepository.Object),
                   new UserValidationHelper(_userRepository.Object),
                   new TaskValidationHelper(_taskRepository.Object, _groupRepository.Object)
                );
        }


        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_ExistingTaskIdAndStudentIdAndStudentAnswerOnTaskInputModelPassed_StudentAnswerWasAdded(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            var groupTaskDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            int taskId = 1;
            int userId = 1;
            int expectedStudentAnswerId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            int countEntry = 2;

            _userRepository.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(GroupData.GetGroupDtos());
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerDto)).Returns(expectedStudentAnswerId);

            // When
            int actualAnswerId = _sut.AddStudentAnswerOnTask(taskId, userId, studentAnswerDto, userInfo);

            // Then
            Assert.AreEqual(expectedStudentAnswerId, actualAnswerId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnTask(studentAnswerDto), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userId), Times.Exactly(countEntry));
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_ExistingTaskIdPassed_StudentAnswersGotList(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = 1;
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();
            var taskListDtos = TaskData.GetListOfGroups();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userDto = UserData.GetUserDto();
            var userId = 1;

            _userRepository.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(taskListDtos);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTask(taskId)).Returns(studentAnswersList);
            _taskRepository.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            // When
            var dtoList = _sut.GetAllStudentAnswersOnTask(taskId, userInfo);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Once);
            _taskRepository.Verify(x => x.GetTaskById(taskId), Times.Once);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_ExistingTaskIdAndStudentIdPassed_StudentAnswerGot(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            dtoForTaskIdAndUserId.Task.Id = taskId;
            dtoForTaskIdAndUserId.User.Id = userId;

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);

            // When
            var dto = _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(dtoForTaskIdAndUserId.Task.Id, dtoForTaskIdAndUserId.User.Id, userInfo);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndStatusIdPassed_StatusChangeded(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            int statusId = (int)TaskStatus.Returned;
            DateTime CompletedDate = default;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate)).Returns(statusId);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, userInfo);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, statusId, CompletedDate), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskStatusAcceptedPassed_CompletedDateChanged(Enum role)
        {
            // Given
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            int taskId = 1;
            int userId = 1;
            int acceptedSatusId = (int)TaskStatus.Accepted;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime)).Returns(acceptedSatusId);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(acceptedStatusDto);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, userInfo);
            var dto = _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(taskId, userId, acceptedSatusId, dateTime), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Exactly(countEntry));
        }


        [TestCase(Role.Student)]
        public void UpdateStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentAnswerOnTaskDto(Enum role)
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.UpdateStudentAnswerOnTask(onlyAnswer));
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(changedStudentAnswerDto);

            // When
            var actualDto = _sut.UpdateStudentAnswerOnTask(taskId, userId, onlyAnswer, userInfo);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.UpdateStudentAnswerOnTask(onlyAnswer), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Exactly(countEntry));
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Methodist)]
        public void GetAllAnswersByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto(Enum role)
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            int userId = 1;
            var userDto = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllAnswersByStudentId(userId)).Returns(studentAnswersListDto);
            _userRepository.Setup(x => x.GetUserById(userId)).Returns(userDto);

            // When
            var dto = _sut.GetAllAnswersByStudentId(userId, userInfo);

            // Then
            Assert.AreEqual(studentAnswersListDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllAnswersByStudentId(userId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(userId), Times.Once);
        }


        [TestCase(Role.Student)]
        public void DeleteStudentAnswerOnTask_ExistingTaskIdAndStudentId_StudentAnswerWasDeleted(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            int taskId = 1;
            int userId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.DeleteStudentAnswerOnTask(taskId, userId));

            // When
            _sut.DeleteStudentAnswerOnTask(taskId, userId, userInfo);

            // Than
            _studentAnswerOnTaskRepoMock.Verify(x => x.DeleteStudentAnswerOnTask(taskId, userId), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, userId), Times.Once);
        }


        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            var groupTaskDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            int taskId = 1;
            int expectedStudentAnswerId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            int anotherUserId = 10;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _userRepository.Setup(x => x.GetUserById(anotherUserId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnTask(studentAnswerOnTaskDto)).Returns(expectedStudentAnswerId);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                () => _sut.AddStudentAnswerOnTask(taskId, studentAnswerOnTaskDto.User.Id, studentAnswerOnTaskDto, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnTask(studentAnswerOnTaskDto), Times.Never);
            _groupRepository.Verify(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(anotherUserId), Times.Once);
        }


        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            int taskId = 1;
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var user = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), studentAnswerOnTaskDto.User.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddStudentAnswerOnTask(taskId, studentAnswerOnTaskDto.User.Id, studentAnswerOnTaskDto, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }


        [TestCase(Role.Student)]
        public void DeleteStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            int taskId = 1;
            int userId = 4;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteStudentAnswerOnTask(taskId, userId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }


        [TestCase(Role.Student, 2)]
        public void DeleteStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int expectedUserId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, expectedUserId)).Returns(studentAnswerDto);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.DeleteStudentAnswerOnTask(taskId, expectedUserId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, expectedUserId), Times.Once);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            int taskId = 1;
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var user = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.User.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentAnswerOnTaskDto.User.Id, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher, 2)]
        [TestCase(Role.Tutor, 2)]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            int taskId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentAnswerOnTaskDto.User.Id)).Returns(studentAnswerOnTaskDto);

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentAnswerOnTaskDto.User.Id, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentAnswerOnTaskDto.User.Id), Times.Once);
        }


        [TestCase(Role.Student)]
        public void UpdateStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            int taskId = 1;
            int userId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.UpdateStudentAnswerOnTask(taskId, userId, onlyAnswer, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }


        [TestCase(Role.Student, 2)]
        public void UpdateStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            int taskId = 1;
            int expectedUserId = 1;

            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, expectedUserId)).Returns(studentAnswerOnTaskDto);

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, expectedUserId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, expectedUserId), Times.Once);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_WhenUserIdDoNotHaveInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = 1;
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();
            var taskListDtos = TaskData.GetListOfGroups();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var user = UserData.GetUserDto();
            var userId = 1;
            var anotherUserId = 2;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            _userRepository.Setup(x => x.GetUserById(anotherUserId)).Returns(user);
            _groupRepository.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(taskListDtos);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetAllStudentAnswersOnTask(taskId)).Returns(studentAnswersList);
            _taskRepository.Setup(x => x.GetTaskById(taskId)).Returns(taskDto);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.GetAllStudentAnswersOnTask(taskId, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Never);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_WheTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = 1;
            var anotherTaskId = 2;
            var task = TaskData.GetAnotherTaskDtoWithTags();
            var taskListDtos = TaskData.GetListOfGroups();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var user = UserData.GetUserDto();
            var userId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId);

            _userRepository.Setup(x => x.GetUserById(userId)).Returns(user);
            _groupRepository.Setup(x => x.GetGroupsByTaskId(anotherTaskId)).Returns(taskListDtos);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _taskRepository.Setup(x => x.GetTaskById(anotherTaskId)).Returns(task);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.GetAllStudentAnswersOnTask(taskId, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Never);
        }


        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_WhenUserIsNotInGroup_StudentAnswerWasNull(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            int taskId = 1;
            var task = TaskData.GetAnotherTaskDtoWithTags();
            var taskListDtos = TaskData.GetListOfGroups();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var user = UserData.GetUserDto();
            var userId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId);

            _userRepository.Setup(x => x.GetUserById(userId)).Returns(user);
            _taskRepository.Setup(x => x.GetTaskById(taskId)).Returns(task);
            _groupRepository.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(taskListDtos);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetAnotherListGroupDtos());

            // When
            var emptyList = _sut.GetAllStudentAnswersOnTask(taskId, userInfo); ;

            // Then
            Assert.AreEqual(emptyList, null);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Never);
            _taskRepository.Verify(x => x.GetTaskById(taskId), Times.Once);
        }
    }
}
