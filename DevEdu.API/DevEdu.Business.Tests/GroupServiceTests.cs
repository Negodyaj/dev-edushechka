﻿using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private GroupValidationHelper _groupValidationHelper;
        private MaterialValidationHelper _materialValidationHelper;
        private GroupService _sut;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _materialRepoMock = new Mock<IMaterialRepository>();
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _materialValidationHelper = new MaterialValidationHelper(_materialRepoMock.Object);
            _sut = new GroupService(_groupRepoMock.Object, _userRepoMock.Object,_groupValidationHelper,_materialValidationHelper);
        }

        [Test]
        public void AddGroup_NotParams_ReturnGroupId()
        {
            //Given            
            var groupDto = GroupData.GetGroupDto();
            var groupId = GroupData.GroupId;

            _groupRepoMock.Setup(x => x.AddGroup(groupDto)).Returns(groupId);

            //When
            var actualGroupId = _sut.AddGroup(groupDto);

            //Then
            Assert.AreEqual(groupId, actualGroupId);
            _groupRepoMock.Verify(x => x.AddGroup(groupDto), Times.Once);
        }

        [Test]
        public void GetGroupWithListStudents_ByIdAndByRoleStudent_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var roleStudent = GroupData.RoleStudent;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = GroupData.GetUserForGroup();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(groupDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, roleStudent)).Returns(studentDtos);
            groupDto.Students = studentDtos;

            //When
            var actualGroupDto = _sut.GetGroup(groupId);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, roleStudent), Times.Once);
        }

        [Test]
        public void GetGroups_NotParams_ReturnListGroupDto()
        {
            //Given
            var groupDtos = GroupData.GetGroupsDto();

            _groupRepoMock.Setup(x => x.GetGroups()).Returns(groupDtos);

            //When
            var actualGroupDtos = _sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroups(), Times.Once);
        }

        [Test]
        public void UpdateGroupNameAndTimetable_ByIdAndGroupDto_ReturnGroupDto()
        {
            //Given
            var groupId = GroupData.GroupId;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            var updGroupDto = GroupData.GetUpdGroupDto();

            _groupRepoMock.Setup(x => x.UpdateGroup(groupId, groupDto)).Returns(updGroupDto);

            //When
            var actualGroupDto = _sut.UpdateGroup(groupId, groupDto);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.UpdateGroup(groupId, groupDto), Times.Once);
        }

        [Test]
        public void ChangeGroupStatus_ByGroupIdAndStatusId_ReturnGroupDto()
        {
            //Given            
            var groupId = GroupData.GroupId;
            var groupStatus = GroupData.StatusGroup;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.ChangeGroupStatus(groupId, groupStatus)).Returns(groupDto);

            //When
            var actualGroupDto = _sut.ChangeGroupStatus(groupId, groupStatus);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.ChangeGroupStatus(groupId, groupStatus), Times.Once);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void AddMaterialToGroup_IntGroupIdAndMaterialId_AddMaterialToGroup(int role)
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            var userToken = UserTokenData.GetUserTokenWithCustomRole(role);
            var userId = userToken.UserId;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);

            _groupRepoMock.Setup(x => x.AddGroupMaterialReference(groupId, materialId));

            //When
            _sut.AddGroupMaterialReference(groupId, materialId, userToken);

            //Than
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);

            _groupRepoMock.Verify(x => x.AddGroupMaterialReference(groupId, materialId), Times.Once);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void DeleteMaterialFromGroup_IntGroupIdAndMaterialId_DeleteMaterialFromGroup(int role)
        {
            //Given
            const int groupId = 1;
            const int materialId = 1;
            var userToken = UserTokenData.GetUserTokenWithCustomRole(role);
            var userId = userToken.UserId;

            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(GroupData.GetGroupDto());
            _materialRepoMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);

            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReference(groupId, materialId));

            //When
            _sut.RemoveGroupMaterialReference(groupId, materialId, userToken);

            //Than
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Exactly(2));
            _materialRepoMock.Verify(x => x.GetMaterialById(materialId), Times.Once);

            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReference(groupId, materialId), Times.Once);
        }
    }
}