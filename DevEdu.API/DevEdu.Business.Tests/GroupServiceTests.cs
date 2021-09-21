using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class GroupServiceTests
    {
        private Mock<IGroupRepository> _groupRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IMaterialRepository> _materialRepoMock;
        private Mock<ILessonRepository> _lessonRepoMock;
        private GroupService _sut;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _materialRepoMock = new Mock<IMaterialRepository>();
            var courseRepoMock = new Mock<ICourseRepository>();
            var taskRepoMock = new Mock<ITaskRepository>();
            var groupHelper = new GroupValidationHelper(_groupRepoMock.Object);
            var userHelper = new UserValidationHelper(_userRepoMock.Object);
            var lessonHelper = new LessonValidationHelper(_lessonRepoMock.Object, _groupRepoMock.Object);
            var materialHelper = new MaterialValidationHelper(_materialRepoMock.Object, _groupRepoMock.Object, courseRepoMock.Object);
            var taskHelper = new TaskValidationHelper(taskRepoMock.Object, _groupRepoMock.Object, courseRepoMock.Object);
            _sut = new
            (
                _groupRepoMock.Object,
                groupHelper,
                _userRepoMock.Object,
                userHelper,
                lessonHelper,
                materialHelper,
                taskHelper
            );
        }

        [Test]
        public async Task AddGroup_NoEntry_GroupCreated()
        {
            //Given
            var groupId = 2;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.AddGroupAsync(groupDto)).ReturnsAsync(groupId);

            //When
            var actualGroupId = await _sut.AddGroup(groupDto);

            //Then
            Assert.AreEqual(groupDto, actualGroupId);
            _groupRepoMock.Verify(x => x.AddGroupAsync(groupDto), Times.Once);
        }

        [Test]
        public async Task GetGroup_GroupIdForStudentRole_GroupDtoWithListOfStudentsReturned()
        {
            //Given            
            var groupId = 2;
            var groupDto = GroupData.GetGroupDto();
            var studentDtos = UserData.GetListUsersDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(It.IsAny<int>())).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRoleAsync(groupId, It.IsAny<int>())).ReturnsAsync(UserData.GetListUsersDto());

            //When
            var actualGroupDto = await _sut.GetGroup(groupId, userInfo);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroupAsync(It.IsAny<int>()), Times.AtMost(2));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRoleAsync(groupId, It.IsAny<int>()), Times.AtMost(3));
        }

        [Test]
        public async Task GetGroups_NoEntry_ListOfGroupDtoReturned()
        {
            //Given
            var groupDtos = GroupData.GetGroupDtos();

            _groupRepoMock.Setup(x => x.GetGroupsAsync()).ReturnsAsync(groupDtos);

            //When
            var actualGroupDtos = await _sut.GetGroups();

            //Then
            Assert.AreEqual(groupDtos, actualGroupDtos);
            _groupRepoMock.Verify(x => x.GetGroupsAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteGroup_GroupId_GroupDeleted()
        {
            //Given
            var groupId = 2;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.DeleteGroupAsync(groupId));

            //When
            await _sut.DeleteGroup(groupId);

            //Then
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.DeleteGroupAsync(groupId), Times.Once);
        }

        [Test]
        public async Task UpdateGroup_GroupIdAndGroupDto_UpdatedGroupDtoReturned()
        {
            //Given
            var groupId = 1;
            var groupDto = GroupData.GetGroupDtoToUpdNameAndTimetable();
            groupDto.Id = groupId;
            var updGroupDto = GroupData.GetUpdGroupDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.UpdateGroupAsync(groupDto)).ReturnsAsync(updGroupDto);

            //When
            var actualGroupDto = await _sut.UpdateGroup(groupId, groupDto, userInfo);

            //Then
            Assert.AreEqual(updGroupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.UpdateGroupAsync(groupDto), Times.Once);
        }

        [TestCase(GroupStatus.Forming)]
        public async Task ChangeGroupStatus_GroupIdAndStatusId_GroupDtoReturned(GroupStatus status)
        {
            //Given            
            var groupId = 3;
            var groupDto = GroupData.GetGroupDto();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _groupRepoMock.Setup(x => x.ChangeGroupStatusAsync(groupId, (int)status)).ReturnsAsync(groupDto);

            //When
            var actualGroupDto = await _sut.ChangeGroupStatus(groupId, status);

            //Then
            Assert.AreEqual(groupDto, actualGroupDto);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.ChangeGroupStatusAsync(groupId, (int)status), Times.Once);
        }

        [Test]
        public async Task AddGroupToLesson_GroupIdAndLessonId_GroupLessonReferenceCreated()
        {
            //Given
            var groupId = 1;
            var lessonId = 2;
            var userInfo = GroupData.GetUserInfo();
            var groupDto = GroupData.GetGroupDto();
            var lessonDto = LessonData.GetSelectedLessonDto();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonByIdAsync(lessonId)).ReturnsAsync(lessonDto);
            _groupRepoMock.Setup(x => x.AddGroupToLessonAsync(groupId, lessonId));

            //When
            await _sut.AddGroupToLesson(groupId, lessonId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupToLessonAsync(groupId, lessonId), Times.Once);
        }

        [Test]
        public async Task RemoveGroupFromLesson_GroupIdAndLessonId_GroupLessonReferenceDeleted()
        {
            //Given
            var groupId = 1;
            var lessonId = 2;
            var groupDto = GroupData.GetGroupDto();
            var lessonDto = LessonData.GetSelectedLessonDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _lessonRepoMock.Setup(x => x.SelectLessonByIdAsync(lessonId)).ReturnsAsync(lessonDto);
            _groupRepoMock.Setup(x => x.RemoveGroupFromLessonAsync(groupId, lessonId));

            //When
            await _sut.RemoveGroupFromLesson(groupId, lessonId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLessonAsync(groupId, lessonId), Times.Once);
        }


        [Test]
        public async Task AddMaterialToGroup_GroupIdAndMaterialId_GroupMaterialReferenceCreated()
        {
            //Given
            var groupId = 2;
            var materialId = 2;
            var expectedAffectedRows = 3;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialId)).ReturnsAsync(materialDto);
            _groupRepoMock.Setup(x => x.AddGroupMaterialReferenceAsync(groupId, materialId)).ReturnsAsync(expectedAffectedRows);

            //When
            var actualAffectedRows = await _sut.AddGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);

            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.AddGroupMaterialReferenceAsync(groupId, materialId), Times.Once);
        }

        [Test]
        public async Task DeleteMaterialFromGroup_GroupIdAndMaterialId_GroupMaterialReferenceDeleted()
        {
            //Given
            var groupId = 2;
            var materialId = 2;
            var expectedAffectedRows = 3;
            var groupDto = GroupData.GetGroupDto();
            var materialDto = MaterialData.GetMaterialDtoWithTags();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _materialRepoMock.Setup(x => x.GetMaterialByIdAsync(materialId)).ReturnsAsync(materialDto);
            _groupRepoMock.Setup(x => x.RemoveGroupMaterialReferenceAsync(groupId, materialId)).ReturnsAsync(expectedAffectedRows);

            //When
            var actualAffectedRows = await _sut.RemoveGroupMaterialReference(groupId, materialId, userInfo);

            //Than
            Assert.AreEqual(expectedAffectedRows, actualAffectedRows);
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _materialRepoMock.Verify(x => x.GetMaterialByIdAsync(materialId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupMaterialReferenceAsync(groupId, materialId), Times.Once);
        }

        [TestCase(Role.Student)]
        public async Task AddUserToGroup_GroupIdLessonIdAndRoleId_UserGroupReferenceCreated(Role role)
        {
            //Given
            var groupId = 2;
            var userId = 3;
            var groupDto = GroupData.GetGroupDto();
            var userDto = UserData.GetUserDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(userDto); ;
            _groupRepoMock.Setup(x => x.AddUserToGroupAsync(groupId, userId, (int)role));

            //When
            await _sut.AddUserToGroup(groupId, userId, role, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _groupRepoMock.Verify(x => x.AddUserToGroupAsync(groupId, userId, (int)role), Times.Once);
        }

        [Test]
        public async Task DeleteUserFromGroup_GroupIdAndUserId_UserGroupReferenceDeleted()
        {
            //Given
            var groupId = 2;
            var userId = 2;
            var groupDto = GroupData.GetGroupDto();
            var userDto = UserData.GetUserDto();
            var userInfo = GroupData.GetUserInfo();

            _groupRepoMock.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(groupDto);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(userDto);
            _groupRepoMock.Setup(x => x.DeleteUserFromGroupAsync(groupId, userId));

            //When
            await _sut.DeleteUserFromGroup(groupId, userId, userInfo);

            //Then
            _groupRepoMock.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _groupRepoMock.Verify(x => x.RemoveGroupFromLessonAsync(groupId, userId), Times.Never);
        }
    }
}