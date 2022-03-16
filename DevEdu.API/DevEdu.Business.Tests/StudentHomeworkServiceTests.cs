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
using System.Threading.Tasks;
using DevEdu.Business.Tests.TestCaseSources;

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
        public async Task AddStudentHomework_ExistingHomeworkIdAndStudentIdAndStudentHomeworkInputModelPassed_StudentHomeworkWasAddedAsync(Enum role)
        {
            // Given
            var expectedDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int userId = 1;
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkDto.Id)).ReturnsAsync(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(GroupData.GetGroupDtos());
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomeworkAsync(expectedDto)).ReturnsAsync(expectedDto.Id);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(expectedDto.Id)).ReturnsAsync(expectedDto);

            // When
            var actualDto = await _sut.AddStudentHomeworkAsync(homeworkDto.Id, expectedDto, userInfo);

            // Then
            Assert.AreEqual(expectedDto, actualDto);
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkDto.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomeworkAsync(expectedDto), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(expectedDto.Id), Times.Once());
        }

        [Test]
        public async Task GetAllStudentHomework_ExistingTaskIdPassed_StudentHomeworkGotListAsync()
        {
            // Given
            var studentAnswersList = StudentAnswerOnTaskData.GetListStudentAnswersOnTaskDto();
            const int taskId = 1;
            var taskDto = TaskData.GetAnotherTaskDtoWithTags();

            _studentHomeworkRepoMock.Setup(x => x.GetAllStudentHomeworkByTaskAsync(taskId)).ReturnsAsync(studentAnswersList);
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            // When
            var dtoList = await _sut.GetAllStudentHomeworkOnTaskAsync(taskId);

            // Then
            Assert.AreEqual(studentAnswersList, dtoList);
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByTaskAsync(taskId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task GetStudentHomeworkById_ExistingStudentHomeworkIdPassed_StudentAnswerGotAsync(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var dtoForTaskIdAndUserId = StudentAnswerOnTaskData.DtoForTaskIdAndUserId();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(dtoForTaskIdAndUserId.Id)).ReturnsAsync(studentAnswerDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());

            // When
            var dto = await _sut.GetStudentHomeworkByIdAsync(dtoForTaskIdAndUserId.Id, userInfo);

            // Then
            Assert.AreEqual(studentAnswerDto, dto);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(dtoForTaskIdAndUserId.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id), Times.Exactly(2));
        }

        [TestCase(Role.Student, StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheck)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.ToFix)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.Done)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.DoneWithLate)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.ToFix)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.Done)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.DoneWithLate)]
        [TestCase(Role.Student, StudentHomeworkStatus.ToFix, StudentHomeworkStatus.OnCheckRepeat)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.ToFix)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.Done)]
        [TestCase(Role.Tutor, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.DoneWithLate)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.ToFix)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.Done)]
        [TestCase(Role.Teacher, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.DoneWithLate)]
        public async Task ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_StatusChangededAsync(Role role, 
            StudentHomeworkStatus currentStatus, StudentHomeworkStatus statusToChange)
        {
            // Given
            var studentHomeworkDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            studentHomeworkDto.StudentHomeworkStatus = currentStatus;
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock
                .Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((int)statusToChange);

            // When
            var actualStatusId = await _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, statusToChange, userInfo);

            // Then
            Assert.AreEqual(statusToChange, (StudentHomeworkStatus)actualStatusId);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id), Times.Exactly(2));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_CompletedDateChangedAsync(Role role)
        {
            // Given
            var studentHomeworkDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            const int homeworkId = 1;
            var acceptedStatus = StudentHomeworkStatus.Done;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            var groupsDto = CommentData.GetGroupsDto();
            int countGetGroupIsInvokedByMethod = 2;
            int countMethodsIsInvoked = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)acceptedStatus, dateTime, null)).ReturnsAsync((int)acceptedStatus);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id)).ReturnsAsync(groupsDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(groupsDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomeworkDto);

            // When
            var actualStatusId = await _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, acceptedStatus, userInfo);
            var dto = await _sut.GetStudentHomeworkByIdAsync(homeworkId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)acceptedStatus, dateTime, It.IsAny<DateTime>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id), Times.Exactly(countGetGroupIsInvokedByMethod * countMethodsIsInvoked));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Exactly(countMethodsIsInvoked));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_StatusMarkDoneWithLateAsync(Role role)
        {
            // Given
            var studentHomeworkDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            var groupsDto = CommentData.GetGroupsDto();
            studentHomeworkDto.Homework.EndDate = DateTime.Now.AddDays(-1);
            const int homeworkId = 1;
            var acceptedStatus = StudentHomeworkStatus.DoneWithLate;
            var sendedStatusToMethod = StudentHomeworkStatus.Done;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            studentHomeworkDto.AnswerDate = dateTime;

            int countGetGroupIsInvokedByMethod = 2;
            int countMethodsIsInvoked = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock
                .Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)acceptedStatus, It.IsAny<DateTime>(), null))
                .ReturnsAsync((int)acceptedStatus);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id)).ReturnsAsync(groupsDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(groupsDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomeworkDto);

            // When
            var actualStatusId = await _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, sendedStatusToMethod, userInfo);
            var dto = await _sut.GetStudentHomeworkByIdAsync(homeworkId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)acceptedStatus, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id), Times.Exactly(countGetGroupIsInvokedByMethod * countMethodsIsInvoked));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Exactly(countMethodsIsInvoked));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task UpdateStudentHomework_ExistingTaskIdStudentIdAndTaskAnswerDtoPassed_ReturnStudentHomeworkDtoAsync(Enum role)
        {
            // Given
            var changedStudentAnswerDto = StudentAnswerOnTaskData.GetChangedStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.UpdateStudentHomeworkAsync(onlyAnswer));
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(changedStudentAnswerDto);

            // When
            var actualDto = await _sut.UpdateStudentHomeworkAsync(homeworkId, onlyAnswer, userInfo);

            // Then
            Assert.AreEqual(changedStudentAnswerDto, actualDto);
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomeworkAsync(onlyAnswer), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Exactly(2));
        }

        [TestCase(Role.Student)]
        [TestCase(Role.Methodist)]
        public async Task GetAllStudentHomeworkByStudentId_ExistingUserIdPassed_ReturnListOfStudentAnswerOnTaskDtoAsync(Enum role)
        {
            // Given
            var studentAnswersListDto = StudentAnswerOnTaskData.GetAllAnswerOfStudent();
            const int userId = 1;
            var userDto = UserData.GetUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetAllStudentHomeworkByStudentIdAsync(userId)).ReturnsAsync(studentAnswersListDto);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            // When
            var dto = await _sut.GetAllStudentHomeworkByStudentIdAsync(userId, userInfo);

            // Then
            Assert.AreEqual(studentAnswersListDto, dto);
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByStudentIdAsync(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Student)]
        public async Task DeleteStudentHomework_ExistingStudentHomeworkId_StudentHomeworkWasDeletedAsync(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentAnswerDto);
            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomeworkAsync(homeworkId));

            // When
            await _sut.DeleteStudentHomeworkAsync(homeworkId, userInfo);

            // Than
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomeworkAsync(homeworkId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
        }


        [TestCase(Role.Student)]
        public void AddStudentHomeworkAsync_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
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

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkDto.Id)).ReturnsAsync(homeworkDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(groupId)).ReturnsAsync(GroupData.GetAnotherListDtos());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomeworkAsync(studentHomework)).ReturnsAsync(expectedStudentAnswerId);

            // When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddStudentHomeworkAsync(homeworkId, studentHomework, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkDto.Id), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Never);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomeworkAsync(studentHomework), Times.Never);
        }

        [TestCase(Role.Student)]
        public void AddStudentHomeworkAsync_UserInGroupNotFoundMessage_AuthorizationExceptionThrown(Enum role)
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

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkDto.Id)).ReturnsAsync(homeworkDto);
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(studentHomework.Id)).ReturnsAsync(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetAnotherListDtos());
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomeworkAsync(studentHomework)).ReturnsAsync(expectedStudentAnswerId);

            // When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddStudentHomeworkAsync(homeworkId, studentHomework, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkDto.Id), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomeworkAsync(studentHomework), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void AddStudentHomeworkAsync_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), studentHomework.User.Id);

            _studentHomeworkRepoMock.Setup(x => x.AddStudentHomeworkAsync(studentHomework)).ReturnsAsync(studentHomework.Id);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddStudentHomeworkAsync(homework.Id, studentHomework, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.AddStudentHomeworkAsync(studentHomework), Times.Never);
        }

        [TestCase(Role.Student)]
        public void DeleteStudentHomeworkAsync_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.Id);

            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomeworkAsync(studentHomework.Id));

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteStudentHomeworkAsync(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomeworkAsync(studentHomework.Id), Times.Never);
        }

        [TestCase(Role.Student, 2)]
        public void DeleteStudentHomeworkAsync_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomework);
            _studentHomeworkRepoMock.Setup(x => x.DeleteStudentHomeworkAsync(studentHomework.Id));

            // When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.DeleteStudentHomeworkAsync(homeworkId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.DeleteStudentHomeworkAsync(studentHomework.Id), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentHomeworkByIdAsync_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int homeworkId = 1;
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.User.Id);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetStudentHomeworkByIdAsync(homeworkId, userInfo));

            // Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetStudentHomeworkByIdAsync_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userInfo.UserId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.GetStudentHomeworkByIdAsync(homeworkId, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [TestCase(Role.Student)]
        public void UpdateStudentHomeworkAsync_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            var onlyAnswer = StudentAnswerOnTaskData.GetAnswerOfStudent();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.Id);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                    () => _sut.UpdateStudentHomeworkAsync(homeworkId, onlyAnswer, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomeworkAsync(studentHomework), Times.Never);
        }

        [TestCase(Role.Student, 2)]
        public void UpdateStudentHomeworkAsync_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var studentHomework = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(studentHomework.Id)).ReturnsAsync(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.GetStudentHomeworkByIdAsync(studentHomework.Id, userInfo));

            //Than
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(studentHomework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _studentHomeworkRepoMock.Verify(x => x.UpdateStudentHomeworkAsync(studentHomework), Times.Never);
        }

        [Test]
        public void GetAllStudentHomeworkOnTaskAsync_WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            // Given
            const int task = 1;
            const int userId = 1;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), userId);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                    () => _sut.GetAllStudentHomeworkOnTaskAsync(task));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByTaskAsync(task), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        public void GetAllStudentHomeworkByStudentIdAsync_WhenStudentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            const int user = 0;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetAllStudentHomeworkByStudentIdAsync(user, userInfo));

            // Then
            Assert.That(actualException.Message, Is.EqualTo(expectedException));
            _studentHomeworkRepoMock.Verify(x => x.GetAllStudentHomeworkByStudentIdAsync(user), Times.Never);
        }

        [TestCase(Role.Student)]
        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Methodist)]
        [TestCase(Role.Manager)]
        public void UpdateStatusOfStudentHomeworkAsync_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 1;
            var status = StudentHomeworkStatus.ToFix;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userInfo.UserId);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, status, userInfo));

            //Than
            Assert.AreEqual(expectedException, actualException.Message);
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomework.User.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void UpdateStatusOfStudentHomeworkAsync_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            // Given
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int homeworkId = 10;
            var status = StudentHomeworkStatus.ToFix;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.User.Id);

            // When
            var actualException = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, status, userInfo));

            // Than
            Assert.AreEqual(expectedException, actualException.Message);
        }

        [TestCaseSource(typeof(StudentHomewokServiceTestCaseSources),
            nameof(StudentHomewokServiceTestCaseSources.GetTestCaseDataForWrongStatusPassedConflictException))]
        public void ChangeStatusOfStudentHomework_WrongStatusPassed_ConflictExceptionThrows(        
            StudentHomeworkStatus currentStatus, StudentHomeworkStatus statusToChange, string expectedErrorMessage)
        {
            // Given
            var role = Role.Teacher;
            var studentHomeworkDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            studentHomeworkDto.StudentHomeworkStatus = currentStatus;
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock
                .Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((int)statusToChange);

            // When
            var actualException = Assert.ThrowsAsync<ConflictExpection>(
                () => _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, statusToChange, userInfo));

            // Then
            Assert.AreEqual(expectedErrorMessage, actualException.Message);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id), Times.Exactly(2));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
        }
        
        [TestCaseSource(typeof(StudentHomewokServiceTestCaseSources),
            nameof(StudentHomewokServiceTestCaseSources.GetTestCaseDataForWrongStatusPassedAuthorizationException))]
        public void ChangeStatusOfStudentHomework_WrongStatusPassed_AuthorizationExceptionThrows(Role role,
            StudentHomeworkStatus currentStatus, StudentHomeworkStatus statusToChange, string expectedErrorMessage)
        {
            // Given
            var studentHomeworkDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            studentHomeworkDto.StudentHomeworkStatus = currentStatus;
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentHomeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock
                .Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((int)statusToChange);

            // When
            var actualException = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, statusToChange, userInfo));

            // Then
            Assert.AreEqual(expectedErrorMessage, actualException.Message);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, (int)statusToChange, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentHomeworkDto.User.Id), Times.Exactly(2));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
        }
    }
}