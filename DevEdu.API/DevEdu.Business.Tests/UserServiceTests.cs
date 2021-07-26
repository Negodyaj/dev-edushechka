using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class UserServiceTests
    {

        private Mock<IUserRepository> _userRepoMock;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
        }

        [Test]
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var user = UserData.GetUserDto();

            _userRepoMock.Setup(x => x.AddUser(user)).Returns(UserData.expectedUserId);
            _userRepoMock.Setup(x => x.AddUserRole(UserData.expectedUserId, It.IsAny<int>()));

            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualId = sut.AddUser(user);

            //Then
            Assert.AreEqual(UserData.expectedUserId, actualId);
            _userRepoMock.Verify(x => x.AddUser(user), Times.Once);
            _userRepoMock.Verify(x => x.AddUserRole(actualId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _userRepoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(expectedDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualDto = sut.SelectUserById(UserData.expectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _userRepoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.Once);
        }

        [Test]
        public void SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedList = UserData.GetListUsersDto();
            _userRepoMock.Setup(x => x.SelectUsers()).Returns(expectedList);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualList = sut.SelectUsers();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _userRepoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var expectedAnotherDto = UserData.GetAnotherUserDto();

            _userRepoMock.Setup(x => x.UpdateUser(expectedDto));
            _userRepoMock.Setup(x => x.SelectUserById(expectedDto.Id)).Returns(expectedAnotherDto);

            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualDto = sut.UpdateUser(expectedDto);

            //Then
            Assert.AreEqual(expectedAnotherDto, actualDto);
            _userRepoMock.Verify(x => x.UpdateUser(expectedDto), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(expectedDto.Id), Times.Once);
        }
    }
}