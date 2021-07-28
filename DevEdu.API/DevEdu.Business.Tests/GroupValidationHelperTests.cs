using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class GroupValidationHelperTests
    {
        private Mock<IGroupRepository> _groupRepoMock;

        [SetUp]
        public void Setup()
        {
            _groupRepoMock = new Mock<IGroupRepository>();
        }

        [Test]
        public void CheckUserBelongToGroup_GroupId_UserId_Role_ValidationException()
        {
            //Given
            int userGroupId = default;
            var groupId = 0;
            var userId = 0;
            var role = Role.Student;

            _groupRepoMock.Setup(x => x.GetUser_GroupByUserIdAndUserRoleAndGroupId(userId, role, groupId)).Returns(userGroupId);

            var sut = new GroupValidationHelper(_groupRepoMock.Object);

            //When

            //Than
            Assert.Throws<ValidationException>(() => sut.CheckUserBelongToGroup(groupId, userId, role));
            _groupRepoMock.Verify(x => x.GetUser_GroupByUserIdAndUserRoleAndGroupId(userId, role, groupId), Times.Exactly(1));
        }

        [Test]
        public void CheckTeacherBelongToGroup_GroupId_TeacherId_Role_AuthorizationException()
        {
            //Given
            int userGroupId = default;
            var groupId = 0;
            var teacherId = 0;
            var role = Role.Student;

            _groupRepoMock.Setup(x => x.GetUser_GroupByUserIdAndUserRoleAndGroupId(teacherId, role, groupId)).Returns(userGroupId);

            var sut = new GroupValidationHelper(_groupRepoMock.Object);

            //When

            //Than
            Assert.Throws<AuthorizationException>(() => sut.CheckTeacherBelongToGroup(groupId, teacherId, role));
            _groupRepoMock.Verify(x => x.GetUser_GroupByUserIdAndUserRoleAndGroupId(teacherId, role, groupId), Times.Exactly(1));
        }
    }
}
