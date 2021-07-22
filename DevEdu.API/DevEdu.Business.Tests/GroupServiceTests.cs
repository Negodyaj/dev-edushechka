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
    }
}