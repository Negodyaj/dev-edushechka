using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
        }

        [Test]
        public void AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.AddGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int materialId = GroupData.MaterialId;

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.RemoveGroupMaterialReference(groupId, materialId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [Test]
        public void AddTaskToGroup_GroupTaskDto_GroupTaskCreated()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.AddTaskToGroup(groupTaskDto)).Returns(GroupTaskData.ExpectedGroupTaskId);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupTaskId = sut.AddTaskToGroup(groupId, taskId, groupTaskDto);

            //Than
            Assert.AreEqual(GroupTaskData.ExpectedGroupTaskId, actualGroupTaskId);
            _groupRepoMock.Verify(x => x.AddTaskToGroup(groupTaskDto), Times.Once);
        }

        [Test]
        public void GetGroupTaskByBothId_IntGroupIdAndTaskId_ReturnedGroupTasDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var dto = sut.GetGroupTask(groupId, taskId);

            //Than
            Assert.AreEqual(groupTaskDto, dto);
            _groupRepoMock.Verify(x => x.GetGroupTask(groupId, taskId), Times.Once);
        }

        [Test]
        public void UpdateGroupTask_GroupTaskDto_ReturnUpdatedGroupTaskDto()
        {
            //Given
            var groupTaskDto = GroupTaskData.GetGroupTaskWithoutGroupAndTask();
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.UpdateGroupTask(groupTaskDto));
            _groupRepoMock.Setup(x => x.GetGroupTask(groupId, taskId)).Returns(groupTaskDto);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualGroupTaskDto = sut.UpdateGroupTask(groupId, taskId, groupTaskDto);

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
            const int groupId = GroupTaskData.GroupId;
            const int taskId = GroupTaskData.TaskId;

            _groupRepoMock.Setup(x => x.DeleteTaskFromGroup(groupId, taskId));

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            sut.DeleteTaskFromGroup(groupId, taskId);

            //Then
            _groupRepoMock.Verify(x => x.DeleteTaskFromGroup(groupId, taskId), Times.Once);
        }

        [Test]
        public void GetTasksByGroupId_IntGroupId_ReturnedListOfGroupTaskDtoWithTask()
        {
            //Given
            var groupTaskList = GroupTaskData.GetListOfGroupTaskDtoWithTask();
            const int groupId = GroupTaskData.GroupId;

            _groupRepoMock.Setup(x => x.GetTaskGroupByGroupId(groupId)).Returns(groupTaskList);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var dto = sut.GetTasksByGroupId(groupId);

            //Than
            Assert.AreEqual(groupTaskList, dto);
            _groupRepoMock.Verify(x => x.GetTaskGroupByGroupId(groupId), Times.Once);
        }
        [Test]
        public void AddGroupToLesson_IntGroupIdAndLessonId_AddLessonToGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int lessonId = GroupData.LessonId;

            _groupRepoMock.Setup(x => x.AddGroupToLesson(groupId, lessonId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.AddGroupToLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.AddGroupToLesson(groupId, lessonId), Times.Once);
        }

        [Test]
        public void RemoveGroupFromLesson_IntGroupIdAndLessonId_DeleteMaterialFromGroup()
        {
            //Given
            const int groupId = GroupData.GroupId;
            const int lessonId = GroupData.LessonId;

            _groupRepoMock.Setup(x => x.RemoveGroupFromLesson(groupId, lessonId)).Returns(GroupData.ExpectedAffectedRows);

            var sut = new GroupService(_groupRepoMock.Object);

            //When
            var actualAffectedRows = sut.RemoveGroupFromLesson(groupId, lessonId);

            //Than
            Assert.AreEqual(GroupData.ExpectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLesson(groupId, lessonId), Times.Once);
        }
    }
}