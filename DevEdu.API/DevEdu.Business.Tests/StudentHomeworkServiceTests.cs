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

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_StatusChangededAsync(Enum role)
        {
            // Given
            var studentAnswerDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskDto();
            const int homeworkId = 1;
            const int statusId = (int)StudentHomeworkStatus.Returned;
            DateTime completedDate = default;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(studentAnswerDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, statusId, completedDate)).ReturnsAsync(statusId);

            // When
            var actualStatusId = await _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, statusId, userInfo);

            // Then
            Assert.AreEqual(statusId, actualStatusId);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, statusId, completedDate), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentAnswerDto.User.Id), Times.Exactly(2));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public async Task ChangeStatusOfStudentHomework_ExistingStudentHomeworkIdPassed_CompletedDateChangedAsync(Enum role)
        {
            // Given
            var acceptedStatusDto = StudentAnswerOnTaskData.GetStudentAnswerOnTaskWithAcceptedTaskStatusDto();
            const int homeworkId = 1;
            const int acceptedStatusId = (int)StudentHomeworkStatus.Accepted;
            DateTime dateTime = DateTime.Now;
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            int countEntry = 2;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _studentHomeworkRepoMock.Setup(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, acceptedStatusId, dateTime)).ReturnsAsync(acceptedStatusId);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(acceptedStatusDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(CommentData.GetGroupsDto());
            _studentHomeworkRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(homeworkId)).ReturnsAsync(acceptedStatusDto);

            // When
            var actualStatusId = await _sut.UpdateStatusOfStudentHomeworkAsync(homeworkId, acceptedStatusId, userInfo);
            var dto = await _sut.GetStudentHomeworkByIdAsync(homeworkId, userInfo);

            // Then
            Assert.AreEqual(dateTime, dto.CompletedDate);
            _studentHomeworkRepoMock.Verify(x => x.ChangeStatusOfStudentAnswerOnTaskAsync(homeworkId, acceptedStatusId, dateTime), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(acceptedStatusDto.User.Id), Times.Exactly(4));
            _studentHomeworkRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(homeworkId), Times.Exactly(countEntry));
        }

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

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
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
        public void AddStudentHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
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
        public void DeleteStudentHomework_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
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
        public void DeleteStudentHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
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
        public void GetStudentHomeworkById_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
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
        public void GetStudentHomeworkById_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
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
        public void UpdateStudentHomework_WhenStudentHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
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
        public void UpdateStudentHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
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
        public void GetAllStudentHomeworkByTask_WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
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
        public void GetAllStudentHomeworkByStudentId_WhenStudentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
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
    }
}