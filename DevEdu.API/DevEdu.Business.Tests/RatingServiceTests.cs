using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class RatingServiceTests
    {
        private Mock<IRatingRepository> _ratingRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private IRatingValidationHelper _ratingValidationHelper;
        private IUserValidationHelper _userValidationHelper;
        private IGroupValidationHelper _groupValidationHelper;
        private RatingService _sut;

        [SetUp]
        public void Setup()
        {
            _ratingRepoMock = new Mock<IRatingRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _ratingValidationHelper = new RatingValidationHelper(_ratingRepoMock.Object);
            _groupValidationHelper = new GroupValidationHelper(_groupRepoMock.Object);
            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);
            _sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelper, _groupValidationHelper, _userValidationHelper);
        }

        [Test]
        public void AddStudentRating_AuthorUserIsTeacher_StudentRatingCreated()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var expectedStudentRatingId = expectedStudentRatingDto.Id;
            var inputStudentRatingDto = RatingData.GetInputStudentRatingDto();
            var group = inputStudentRatingDto.Group;
            var groupId = group.Id;
            var student = inputStudentRatingDto.User;
            var studentId = student.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);
            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student)).Returns(usersInGroup);
            _ratingRepoMock.Setup(x => x.AddStudentRating(expectedStudentRatingDto)).Returns(expectedStudentRatingId);
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(expectedStudentRatingId)).Returns(expectedStudentRatingDto);

            //When
            var actualStudentRatingDto = _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Once);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(expectedStudentRatingId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
        }

        [Test]
        public void AddStudentRating_AuthorUserIsAdmin_StudentRatingCreated()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var expectedStudentRatingId = expectedStudentRatingDto.Id;
            var inputStudentRatingDto = RatingData.GetInputStudentRatingDto();
            var group = inputStudentRatingDto.Group;
            var groupId = group.Id;
            var student = inputStudentRatingDto.User;
            var studentId = student.Id;
            var authorUserInfo = RatingData.GetAdminIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);
            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student)).Returns(usersInGroup);
            _ratingRepoMock.Setup(x => x.AddStudentRating(expectedStudentRatingDto)).Returns(expectedStudentRatingId);
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(expectedStudentRatingId)).Returns(expectedStudentRatingDto);

            //When
            var actualStudentRatingDto = _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Once);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(expectedStudentRatingId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
        }

        [Test]
        public void AddStudentRating_GroupDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var studentId = expectedStudentRatingDto.User.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var groupId = expectedStudentRatingDto.Group.Id;
            GroupDto group = default;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Never);
        }

        [Test]
        public void AddStudentRating_TeacherDoesntAuthorizeToGroup_AuthorizationExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var studentId = expectedStudentRatingDto.User.Id;
            var authorUserInfo = RatingData.GetTeacherOutOfGroupIdentityInfo();
            var group = expectedStudentRatingDto.Group;
            var groupId = group.Id;
            var usersInGroup = UserData.GetListUsersDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            Assert.Throws<AuthorizationException>(() => _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Never);
        }

        [Test]
        public void AddStudentRating_UserDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var studentId = expectedStudentRatingDto.User.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var group = expectedStudentRatingDto.Group;
            var groupId = group.Id;
            UserDto student = default;
            var usersInGroup = UserData.GetListUsersDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
        }

        [Test]
        public void AddStudentRating_StudentDoesntBelongToGroup_ValidationExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var student = expectedStudentRatingDto.User;
            var studentId = student.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var group = expectedStudentRatingDto.Group;
            var groupId = group.Id;
            var usersInGroup = UserData.GetListUsersDto();

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);
            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student)).Returns(usersInGroup);

            //When
            Assert.Throws<ValidationException>(() => _sut.AddStudentRating(expectedStudentRatingDto, authorUserInfo));

            //Than
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Student), Times.Once);
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Never);
        }

        [Test]
        public void DeleteStudentRating_AuthorUserIsTeacher_StudentRatingDeleted()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var groupId = expectedStudentRatingDto.Group.Id;
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            _sut.DeleteStudentRating(studentRatingId, authorUserInfo);

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(studentRatingId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
        }

        [Test]
        public void DeleteStudentRating_AuthorUserIsAdmin_StudentRatingDeleted()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var authorUserInfo = RatingData.GetAdminIdentityInfo();
            var groupId = expectedStudentRatingDto.Group.Id;
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            _sut.DeleteStudentRating(studentRatingId, authorUserInfo);

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(studentRatingId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteStudentRating_TeacherDoesntAuthorizeToGroup_AuthorizationExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var authorUserInfo = RatingData.GetTeacherOutOfGroupIdentityInfo();
            var groupId = expectedStudentRatingDto.Group.Id;
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            Assert.Throws<AuthorizationException>(() => _sut.DeleteStudentRating(studentRatingId, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(studentRatingId), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
        }

        [Test]
        public void DeleteStudentRating_EntityDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            StudentRatingDto expectedStudentRatingDto = default;
            var studentRatingId = UserData.GetUserDto().Id;
            var authorUserInfo = RatingData.GetTeacherOutOfGroupIdentityInfo();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.DeleteStudentRating(studentRatingId, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetAllStudentRatings_NoEntries_ListOfStudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();

            _ratingRepoMock.Setup(x => x.SelectAllStudentRatings()).Returns(expectedStudentRatingDtos);

            //When
            var actualStudentRatingDtos = _sut.GetAllStudentRatings();

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectAllStudentRatings(), Times.Once);
        }

        [Test]
        public void GetStudentRatingByUserId_UserId_ListOfStudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var student = expectedStudentRatingDtos[0].User;
            var studentId = student.Id;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByUserId(studentId)).Returns(expectedStudentRatingDtos);
            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);

            //When
            var actualStudentRatingDtos = _sut.GetStudentRatingByUserId(studentId);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByUserId(studentId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
        }

        [Test]
        public void GetStudentRatingByUserId_UserDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var studentId = expectedStudentRatingDtos[0].User.Id;
            UserDto student = default;

            _userRepoMock.Setup(x => x.SelectUserById(studentId)).Returns(student);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.GetStudentRatingByUserId(studentId));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByUserId(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.SelectUserById(studentId), Times.Once);
        }

        [Test]
        public void GetStudentRatingByGroupId_AuthorUserIsManager_ListOfStudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var groupId = expectedStudentRatingDtos[0].Group.Id;
            var authorUserInfo = RatingData.GetManagerIdentityInfo();
            var group = expectedStudentRatingDtos[0].Group;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByGroupId(groupId)).Returns(expectedStudentRatingDtos);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            var actualStudentRatingDtos = _sut.GetStudentRatingByGroupId(groupId, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetStudentRatingByGroupId_AuthorUserIsTeacher_ListOfStudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var groupId = expectedStudentRatingDtos[0].Group.Id;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var group = expectedStudentRatingDtos[0].Group;
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByGroupId(groupId)).Returns(expectedStudentRatingDtos);
            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(authorUserInfo.UserId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            var actualStudentRatingDtos = _sut.GetStudentRatingByGroupId(groupId, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(groupId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(authorUserInfo.UserId, (int)Role.Teacher), Times.Once);
        }

        [Test]
        public void GetStudentRatingByGroupId_GroupDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var groupId = expectedStudentRatingDtos[0].Group.Id;
            var authorUserInfo = RatingData.GetManagerIdentityInfo();
            GroupDto group = default;

            _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(group);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.GetStudentRatingByGroupId(groupId, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(It.IsAny<int>()), Times.Never);
            _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
        }

        [Test]
        public void GetStudentRatingByGroupId_TeacherDoesntAuthorizeToGroup_ListOfStudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var group = expectedStudentRatingDtos[0].Group;
            var groupId = group.Id;
            var authorUserInfo = RatingData.GetTeacherOutOfGroupIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            Assert.Throws<AuthorizationException>(() => _sut.GetStudentRatingByGroupId(groupId, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(It.IsAny<int>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroup(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateStudentRating_AuthorUserIsTeacher_StudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingDtoForUpdate = RatingData.GetStudentRatingDtoForUpdate();
            var studentRatingId = expectedStudentRatingDto.Id;
            var groupId = expectedStudentRatingDto.Group.Id;
            var value = expectedStudentRatingDto.Rating;
            var periodNumber = expectedStudentRatingDto.ReportingPeriodNumber;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            var actualStudentRatingDto = _sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.Is<StudentRatingDto>(dto =>
                dto.Equals(studentRatingDtoForUpdate))));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Exactly(2));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
        }

        [Test]
        public void UpdateStudentRating_AuthorUserIsAdmin_StudentRatingDtoReturned()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingDtoForUpdate = RatingData.GetStudentRatingDtoForUpdate();
            var studentRatingId = expectedStudentRatingDto.Id;
            var groupId = expectedStudentRatingDto.Group.Id;
            var value = expectedStudentRatingDto.Rating;
            var periodNumber = expectedStudentRatingDto.ReportingPeriodNumber;
            var authorUserInfo = RatingData.GetAdminIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);
            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);

            //When
            var actualStudentRatingDto = _sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserInfo);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.Is<StudentRatingDto>(dto =>
                dto.Equals(studentRatingDtoForUpdate))));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Exactly(2));
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UpdateStudentRating_StudentRatingDoesntExist_EntityNotFoundExceptionThrown()
        {
            //Given
            StudentRatingDto expectedStudentRatingDto = default;
            var inputStudentRatingDto = RatingData.GetStudentRatingDto();
            var studentRatingId = inputStudentRatingDto.Id;
            var value = inputStudentRatingDto.Rating;
            var periodNumber = inputStudentRatingDto.ReportingPeriodNumber;
            var authorUserInfo = RatingData.GetTeacherIdentityInfo();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(It.IsAny<int>())).Returns(expectedStudentRatingDto);

            //When
            Assert.Throws<EntityNotFoundException>(() => _sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.IsAny<StudentRatingDto>()), Times.Never);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(It.IsAny<int>(), (int)Role.Teacher), Times.Never);
        }

        [Test]
        public void UpdateStudentRating_TeacherDoesntAuthorizeToGroup_AuthorizationExceptionThrown()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetOutputStudentRatingDto();
            var studentRatingId = expectedStudentRatingDto.Id;
            var groupId = expectedStudentRatingDto.Group.Id;
            var value = expectedStudentRatingDto.Rating;
            var periodNumber = expectedStudentRatingDto.ReportingPeriodNumber;
            var authorUserInfo = RatingData.GetTeacherOutOfGroupIdentityInfo();
            var usersInGroup = UserData.GetListUsersDto();

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher)).Returns(usersInGroup);
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);

            //When
            Assert.Throws<AuthorizationException>(() => _sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserInfo));

            //Than
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.IsAny<StudentRatingDto>()), Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)Role.Teacher), Times.Once);
        }
    }
}