using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class HomeworkServiceTests
    {
        private Mock<IHomeworkRepository> _homeworkRepoMock;
        private HomeworkService _sut;

        [SetUp]
        public void Setup()
        {
            _homeworkRepoMock = new Mock<IHomeworkRepository>();
            _sut = new HomeworkService(_homeworkRepoMock.Object);
        }

        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = HomeworkData.GetGroupTaskWithoutGroupAndTask();
            var groupId = 1;
            var taskId = 2;
            var expectedGroupTaskId = 1;

            _homeworkRepoMock.Setup(x => x.AddHomework(groupTaskDto)).Returns(expectedGroupTaskId);
            _homeworkRepoMock.Setup(x => x.GetHomework(expectedGroupTaskId)).Returns(groupTaskDto);

            //When
            var actualGroupTaskDto = _sut.AddHomework(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _homeworkRepoMock.Verify(x => x.AddHomework(groupTaskDto), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(expectedGroupTaskId), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = HomeworkData.GetGroupTaskWithGroupAndTask();
            var homeworkId = 1;

            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(groupTaskDto);

            //When
            var dto = _sut.GetHomework(homeworkId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = HomeworkData.GetGroupTaskWithoutGroupAndTask();
            var homeworkId = 1;

            _homeworkRepoMock.Setup(x => x.UpdateHomework(groupTaskDto));
            _homeworkRepoMock.Setup(x => x.GetHomework(homeworkId)).Returns(groupTaskDto);

            //When
            var actualGroupTaskDto = _sut.UpdateHomework(homeworkId, groupTaskDto);

            //Then
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _homeworkRepoMock.Verify(x => x.UpdateHomework(groupTaskDto), Times.Once);
            _homeworkRepoMock.Verify(x => x.GetHomework(homeworkId), Times.Once);
        }

        [Test]
        public void DeleteGroupTask_IntGroupIdAndTaskId_DeleteGroupTask()
        {
            //Given
            var homeworkId = 1;

            _homeworkRepoMock.Setup(x => x.DeleteHomework(homeworkId));

            //When
            _sut.DeleteHomework(homeworkId);

            //Then
            _homeworkRepoMock.Verify(x => x.DeleteHomework(homeworkId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = HomeworkData.GetListOfGroupTaskDtoWithTask();
            var groupId = 1;

            _homeworkRepoMock.Setup(x => x.GetHomeworkByGroupId(groupId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetHomeworkByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByGroupId(groupId), Times.Once);
        }

        [Test]
        public void GetGroupsByTaskId_IntTaskId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = HomeworkData.GetListOfGroupTaskDtoWithGroup();
            var taskId = 2;

            _homeworkRepoMock.Setup(x => x.GetHomeworkByTaskId(taskId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetHomeworkByTaskId(taskId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _homeworkRepoMock.Verify(x => x.GetHomeworkByTaskId(taskId), Times.Once);
        }
    }
}