using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<ITopicRepository> _topicRepository;
        private LessonService _sut;

        [SetUp]
        public void Setup()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _userRepository = new Mock<IUserRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _topicRepository = new Mock<ITopicRepository>();

            _sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                new UserValidationHelper(_userRepository.Object),
                new LessonValidationHelper(
                    _lessonRepository.Object,
                    _groupRepository.Object),
                new TopicValidationHelper(_topicRepository.Object),
                new GroupValidationHelper(_groupRepository.Object)
                );
        }

        [Test]
        public void AddTopicToLesson_WhenLessonIdAndTopicIdExist_TopicLessonReferenceCreated()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetTopicDtoWithoutTags();

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopicAsync(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.AddTopicToLessonAsync(lesson.Id, topic.Id));

            //When
            _sut.AddTopicToLesson(lesson.Id, topic.Id);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(lesson.Id, topic.Id), Times.Once);
        }

        [Test]
        public void AddTopicToLesson_WhenLessonWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            var expectedMessage = string.Format(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId));

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                _sut.AddTopicToLesson(lessonId, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lessonId)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topicId)), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topicId = 7;
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                            _sut.AddTopicToLesson(lesson.Id, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(lesson.Id, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicLessonReferenceAlreadyExists_ValidationExceptionThrown()
        {
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();
            var expectedMessage = string.Format(string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lesson.Id, topic.Id));

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopicAsync(topic.Id)).Returns(topic);

            //When
            var actual = Assert.Throws<ValidationException>(() =>
                            _sut.AddTopicToLesson(lesson.Id, topic.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(lesson.Id, topic.Id), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenLessonIdAndTopicIdAreValid_TopicLessonReferenceDeleted()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopicAsync(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.DeleteTopicFromLessonAsync(lesson.Id, topic.Id)).ReturnsAsync(1);

            //When
            _sut.DeleteTopicFromLesson(lesson.Id, topic.Id);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lesson.Id, topic.Id), Times.Once);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenLessonWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            var expectedMessage = string.Format(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId));

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                _sut.DeleteTopicFromLesson(lessonId, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lessonId)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topicId)), Times.Never);
            _lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lessonId, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topicId = 7;
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                            _sut.DeleteTopicFromLesson(lesson.Id, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lesson.Id, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicLessonReferenceDoesNotExist_ValidationExceptionThrown()
        {
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();
            var expectedMessage = string.Format(string.Format(ServiceMessages.LessonTopicReferenceNotFound, lesson.Id, topic.Id));

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopicAsync(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.DeleteTopicFromLessonAsync(lesson.Id, topic.Id)).ReturnsAsync(0);

            //When
            var actual = Assert.Throws<ValidationException>(() =>
                            _sut.DeleteTopicFromLesson(lesson.Id, topic.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLessonAsync(lesson.Id, topic.Id), Times.Once);
        }

        [Test]
        public void AddLesson_UserDtoAndSimpleDtoAndListOfTopicsPassed_LessonAdded()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lessonId = LessonData.LessonId;
            var expectedLesson = LessonData.GetSelectedLessonDto();
            var topicIds = TopicData.GetListTopicId();
            var topics = TopicData.GetListTopicDto();

            _lessonRepository.Setup(x => x.AddLessonAsync(expectedLesson)).Returns(lessonId);
            for (int i = 0; i < topics.Count; i++)
            {
                _topicRepository.Setup(x => x.GetTopicAsync(topicIds[i])).Returns(topics[i]);
                _lessonRepository.Setup(x => x.AddTopicToLessonAsync(lessonId, topicIds[i]));
            }
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(expectedLesson);

            //When
            var actualLesson = _sut.AddLesson(userIdentity, expectedLesson, topicIds);

            //Then
            Assert.AreEqual(expectedLesson, actualLesson);
            _lessonRepository.Verify(x => x.AddLessonAsync(expectedLesson), Times.Once);
            foreach (int topicId in topicIds)
            {
                _topicRepository.Verify(x => x.GetTopicAsync(topicId), Times.Once);
                _lessonRepository.Verify(x => x.AddTopicToLessonAsync(lessonId, topicId), Times.Once);
            }
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
        }

        [Test]
        public void AddLesson_UserAndTeacherAreNotSame_ValidationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var addedLesson = LessonData.GetLessonDto();
            var expectedException = string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.UserId, addedLesson.Teacher.Id);

            //When
            var ex = Assert.Throws<ValidationException>(() => _sut.AddLesson(userIdentity, addedLesson, null));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.AddLessonAsync(addedLesson), Times.Never);
            _topicRepository.Verify(x => x.GetTopicAsync(It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddLesson_TopicDoesntExist_EntityNotFoundExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var addedLesson = LessonData.GetSelectedLessonDto();
            var topicIds = new List<int> { 1 };

            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicIds.First());

            _lessonRepository.Setup(x => x.AddLessonAsync(addedLesson)).Returns(It.IsAny<int>());
            _topicRepository.Setup(x => x.GetTopicAsync(topicIds.First())).Returns(It.IsAny<TopicDto>());
            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.AddLesson(userIdentity, addedLesson, topicIds));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.AddLessonAsync(addedLesson), Times.Once);
            _topicRepository.Verify(x => x.GetTopicAsync(topicIds.First()), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLessonAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByGroupId_UserDtoAndExistingGroupIdPassed_LessonsReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var userDto = UserData.GetTeacherDto();
            var expected = LessonData.GetLessons();
            var group = GroupData.GetGroupDto();

            _groupRepository.Setup(x => x.GetGroupAsync(group.Id)).ReturnsAsync(group);
            _userRepository
                .Setup(x => x.GetUsersByGroupIdAndRoleAsync(group.Id, It.IsAny<int>()))
                .Returns(new List<UserDto> { userDto });
            _lessonRepository.Setup(x => x.SelectAllLessonsByGroupIdAsync(group.Id)).ReturnsAsync(expected);

            //When
            var actual = _sut.SelectAllLessonsByGroupIdAsync(userIdentity, group.Id);

            //Then
            Assert.AreEqual(expected, actual);
            _groupRepository.Verify(x => x.GetGroupAsync(group.Id), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>()), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupIdAsync(group.Id), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByGroupId_GroupDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var groupId = 3;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "group", groupId);

            _groupRepository.Setup(x => x.GetGroupAsync(groupId)).ReturnsAsync(It.IsAny<GroupDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectAllLessonsByGroupIdAsync(userIdentity, groupId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepository.Verify(x => x.GetGroupAsync(groupId), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupIdAsync(groupId), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByGroupId_UserDoesntBelongTOGroup_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var group = GroupData.GetGroupDto();
            var lessons = new List<UserDto> { };
            var expectedException = string.Format(ServiceMessages.UserWithRoleDoesntAuthorizeToGroup, userIdentity.UserId, group.Id, Role.Teacher);

            _groupRepository.Setup(x => x.GetGroupAsync(group.Id)).ReturnsAsync(group);
            _userRepository.Setup(x => x.GetUsersByGroupIdAndRoleAsync(group.Id, It.IsAny<int>())).Returns(lessons);
            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectAllLessonsByGroupIdAsync(userIdentity, group.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepository.Verify(x => x.GetGroupAsync(group.Id), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>()), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupIdAsync(group.Id), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_ExistingTeacherIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();
            var teacher = UserData.GetTeacherDto();

            _lessonRepository.Setup(x => x.SelectAllLessonsByTeacherIdAsync(teacher.Id)).Returns(expected);
            _userRepository.Setup(x => x.GetUserByIdAsync(teacher.Id)).Returns(teacher);

            //When
            var actual = _sut.SelectAllLessonsByTeacherId(teacher.Id);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherIdAsync(teacher.Id), Times.Once);
            _userRepository.Verify(x => x.GetUserByIdAsync(teacher.Id), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_TeacherDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var teacherId = 3;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "user", teacherId);

            _userRepository.Setup(x => x.GetUserByIdAsync(teacherId)).Returns(It.IsAny<UserDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectAllLessonsByTeacherId(teacherId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _userRepository.Verify(x => x.GetUserByIdAsync(teacherId), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherIdAsync(teacherId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_UserDtoAndExistingLessonIdPassed_LessonWithCommentsReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lessonId = LessonData.LessonId;
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();

            var expected = lesson;
            expected.Comments = comments;

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId)).Returns(comments);

            //When
            var actual = _sut.SelectLessonWithCommentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectLessonWithCommentsById(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithStudentRole();
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentity.UserId)).ReturnsAsync(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Once);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lesson.Id), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lesson.Id), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_UserDtoAndExistingLessonIdPassed_LessonWithCommentsAndAttendancesReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();
            var students = LessonData.GetAttendances();

            var expectedLesson = lesson;
            expectedLesson.Comments = comments;
            expectedLesson.Students = students;

            var lessonId = LessonData.LessonId;

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId)).Returns(comments);
            _lessonRepository.Setup(x => x.SelectStudentsLessonByLessonIdAsync(lessonId)).ReturnsAsync(students);

            //When
            var actual = _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expectedLesson, actual);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lessonId), Times.Once);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lessonId), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentity.UserId)).ReturnsAsync(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonIdAsync(lesson.Id), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonIdAsync(lesson.Id), Times.Never);
        }

        [Test]
        public void UpdateLesson_UserDtoAndSimpleDtoWithoutTeacherPassed_UpdatedLessonReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();
            var expected = LessonData.GetSelectedLessonDto();

            _lessonRepository.Setup(x => x.UpdateLessonAsync(updatedLesson));
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(expected);

            //When
            var actual = _sut.UpdateLesson(userIdentity, updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);

            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLessonAsync(updatedLesson), Times.Once);
        }

        [Test]
        public void UpdateLesson_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetLessonDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.UpdateLesson(userIdentity, updatedLesson, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLessonAsync(updatedLesson), Times.Never);
        }

        [Test]
        public void UpdateLesson_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentity.UserId)).ReturnsAsync(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLessonAsync(lesson), Times.Never);
        }

        [Test]
        public void DeleteLesson_UserDtoAndExistingLessonIdPassed_DeletedLesson()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lessonId = LessonData.LessonId;
            var lesson = LessonData.GetSelectedLessonDto();

            _lessonRepository.Setup(x => x.DeleteLessonAsync(lessonId));
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(lesson);

            //When
            _sut.DeleteLesson(userIdentity, lessonId);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.DeleteLessonAsync(lessonId), Times.Once);
        }

        [Test]
        public void DeleteLesson_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithAdminRole();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.DeleteLesson(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.DeleteLessonAsync(lessonId), Times.Never);
        }

        [Test]
        public void DeleteLesson_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserIdentityInfoData.GetUserIdentityWithRole(Role.Teacher, 3);
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentity.UserId)).ReturnsAsync(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.DeleteLesson(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLessonAsync(lesson), Times.Never);
        }
        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.AddStudentToLessonAsync(lessonId, userId));
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.GetUserByIdAsync(userId)).Returns(LessonData.GetUserDto);
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId)).Returns(studentLessonDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());
            //When
            var dto = _sut.AddStudentToLesson(lessonId, userId, userIdentityInfo);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.AddStudentToLessonAsync(lessonId, userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _userRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.DeleteStudentFromLessonAsync(lessonId, userId));
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.GetUserByIdAsync(userId)).Returns(LessonData.GetUserDto);
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId)).Returns(studentLessonDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());

            //When
            _sut.DeleteStudentFromLesson(lessonId, userId, userIdentityInfo);

            //Than
            _lessonRepository.Verify(x => x.DeleteStudentFromLessonAsync(lessonId, userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _userRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }

        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithStudentRole();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentFeedbackForLessonAsync(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.GetUserById(userId)).Returns(LessonData.GetUserDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());

            //When
            var dto = _sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto, userIdentityInfo);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentFeedbackForLessonAsync(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId), Times.Exactly(2));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(userId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithStudentRole();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAbsenceReasonOnLessonAsync(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.GetUserById(userId)).Returns(LessonData.GetUserDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());

            //When
            var dto = _sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto, userIdentityInfo);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAbsenceReasonOnLessonAsync(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId), Times.Exactly(2));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(userId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAttendanceOnLessonAsync(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.GetUserById(userId)).Returns(LessonData.GetUserDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());

            //When
            var dto = _sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto, userIdentityInfo);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAttendanceOnLessonAsync(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId), Times.Exactly(2));
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _userRepository.Verify(x => x.GetUserById(userId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var userIdentityInfo = UserIdentityInfoData.GetUserIdentityWithTeacherRole();
            var lessonId = 30;

            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepository.Setup(x => x.SelectAllFeedbackByLessonIdAsync(lessonId)).Returns(listStudentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonByIdAsync(lessonId)).Returns(LessonData.GetLessonDto);
            _groupRepository.Setup(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id)).ReturnsAsync(LessonData.GetGroupsDto());
            _groupRepository.Setup(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId)).ReturnsAsync(LessonData.GetGroupsDto());

            //When
            var listOfDto = _sut.SelectAllFeedbackByLessonId(lessonId, userIdentityInfo);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepository.Verify(x => x.SelectAllFeedbackByLessonIdAsync(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonByIdAsync(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByLessonIdAsync(studentLessonDto.Lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserIdAsync(userIdentityInfo.UserId), Times.Once);
        }
    }
}