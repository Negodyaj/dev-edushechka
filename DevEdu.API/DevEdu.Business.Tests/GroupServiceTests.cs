using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        GroupService _sut;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _sut = new GroupService(_groupRepoMock.Object);
        }

        [Test]
        public void AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.AddGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.RemoveGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;
            const int expectedGroupTaskId = 42;

            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).Returns(expectedGroupTaskId);

            //When
            var actualGroupTaskId = _sut.AddTaskToGroup(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(expectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            //When
            var dto = _sut.GetGroupTask(groupId, taskId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            //When
            var actualGroupTaskDto = _sut.UpdateGroupTask(groupId, taskId, groupTaskDto);

            //Then
            Assert.AreEqual(groupTaskDto, actualGroupTaskDto);
            _groupRepoMock.Verify(x => x.UpdateGroupTask(groupTaskDto), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void DeleteGroupTask_IntGroupIdAndTaskId_DeleteGroupTask()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = 1;
            const int taskId = 1;

            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            //When
            _sut.DeleteTaskFromGroup(groupId, taskId);

            //Then
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            const int groupId = 1;

            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).Returns(groupTaskList);

            //When
            var dto = _sut.GetTasksByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
        [Test]
        public void AddGroupToLesson_IntGroupIdAndLessonId_AddLessonToGroup()
        {
            //Given
            const int groupId = 1;
            const int lessonId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.AddGroupToLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public void RemoveGroupFromLesson_IntGroupIdAndLessonId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = 1;
            const int lessonId = 1;
            const int expectedAffectedRows = 1;

            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId)).Returns(expectedAffectedRows);

            //When
            var actualAffectedRows = _sut.RemoveGroupFromLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }
    }
}