using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepoMock;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
        }

        [Test]
        public void GetGroupsByTaskId_IntTaskId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithGroup();
            const int taskId = GroupTaskData.TaskId;

            _taskRepoMock.Setup(x => x.GetGroupsByTaskId(taskId)).Returns(groupTaskList);

            var sut = new TaskService(_taskRepoMock.Object);

            //When
            var dto = sut.GetGroupsByTaskId(taskId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _taskRepoMock.Verify(x => x.GetGroupsByTaskId(taskId), Times.Once);
        }
    }
}