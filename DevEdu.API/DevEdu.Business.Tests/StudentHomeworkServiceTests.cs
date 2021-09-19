using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using DevEdu.Business.Tests.TestDataHelpers;

namespace DevEdu.Business.Tests
{
    public class StudentAnswerOnTaskServiceTests
    {
        private Mock<IStudentHomeworkRepository> _studentHomeworkRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IHomeworkRepository> _homeworkRepoMock;
        private StudentHomeworkService _sut;

        [SetUp]
        public void Setup()
        {
            _studentHomeworkRepoMock = new Mock<IStudentHomeworkRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _homeworkRepoMock = new Mock<IHomeworkRepository>();
            var courseRepoMock = new Mock<ICourseRepository>();
            _sut = new StudentHomeworkService(
                   _studentHomeworkRepoMock.Object,
                   new StudentHomeworkValidationHelper(_studentHomeworkRepoMock.Object, _groupRepoMock.Object),
                   new UserValidationHelper(_userRepoMock.Object),
                   new TaskValidationHelper(_taskRepoMock.Object, _groupRepoMock.Object, courseRepoMock.Object),
                   new HomeworkValidationHelper(_homeworkRepoMock.Object)
                );
        }

        [TestCase(Role.Student)]
        public void AddStudentHomework_ExistingHomeworkIdAndStudentIdAndStudentHomeworkInputModelPassed_StudentHomeworkWasAdded(Enum role)
        {
            // Given
            var expectedDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int userId = 1;
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkDto.Id)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(GroupData.GetGroupDtos());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomework(expectedDto)).Returns(expectedDto.Id);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(expectedDto.Id)).Returns(expectedDto);

            // When
            var actualDto = _sut.AddStudentHomework(homeworkDto.Id, expectedDto, userInfo);

            // Then
            Assert.AreEqual(expectedDto, actualDto);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkDto.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomework(expectedDto), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(expectedDto.Id), Times.Once());
        }

        [Test]
        public void GetAllStudentHomework_ExistingTaskIdPassed_StudentHomeworkGotList()
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            const int taskId = 1;
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();

            _studentHomeworkRepoMock.Setup(x => x.GetAllStudentHomeworkByTask(taskId)).Returns(studentAnswersList);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            // When
            var dtoList = _sut.GetAllStudentHomeworkOnTaskAsync(taskId);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByTask(taskId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentHomeworkById_ExistingStudentHomeworkIdPassed_StudentAnswerGot(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id)).Returns(studentAnswerDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());

            // When
            var dto = _sut.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id, userInfo);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(dtoForTaskIdAndUserId.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_StatusChangeded(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int statusId = (int)StudentHomeworkStatus.Returned;
            DateTime completedDate = default;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, statusId, completedDate)).Returns(statusId);

            // When
            var actualStatusId = _sut.UpdateStatusOfStudentHomework(homeworkId, statusId, userInfo);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, statusId, completedDate), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id), Times.Exactly(2));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_CompletedDateChanged(Enum role)
        {
            // Given
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            const int homeworkId = 1;
            const int acceptedStatusId = (int)StudentHomeworkStatus.Accepted;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, acceptedStatusId, dateTime)).Returns(acceptedStatusId);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(acceptedStatusDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(acceptedStatusDto);

            // When
            var actualStatusId = _sut.UpdateStatusOfStudentHomework(homeworkId, acceptedStatusId, userInfo);
            var dto = _sut.GetStudentHomeworkById(homeworkId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTask(homeworkId, acceptedStatusId, dateTime), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(acceptedStatusDto.User.Id), Times.Exactly(4));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Exactly(countEntry));
        }

        [TestCase(Role.Student)]
        public void UpdateStudentHomework_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentHomeworkDto(Enum role)
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.UpdateStudentHomework(onlyAnswer));
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(changedStudentAnswerDto);

            // When
            var actualDto = _sut.UpdateStudentHomework(homeworkId, onlyAnswer, userInfo);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomework(onlyAnswer), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentHomeworkByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDto(Enum role)
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            const int userId = 1;
            var userDto = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetAllStudentHomeworkByStudentId(userId)).Returns(studentAnswersListDto);
            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            // When
            var dto = _sut.GetAllStudentHomeworkByStudentId(userId, userInfo);

            // Then
            Assert.AreEqual(studentAnswersListDto, dto);
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByStudentId(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void DeleteStudentHomework_ExistingStudentHomeworkId_StudentHomeworkWasDeleted(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentAnswerDto);
            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomework(homeworkId));

            // When
            _sut.DeleteStudentHomework(homeworkId, userInfo);

            // Than
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomework(homeworkId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void AddStudentHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            const int expectedStudentAnswerId = 1;
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, homeworkDto.Group.Id);

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkDto.Id)).Returns(homeworkDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(groupId)).ReturnsAsync(GroupData.GetAnotherListDtos());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomework(studentHomework)).Returns(expectedStudentAnswerId);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                () => _sut.AddStudentHomework(homeworkId, studentHomework, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkDto.Id), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Never);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomework(studentHomework), Times.Never);
        }

        [TestCase(Role.Student)]
        public void AddStudentHomework_UserInGroupNotFoundMessage_AuthorizationExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            studentHomework.User.Id = 1;
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            const int expectedStudentAnswerId = 1;
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userId, homeworkDto.Group.Id);

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkDto.Id)).Returns(homeworkDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(studentHomework.Id)).Returns(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetAnotherListDtos());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomework(studentHomework)).Returns(expectedStudentAnswerId);

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                () => _sut.AddStudentHomework(homeworkId, studentHomework, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkDto.Id), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomework(studentHomework), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void AddStudentHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), studentHomework.User.Id);

            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomework(studentHomework)).Returns(studentHomework.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddStudentHomework(homework.Id, studentHomework, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomework(studentHomework), Times.Never);
        }

        [TestCase(Role.Student)]
        public void DeleteStudentHomework_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.Id);

            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomework(studentHomework.Id));

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteStudentHomework(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomework(studentHomework.Id), Times.Never);
        }

        [TestCase(Role.Student, 2)]
        public void DeleteStudentHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentHomework);
            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomework(studentHomework.Id));

            // When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.DeleteStudentHomework(homeworkId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomework(studentHomework.Id), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentHomeworkById_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int homeworkId = 1;
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.User.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetStudentHomeworkById(homeworkId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentHomeworkById_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userInfo.UserId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(homeworkId)).Returns(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentHomeworkById(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void UpdateStudentHomework_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.Id);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.UpdateStudentHomework(homeworkId, onlyAnswer, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomework(studentHomework), Times.Never);
        }

        [TestCase(Role.Student, 2)]
        public void UpdateStudentHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkById(studentHomework.Id)).Returns(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var actualException = Assert.Throws<AuthorizationException>(
                    () => _sut.GetStudentHomeworkById(studentHomework.Id, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkById(studentHomework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomework(studentHomework), Times.Never);
        }

        [Test]
        public void GetAllStudentHomeworkByTask_WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            // Given
            const int task = 1;
            const int userId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), userId);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                    () => _sut.GetAllStudentHomeworkOnTaskAsync(task));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByTask(task), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentHomeworkByStudentId_WhenStudentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int user = 0;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user);

            // When
            var actualException = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetAllStudentHomeworkByStudentId(user, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByStudentId(user), Times.Never);
        }
    }
}