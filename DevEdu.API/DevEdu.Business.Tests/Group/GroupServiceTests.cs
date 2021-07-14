using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

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
        public void AddGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        {
            //Given
            _mock.Setup(mock => mock.AddGroup(dto)).Returns(expectedDto).Verifiable();

            var _service = new GroupService(_mock.Object);
            
            //When
            var actual = _service.AddGroup(dto);

            //Then
            Assert.AreEqual(expected, actual);
            _mock.Verify(mock => mock.AddGroup(dto), Times.Once);
        }

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.DeleteGroup))]
        //public void DeleteGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.DeleteGroup(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.DeleteGroup(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.DeleteGroup(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.GetGroup))]
        //public void GetGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.GetGroup(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.GetGroup(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.GetGroup(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.GetGroups))]
        //public void GetGroups(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.GetGroups(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.GetGroups(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.GetGroups(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.AddGroupLesson))]
        //public void AddGroupLesson(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.AddGroupLesson(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.AddGroupLesson(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.AddGroupLesson(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.RemoveGroupLesson))]
        //public void RemoveGroupLesson(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.RemoveGroupLesson(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.RemoveGroupLesson(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.RemoveGroupLesson(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.UpdateGroup))]
        //public void UpdateGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.UpdateGroup(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.UpdateGroup(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.UpdateGroup(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.AddGroupMaterialReference))]
        //public void AddGroupMaterialReference(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.AddGroupMaterialReference(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.AddGroupMaterialReference(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.AddGroupMaterialReference(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.RemoveGroupMaterialReference))]
        //public void RemoveGroupMaterialReference(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.RemoveGroupMaterialReference(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.RemoveGroupMaterialReference(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.RemoveGroupMaterialReference(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.AddUserToGroup))]
        //public void AddUserToGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.AddUserToGroup(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.AddUserToGroup(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.AddUserToGroup(dto), Times.Once);
        //}

        //[TestCaseSource(typeof(GroupServiceExpecteds), nameof(GroupServiceExpecteds.DeleteUserFromGroup))]
        //public void DeleteUserFromGroup(GroupDto dto, GroupDto expectedDto, GroupDto expected)
        //{
        //    //Given
        //    _mock.Setup(mock => mock.DeleteUserFromGroup(dto))
        //        .Returns(expectedDto)
        //        .Verifiable();

        //    var _service = new GroupService(_mock.Object);

        //    //When
        //    var actual = _service.DeleteUserFromGroup(dto);

        //    //Then
        //    Assert.AreEqual(expected, actual);
        //    _mock.Verify(mock => mock.DeleteUserFromGroup(dto), Times.Once);
        //}
    }
}