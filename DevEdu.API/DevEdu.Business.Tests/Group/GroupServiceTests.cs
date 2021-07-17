using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.Group
{
    public class GroupServiceTests
    {

        private Mock<IGroupRepository> _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new Mock<IGroupRepository>();
        }

        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.AddGroup))]
        public void AddGroup(GroupDto dto, GroupDto expected)
        {
            //Given
            _mock.Setup(mock => mock.AddGroup(dto)).Returns(expected);
            var _service = new GroupService(_mock.Object);
            
            //When
            var actual = _service.AddGroup(dto);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.AddGroup(dto), Times.Once);
        }

        
        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.GetGroup))]
        public void GetGroup(int id, GroupDto expected)
        {
            //Given
            _mock.Setup(mock => mock.GetGroup(id)).Returns(expected);

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.GetGroup(id);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.GetGroup(id), Times.Once);
        }

        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.GetGroups))]
        public void GetGroups(List<GroupDto> expected)
        {
            //Given
            _mock.Setup(mock => mock.GetGroups())
                .Returns(expected)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.GetGroups();

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.GetGroups(), Times.Once);
        }        

        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.UpdateGroup))]
        public void UpdateGroup(int id, GroupDto dto, GroupDto expected)
        {
            //Given
            _mock.Setup(mock => mock.UpdateGroup(id, dto))
                .Returns(expected)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.UpdateGroup(id, dto);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.UpdateGroup(id, dto), Times.Once);
        }

        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.AddGroupMaterialReference))]
        public void AddGroupMaterialReference(int groupId, int materialId, int expected)
        {
            //Given
            _mock.Setup(mock => mock.AddGroupMaterialReference(groupId, materialId))
                .Returns(expected)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.AddGroupMaterialReference(groupId, materialId);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.RemoveGroupMaterialReference))]
        public void RemoveGroupMaterialReference(int groupId, int materialId, int expected)
        {
            //Given
            _mock.Setup(mock => mock.RemoveGroupMaterialReference(groupId, materialId))
                .Returns(expected)
                .Verifiable();

            var _service = new GroupService(_mock.Object);

            //When
            var actual = _service.RemoveGroupMaterialReference(groupId, materialId);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }
    }
}