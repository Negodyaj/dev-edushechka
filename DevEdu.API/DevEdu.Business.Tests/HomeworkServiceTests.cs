using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class HomeworkServiceTests
    {
        private Mock<IHomeworkRepository> _homeworkRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private HomeworkValidationHelper _homeworkValidationHelper;
        private GroupValidationHelper _groupValidationHelper;
        private TaskValidationHelper _taskValidationHelper;
        private HomeworkService _sut;

        [SetUp]
        public void Setup()
        {
            _homeworkRepoMock = new Mock<IHomeworkRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _homeworkValidationHelper = new HomeworkValidationHelper(_homeworkRepoMock.Object);
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _taskValidationHelper = new TaskValidationHelper(_taskRepoMock.Object, _groupRepoMock.Object, _courseRepoMock.Object);
            _sut = new HomeworkService
            (
                _homeworkRepoMock.Object,
                _homeworkValidationHelper,
                _groupValidationHelper,
                _taskValidationHelper
            );
        }

        [Test]
        public void AddHomework_HomeworkDtoAndExistingGroupIdAndTaskIdPassed_HomeworkCreated()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;
            const int expectedHomeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(TaskData.GetTaskDtoWithoutTags());

            _homeworkRepoMock.Setup(x => x.AddHomeworkAsync(homeworkDto)).ReturnsAsync(expectedHomeworkId);
            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(expectedHomeworkId)).ReturnsAsync(homeworkDto);

            //When
            var actualHomeworkDto = _sut.AddHomeworkAsync(groupId, taskId, homeworkDto, userInfo);

            //Than
            Assert.AreEqual(homeworkDto, actualHomeworkDto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);

            _homeworkRepoMock.Verify(x => x.AddHomeworkAsync(homeworkDto), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(expectedHomeworkId), Times.Once);
        }

        [Test]
        public void GetHomeworkById_ExistingHomeworkIdPassed_ReturnedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkId)).ReturnsAsync(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var dto = _sut.GetHomeworkAsync(homeworkId, userInfo);

            //Than
            Assert.AreEqual(homeworkDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkId), Times.Once);
        }

        [Test]
        public void UpdateHomework_HomeworkDtoAndExistingHomeworkIdPassed_ReturnUpdatedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkId)).ReturnsAsync(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            _homeworkRepoMock.Setup(x => x.UpdateHomeworkAsync(homeworkDto));

            //When
            var actualHomeworkDto = _sut.UpdateHomeworkAsync(homeworkId, homeworkDto, userInfo);

            //Then
            Assert.AreEqual(homeworkDto, actualHomeworkDto);
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkId), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);

            _homeworkRepoMock.Verify(x => x.UpdateHomeworkAsync(homeworkDto), Times.Once);
        }

        [Test]
        public async Task DeleteHomework_ExistingHomeworkIdPassed_HomeworkRemovedAsync()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homeworkId)).ReturnsAsync(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);
            _homeworkRepoMock.Setup(x => x.DeleteHomeworkAsync(homeworkId));

            //When
            await _sut.DeleteHomeworkAsync(homeworkId, userInfo);

            //Then
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);

            _homeworkRepoMock.Verify(x => x.DeleteHomeworkAsync(homeworkId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_ExistingGroupIdPassed_ReturnedListOfHomeworkDtoByGroupId()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithTask();
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupIdAsync(groupId)).ReturnsAsync(homeworkList);

            //When
            var dto = _sut.GetHomeworkByGroupIdAsync(groupId, userInfo);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupIdAsync(groupId), Times.Once);
        }

        [Test]
        public void GetHomeworkByTaskId_ExistingTaskIdPassed_ReturnedListOfHomeworkDtoByTaskId()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithGroup();
            const int taskId = 1;

            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(TaskData.GetTaskDtoWithoutTags());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByTaskIdAsync(taskId)).ReturnsAsync(homeworkList);

            //When
            var dto = _sut.GetHomeworkByTaskIdAsync(taskId);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(taskId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByTaskIdAsync(taskId), Times.Once);
        }

        [Test]
        public void AddHomework_WhenGroupIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithoutGroupAndTask();
            var group = GroupData.GetGroupDto();
            group.Id = 0;
            var task = TaskData.GetTaskDtoWithoutTags();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(group), group.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddHomeworkAsync(group.Id, task.Id, homeworkDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void AddHomework__WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithoutGroupAndTask();
            var group = GroupData.GetGroupDto();
            var task = TaskData.GetTaskDtoWithoutTags();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), task.Id);

            _groupRepoMock.Setup(x => x.GetGroupAsync(group.Id)).ReturnsAsync(GroupData.GetGroupDto());

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddHomeworkAsync(group.Id, task.Id, homeworkDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroupAsync(group.Id), Times.Once);
        }

        [Test]
        public void AddHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithoutGroupAndTask();
            var group = GroupData.GetAnotherGroupDto();
            var task = TaskData.GetTaskDtoWithoutTags();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _groupRepoMock.Setup(x => x.GetGroupAsync(group.Id)).ReturnsAsync(GroupData.GetAnotherGroupDto());
            _taskRepoMock.Setup(x => x.GetTaskByIdAsync(task.Id)).ReturnsAsync(TaskData.GetTaskDtoWithoutTags());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddHomeworkAsync(group.Id, task.Id, homeworkDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroupAsync(group.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskByIdAsync(task.Id), Times.Once);
        }

        [Test]
        public void GetHomeworkById_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetHomeworkAsync(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void GetHomeworkById_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetAnotherHomeworkDtoWithGroupAndTask();
            var group = GroupData.GetAnotherGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homework.Id)).ReturnsAsync(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.GetHomeworkAsync(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [Test]
        public void UpdateHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateHomeworkAsync(homework.Id, homework, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void UpdateHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetAnotherHomeworkDtoWithGroupAndTask();
            var group = GroupData.GetAnotherGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homework.Id)).ReturnsAsync(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.UpdateHomeworkAsync(homework.Id, homework, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [Test]
        public void DeleteHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteHomeworkAsync(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void DeleteHomework_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetAnotherHomeworkDtoWithGroupAndTask();
            var group = GroupData.GetAnotherGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _homeworkRepoMock.Setup(x => x.GetHomeworkAsync(homework.Id)).ReturnsAsync(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.DeleteHomeworkAsync(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomeworkAsync(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_WhenGroupIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithTask();
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupIdAsync(groupId)).ReturnsAsync(homeworkList);

            //When
            var dto = _sut.GetHomeworkByGroupIdAsync(groupId, userInfo);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupIdAsync(groupId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var group = GroupData.GetAnotherGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _groupRepoMock.Setup(x => x.GetGroupAsync(group.Id)).ReturnsAsync(GroupData.GetAnotherGroupDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userInfo.UserId)).ReturnsAsync(GroupData.GetGroupDtos);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.GetHomeworkByGroupIdAsync(group.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroupAsync(group.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userInfo.UserId), Times.Once);
        }

        [Test]
        public void GetHomeworkByTaskId_WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var task = TaskData.GetTaskDtoWithoutTags();

            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), task.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetHomeworkByTaskIdAsync(task.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }
    }
}