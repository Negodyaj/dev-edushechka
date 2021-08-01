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
            _taskValidationHelper = new TaskValidationHelper(_taskRepoMock.Object);
            _sut = new HomeworkService(_homeworkRepoMock.Object,_homeworkValidationHelper,_groupValidationHelper,_taskValidationHelper);
        }

        [Test]
        public void AddHomework_HomeworkDtoAndExistingGroupIdAndTaskIdPassed_HomeworkCreated()
        {
            //Given
            var groupTaskDto = HomeworkData.GetHomeworkDtokWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;
            const int expectedGroupTaskId = 1;
            const int userId = 1;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(TaskData.GetTaskDtoWithoutTags());

            _homeworkRepoMock.Setup(x => x.AddHomework(groupTaskDto)).Returns(expectedGroupTaskId);
            _homeworkRepoMock.Setup(x => x.GetHomework(expectedGroupTaskId)).Returns(groupTaskDto);

            //When
            var actualGroupTaskDto = _sut.AddHomework(groupId, taskId, groupTaskDto, userId);

            //Than
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);

            _homeworkRepoMock.Verify(x => x.AddHomework(groupTaskDto), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(expectedGroupTaskId), Times.Once);
        }

        [Test]
        public void GetHomeworkById_ExistingHomeworkIdPassed_ReturnedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtokWithGroupAndTask();
            const int homeworkId = 1;
            const int userId = 1;
            const int groupId = 1;

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());

            //When
            var dto = _sut.GetHomework(homeworkId, userId);

            //Than
            Assert.AreEqual(homeworkDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
        }

        [Test]
        public void UpdateHomework_HomeworkDtoAndExistingHomeworkIdPassed_ReturnUpdatedHomeworkDto()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtokWithGroupAndTask();
            const int homeworkId = 1;
            const int userId = 1;
            const int groupId = 1;

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());

            _homeworkRepoMock.Setup(x => x.UpdateHomework(homeworkDto));

            //When
            var actualGroupTaskDto = _sut.UpdateHomework(homeworkId, homeworkDto, userId);

            //Then
            Assert.AreEqual(homeworkDto, actualGroupTaskDto);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);

            _homeworkRepoMock.Verify(x => x.UpdateHomework(homeworkDto), Times.Once);
        }

        [Test]
        public void DeleteHomework_ExistingHomeworkIdPassed_DeleteHomework()
        {
            //Given
            var homeworkDto = HomeworkData.GetHomeworkDtokWithGroupAndTask();
            const int homeworkId = 1;
            const int userId = 1;
            const int groupId = 1;

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(homeworkDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());

            _homeworkRepoMock.Setup(x => x.DeleteHomework(homeworkId));

            //When
            _sut.DeleteHomework(homeworkId, userId);

            //Then
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);

            _homeworkRepoMock.Verify(x => x.DeleteHomework(homeworkId), Times.Once);
        }

        [Test]
        public void GetHomeworkByGroupId_ExistingGroupIdPassed_ReturnedListOfHomeworkDtoByGroupId()
        {
            //Given
            var groupTaskList = HomeworkData.GetListOfHomeworkDtoWithTask();
            const int groupId = 1;
            const int userId = 1;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupId(groupId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetHomeworkByGroupId(groupId, userId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupId(groupId), Times.Once);
        }

        [Test]
        public void GetHomeworkByTaskId_ExistingTaskIdPassed_ReturnedListOfHomeworkDtoByTaskId()
        {
            //Given
            var groupTaskList = HomeworkData.GetListOfHomeworkDtoWithGroup();
            const int taskId = 1;
            const int userId = 1;

            _taskRepoMock.Setup(x => x.GetTaskById(taskId)).Returns(TaskData.GetTaskDtoWithoutTags());
            _homeworkRepoMock.Setup(x => x.GetHomeworkByTaskId(taskId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetHomeworkByTaskId(taskId, userId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _taskRepoMock.Verify(x => x.GetTaskById(taskId), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByTaskId(taskId), Times.Once);
        }
    }
}