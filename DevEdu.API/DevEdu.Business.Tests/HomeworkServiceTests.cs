using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class HomeworkServiceTests
    {
        private Mock<IHomeworkRepository> _homeworkRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<ITaskRepository> _taskRepoMock;
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
            _homeworkValidationHelper = new HomeworkValidationHelper(_homeworkRepoMock.Object);
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _taskValidationHelper = new TaskValidationHelper(_taskRepoMock.Object, _groupRepoMock.Object);
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

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(TaskData.GetTaskDtoWithoutTags());

            _homeworkRepoMock.Setup(x => x.AddHomework(homeworkDto)).Returns(expectedHomeworkId);
            _homeworkRepoMock.Setup(x => x.GetHomework(expectedHomeworkId)).Returns(homeworkDto);

            //When
            var actualHomeworkDto = _sut.AddHomework(groupId, taskId, homeworkDto, userInfo);

            //Than
            Assert.AreEqual(homeworkDto, actualHomeworkDto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);

            _homeworkRepoMock.Verify(x => x.AddHomework(homeworkDto), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(expectedHomeworkId), Times.Once);
        }

        [Test]
        public void GetHomeworkById_ExistingHomeworkIdPassed_ReturnedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var dto = _sut.GetHomework(homeworkId, userInfo);

            //Than
            Assert.AreEqual(homeworkDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
        }

        [Test]
        public void UpdateHomework_HomeworkDtoAndExistingHomeworkIdPassed_ReturnUpdatedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            _homeworkRepoMock.Setup(x => x.UpdateHomework(homeworkDto));

            //When
            var actualHomeworkDto = _sut.UpdateHomework(homeworkId, homeworkDto, userInfo);

            //Then
            Assert.AreEqual(homeworkDto, actualHomeworkDto);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);

            _homeworkRepoMock.Verify(x => x.UpdateHomework(homeworkDto), Times.Once);
        }

        [Test]
        public void DeleteHomework_ExistingHomeworkIdPassed_HomeworkRemoved()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            const int homeworkId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            _homeworkRepoMock.Setup(x => x.DeleteHomework(homeworkId));

            //When
            _sut.DeleteHomework(homeworkId, userInfo);

            //Then
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);

            _homeworkRepoMock.Verify(x => x.DeleteHomework(homeworkId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_ExistingGroupIdPassed_ReturnedListOfHomeworkDtoByGroupId()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithTask();
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupId(groupId)).Returns(homeworkList);

            //When
            var dto = _sut.GetHomeworkByGroupId(groupId, userInfo);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupId(groupId), Times.Once);
        }

        [Test]
        public void GetHomeworkByTaskId_ExistingTaskIdPassed_ReturnedListOfHomeworkDtoByTaskId()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithGroup();
            const int taskId = 1;

            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(TaskData.GetTaskDtoWithoutTags());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByTaskId(taskId)).Returns(homeworkList);

            //When
            var dto = _sut.GetHomeworkByTaskId(taskId);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByTaskId(taskId), Times.Once);
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
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddHomework(group.Id, task.Id, homeworkDto, userInfo));

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

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).ReturnsAsync(GroupData.GetGroupDto());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddHomework(group.Id, task.Id, homeworkDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Once);
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

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).ReturnsAsync(GroupData.GetAnotherGroupDto());
            _taskRepoMock.Setup(x => x.GetTaskById(task.Id)).Returns(TaskData.GetTaskDtoWithoutTags());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.AddHomework(group.Id, task.Id, homeworkDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
            _taskRepoMock.Verify(x => x.GetTaskById(task.Id), Times.Once);
        }

        [Test]
        public void GetHomeworkById_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetHomework(homework.Id, userInfo));

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

            _homeworkRepoMock.Setup(x => x.GetHomework(homework.Id)).Returns(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.GetHomework(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomework(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
        }

        [Test]
        public void UpdateHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.UpdateHomework(homework.Id, homework, userInfo));

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

            _homeworkRepoMock.Setup(x => x.GetHomework(homework.Id)).Returns(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.UpdateHomework(homework.Id, homework, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomework(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
        }

        [Test]
        public void DeleteHomework_WhenHomeworkIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homework = HomeworkData.GetHomeworkDtoWithGroupAndTask();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(homework), homework.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteHomework(homework.Id, userInfo));

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

            _homeworkRepoMock.Setup(x => x.GetHomework(homework.Id)).Returns(homework);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.DeleteHomework(homework.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _homeworkRepoMock.Verify(x => x.GetHomework(homework.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_WhenGroupIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var homeworkList = HomeworkData.GetListOfHomeworkDtoWithTask();
            const int groupId = 1;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).ReturnsAsync(GroupData.GetGroupDto());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupId(groupId)).Returns(homeworkList);

            //When
            var dto = _sut.GetHomeworkByGroupId(groupId, userInfo);

            //Than
            Assert.AreEqual(homeworkList, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupId(groupId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown()
        {
            //Given
            var group = GroupData.GetAnotherGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var expectedException = string.Format(ServiceMessages.UserInGroupNotFoundMessage, userInfo.UserId, group.Id);

            _groupRepoMock.Setup(x => x.GetGroup(group.Id)).ReturnsAsync(GroupData.GetAnotherGroupDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userInfo.UserId)).Returns(GroupData.GetGroupDtos);

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.GetHomeworkByGroupId(group.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepoMock.Verify(x => x.GetGroup(group.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userInfo.UserId), Times.Once);
        }

        [Test]
        public void GetHomeworkByTaskId_WhenTaskIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var task = TaskData.GetTaskDtoWithoutTags();

            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), task.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetHomeworkByTaskId(task.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }
    }
}