using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskServiceTests
    {
        private Mock<IStudentHomeworkRepository> _studentAnswerOnTaskRepoMock;
        private Mock<ITaskRepository> _taskRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IHomeworkRepository> _homeworkRepository;
        private StudentHomeworkService _sut;

        [SetUp]
        public void Setup()
        {
            _studentAnswerOnTaskRepoMock = new Mock<IStudentHomeworkRepository>();
            _taskRepository = new Mock<ITaskRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _userRepository = new Mock<IUserRepository>();
            _homeworkRepository = new Mock<IHomeworkRepository>();
            _sut = new StudentHomeworkService(
                   _studentAnswerOnTaskRepoMock.Object,
                   new StudentHomeworkValidationHelper(_studentAnswerOnTaskRepoMock.Object, _groupRepository.Object),
                   new UserValidationHelper(_userRepository.Object),
                   new TaskValidationHelper(_taskRepository.Object, _groupRepository.Object),
                   new HomeworkValidationHelper(_homeworkRepository.Object)
                );
        }

        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_ExistingTaskIdAndStudentIdAndStudentAnswerOnTaskInputModelPassed_StudentAnswerWasAdded(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            const int homeworkId = 1;
            const int userId = 1;
            const int expectedStudentAnswerId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int countEntry = 2;

            _userRepository.Setup(x => x.GetUserById(userId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(GroupData.GetGroupDtos());
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnHomework(studentAnswerDto)).Returns(expectedStudentAnswerId);

            // When
            int actualAnswerId = _sut.AddStudentAnswerOnTask(homeworkId, studentAnswerDto, userInfo);

            // Then
            Assert.AreEqual(expectedStudentAnswerId, actualAnswerId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnHomework(studentAnswerDto), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userId), Times.Exactly(countEntry));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_ExistingTaskIdPassed_StudentAnswersGotList(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            const int taskId = 1;
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
            var dtoList = _sut.GetAllStudentAnswersOnTask(taskId);

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
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id)).Returns(studentAnswerDto);

            // When
            var dto = _sut.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id, userInfo);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndStatusIdPassed_StatusChangeded(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int userId = 1;
            const int statusId = (int)TaskStatus.Returned;
            DateTime CompletedDate = default;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, statusId, CompletedDate)).Returns(statusId);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(homeworkId, statusId, userInfo);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, statusId, CompletedDate), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskStatusAcceptedPassed_CompletedDateChanged(Enum role)
        {
            // Given
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            const int homeworkId = 1;
            const int userId = 1;
            const int acceptedSatusId = (int)TaskStatus.Accepted;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, acceptedSatusId, dateTime)).Returns(acceptedSatusId);
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(acceptedStatusDto);

            // When
            var actualStatusId = _sut.ChangeStatusOfStudentAnswerOnTask(homeworkId, acceptedSatusId, userInfo);
            var dto = _sut.GetStudentHomeworkById(homeworkId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentAnswerOnTaskRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, acceptedSatusId, dateTime), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Exactly(countEntry));
        }

        [TestCase(Role.Student)]
        public void UpdateStudentAnswerOnTask_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentAnswerOnTaskDto(Enum role)
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int userId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            const int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.UpdateStudentAnswerOnTask(onlyAnswer));
            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(changedStudentAnswerDto);

            // When
            var actualDto = _sut.UpdateStudentAnswerOnTask(homeworkId, onlyAnswer, userInfo);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentAnswerOnTaskRepoMock.Verify(x => x.UpdateStudentAnswerOnTask(onlyAnswer), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Exactly(countEntry));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Methodist)]
        public void GetAllAnswersByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto(Enum role)
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            const int userId = 1;
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
            const int homeworkId = 1;
            const int userId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerDto);
            _studentAnswerOnTaskRepoMock.Setup(x => x.DeleteStudentHomework(homeworkId));

            // When
            _sut.DeleteStudentAnswerOnTask(homeworkId, userInfo);

            // Than
            _studentAnswerOnTaskRepoMock.Verify(x => x.DeleteStudentHomework(homeworkId), Times.Once);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userDto = UserData.GetUserDto();
            var groupTaskDtos = TaskData.GetListOfGroups();
            var groupsByUser = TaskData.GetListOfSameGroups();
            const int homeworkId = 1;
            const int expectedStudentAnswerId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            const int anotherUserId = 10;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _userRepository.Setup(x => x.GetUserById(anotherUserId)).Returns(userDto);
            _groupRepository.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupDtos());
            _studentAnswerOnTaskRepoMock.Setup(x => x.AddStudentAnswerOnHomework(studentAnswerOnTaskDto)).Returns(expectedStudentAnswerId);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                () => _sut.AddStudentAnswerOnTask(homeworkId, studentAnswerOnTaskDto, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.AddStudentAnswerOnHomework(studentAnswerOnTaskDto), Times.Never);
            _groupRepository.Verify(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(anotherUserId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void AddStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int homeworkId = 1;
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var user = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), studentAnswerOnTaskDto.User.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddStudentAnswerOnTask(homeworkId, studentAnswerOnTaskDto, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Student)]
        public void DeleteStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            const int userId = 4;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteStudentAnswerOnTask(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Student, 2)]
        public void DeleteStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int expectedUserId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerDto);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.DeleteStudentAnswerOnTask(homeworkId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentAnswerOnTaskByTaskIdAndStudentId_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int homeworkId = 1;
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var user = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.User.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetStudentHomeworkById(homeworkId, userInfo));

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
            const int homeworkId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerOnTaskDto);

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentHomeworkById(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void UpdateStudentAnswerOnTask_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int userId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTaskDto), studentAnswerOnTaskDto.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.UpdateStudentAnswerOnTask(homeworkId, onlyAnswer, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Student, 2)]
        public void UpdateStudentAnswerOnTask_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var studentAnswerOnTaskDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            const int homeworkId = 1;
            const int expectedUserId = 1;

            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerOnTaskRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerOnTaskDto);

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentHomeworkById(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentAnswersOnTask_WhenUserIdDoNotHaveInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            const int taskId = 1;
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
                    () => _sut.GetAllStudentAnswersOnTask(taskId));

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
            const int taskId = 1;
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
                    () => _sut.GetAllStudentAnswersOnTask(taskId));

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
            const int taskId = 1;
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
            var emptyList = _sut.GetAllStudentAnswersOnTask(taskId); ;

            // Then
            Assert.AreEqual(emptyList, null);
            _studentAnswerOnTaskRepoMock.Verify(x => x.GetAllStudentAnswersOnTask(taskId), Times.Never);
            _taskRepository.Verify(x => x.GetTaskById(taskId), Times.Once);
        }
    }
}