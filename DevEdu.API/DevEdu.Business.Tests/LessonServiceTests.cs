using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using Moq;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Constants;
using System.Collections.Generic;
using System.Linq;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<ITopicRepository> _topicRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;

        private ILessonService _sut;

        [SetUp]
        public void Setup()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _topicRepository = new Mock<ITopicRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _userRepository = new Mock<IUserRepository>();

            UserValidationHelper userValidationHelper = new UserValidationHelper(_userRepository.Object);
            ILessonValidationHelper lessonValidationHelper = new LessonValidationHelper(
                _lessonRepository.Object,
                _groupRepository.Object,
                _userRepository.Object
            );
            ITopicValidationHelper topicValidationHelper = new TopicValidationHelper(_topicRepository.Object);
            IGroupValidationHelper groupValidationHelper = new GroupValidationHelper(_groupRepository.Object);

            _sut = new LessonService(_lessonRepository.Object,
                    _commentRepository.Object,
                    userValidationHelper,
                    lessonValidationHelper,
                    topicValidationHelper,
                    groupValidationHelper);
        }

        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();

            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.AddStudentToLesson(lessonId, userId));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.AddStudentToLesson(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.AddStudentToLesson(lessonId, userId), Times.Once);
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.DeleteStudentFromLesson(lessonId, userId));

            //When
            _sut.DeleteStudentFromLesson(lessonId, userId);

            //Than
            _lessonRepository.Verify(x => x.DeleteStudentFromLesson(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentFeedbackForLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.SelectUserById(userId)).Returns(LessonData.GetUserDto);

            //When
            var dto = _sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentFeedbackForLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _userRepository.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var lessonId = 30;
            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepository.Setup(x => x.SelectAllFeedbackByLessonId(lessonId)).Returns(listStudentLessonDto);

            //When
            var listOfDto = _sut.SelectAllFeedbackByLessonId(lessonId);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepository.Verify(x => x.SelectAllFeedbackByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void AddTopicToLesson_LessonIdTopicId_TopicLessonReferenceCreated()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepository.Setup(x => x.AddTopicToLesson(lessonId, topicId));

            //When
            _sut.AddTopicToLesson(lessonId, topicId);

            //Then
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void DeleteTopicFromLesson_LessonIdTopicId_TopicLessonReferenceDeleted()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(lessonId, topicId));

            //When
            _sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_UserDtoAndSimpleDtoAndListOfTopicsPassed_LessonAdded()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lessonId = LessonData.LessonId;
            var expectedLesson = LessonData.GetSelectedLessonDto();
            var topicIds = TopicData.GetListTopicId();
            var topics = TopicData.GetListTopicDto();

            _lessonRepository.Setup(x => x.AddLesson(expectedLesson)).Returns(lessonId);
            for(int i = 0; i < topics.Count; i++)
            {
                _topicRepository.Setup(x => x.GetTopic(topicIds[i])).Returns(topics[i]);
                _lessonRepository.Setup(x => x.AddTopicToLesson(lessonId, topicIds[i]));
            }
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expectedLesson);

            //When
            var actualLesson = _sut.AddLesson(userIdentity, expectedLesson, topicIds);

            //Then
            Assert.AreEqual(expectedLesson, actualLesson);
            _lessonRepository.Verify(x => x.AddLesson(expectedLesson), Times.Once);
            foreach (int topicId in topicIds)
            {
                _topicRepository.Verify(x => x.GetTopic(topicId), Times.Once);
                _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Once);
            }
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

        [Test]
        public void AddLesson_UserAndTeacherAreNotSame_ValidationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var addedLesson = LessonData.GetLessonDto();
            var expectedException = string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.UserId, addedLesson.Teacher.Id);

            //When
            var ex = Assert.Throws<ValidationException>(() => _sut.AddLesson(userIdentity, addedLesson, null));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.AddLesson(addedLesson), Times.Never);
            _topicRepository.Verify(x => x.GetTopic(It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLesson(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectLessonById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddLesson_TopicDoesntExist_EntityNotFoundExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var addedLesson = LessonData.GetSelectedLessonDto();
            var topicIds = new List<int>{1};

            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicIds.First());

            _lessonRepository.Setup(x => x.AddLesson(addedLesson)).Returns(It.IsAny<int>());
            _topicRepository.Setup(x => x.GetTopic(topicIds.First())).Returns(It.IsAny<TopicDto>());
            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.AddLesson(userIdentity, addedLesson, topicIds));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.AddLesson(addedLesson), Times.Once);
            _topicRepository.Verify(x => x.GetTopic(topicIds.First()), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectLessonById(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByGroupId_UserDtoAndExistingGroupIdPassed_LessonsReturned()
        { 
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var userDto = UserData.GetTeacherDto();
            var expected = LessonData.GetLessons();
            var group = GroupData.GetGroupDto();

            _groupRepository.Setup(x => x.GetGroup(group.Id)).Returns(group);
            _userRepository
                .Setup(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>()))
                .Returns(new List<UserDto> { userDto });
            _lessonRepository.Setup(x => x.SelectAllLessonsByGroupId(group.Id)).Returns(expected);

            //When
            var actual = _sut.SelectAllLessonsByGroupId(userIdentity, group.Id);

            //Then
            Assert.AreEqual(expected, actual);
            _groupRepository.Verify(x => x.GetGroup(group.Id), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>()), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupId(group.Id), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByGroupId_GroupDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var groupId = 3;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "group", groupId);

            _groupRepository.Setup(x => x.GetGroup(groupId)).Returns(It.IsAny<GroupDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectAllLessonsByGroupId(userIdentity, groupId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepository.Verify(x => x.GetGroup(groupId), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(groupId, It.IsAny<int>()), Times.Never);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupId(groupId), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByGroupId_UserDoesntBelongTOGroup_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var group = GroupData.GetGroupDto();
            var lessons = new List<UserDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToGroup, userIdentity.UserId, group.Id);

            _groupRepository.Setup(x => x.GetGroup(group.Id)).Returns(group);
            _userRepository.Setup(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>())).Returns(lessons);
            //When
            var ex = Assert.Throws<ValidationException>(() => _sut.SelectAllLessonsByGroupId(userIdentity, group.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _groupRepository.Verify(x => x.GetGroup(group.Id), Times.Once);
            _userRepository.Verify(x => x.GetUsersByGroupIdAndRole(group.Id, It.IsAny<int>()), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupId(group.Id), Times.Never);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_ExistingTeacherIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();
            var teacher = UserData.GetTeacherDto();

            _lessonRepository.Setup(x => x.SelectAllLessonsByTeacherId(teacher.Id)).Returns(expected);
            _userRepository.Setup(x => x.SelectUserById(teacher.Id)).Returns(teacher);

            //When
            var actual = _sut.SelectAllLessonsByTeacherId(teacher.Id);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherId(teacher.Id), Times.Once);
            _userRepository.Verify(x => x.SelectUserById(teacher.Id), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_TeacherDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var teacherId = 3;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "user", teacherId);

            _userRepository.Setup(x => x.SelectUserById(teacherId)).Returns(It.IsAny<UserDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectAllLessonsByTeacherId(teacherId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _userRepository.Verify(x => x.SelectUserById(teacherId), Times.Once);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherId(teacherId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_UserDtoAndExistingLessonIdPassed_LessonWithCommentsReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();

            var expected = lesson;
            expected.Comments = comments;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);

            //When
            var actual = _sut.SelectLessonWithCommentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectLessonWithCommentsById(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsById_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetStudentIdentity();
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userIdentity.UserId)).Returns(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Once);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lesson.Id), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lesson.Id), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_UserDtoAndExistingLessonIdPassed_LessonWithCommentsAndAttendancesReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();
            var students = LessonData.GetAttendances();

            var expectedLesson = lesson;
            expectedLesson.Comments = comments;
            expectedLesson.Students = students;

            var lessonId = LessonData.LessonId;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);
            _lessonRepository.Setup(x => x.SelectStudentsLessonByLessonId(lessonId)).Returns(students);

            //When
            var actual = _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expectedLesson, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userIdentity.UserId)).Returns(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lesson.Id), Times.Never);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lesson.Id), Times.Never);
        }

        [Test]
        public void UpdateLesson_UserDtoAndSimpleDtoWithoutTeacherPassed_UpdatedLessonReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();
            var expected = LessonData.GetSelectedLessonDto();

            _lessonRepository.Setup(x => x.UpdateLesson(updatedLesson));
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            //When
            var actual = _sut.UpdateLesson(userIdentity, updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);

            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Exactly(2));
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
        }

        [Test]
        public void UpdateLesson_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetLessonDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.UpdateLesson(userIdentity, updatedLesson, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Never);
        }

        [Test]
        public void UpdateLesson_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userIdentity.UserId)).Returns(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLesson(lesson), Times.Never);
        }

        [Test]
        public void DeleteLesson_UserDtoAndExistingLessonIdPassed_DeletedLesson()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lessonId = LessonData.LessonId;
            var lesson = LessonData.GetSelectedLessonDto();

            _lessonRepository.Setup(x => x.DeleteLesson(lessonId));
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);

            //When
            _sut.DeleteLesson(userIdentity, lessonId);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.DeleteLesson(lessonId), Times.Once);
        }

        [Test]
        public void DeleteLesson_LessonDoesntExist_EntityNotFoundExciptionReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId);

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(It.IsAny<LessonDto>());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(() => _sut.DeleteLesson(userIdentity, lessonId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.DeleteLesson(lessonId), Times.Never);
        }

        [Test]
        public void DeleteLesson_UserDoesntBelongToLesson_AuthorizationExceptionReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lesson = LessonData.GetLessonDto();
            var groups = new List<GroupDto> { };
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _groupRepository.Setup(x => x.GetGroupsByUserId(userIdentity.UserId)).Returns(groups);

            //When
            var ex = Assert.Throws<AuthorizationException>(() => _sut.DeleteLesson(userIdentity, lesson.Id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepository.Verify(x => x.SelectLessonById(lesson.Id), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByUserId(userIdentity.UserId), Times.Never);
            _lessonRepository.Verify(x => x.UpdateLesson(lesson), Times.Never);
        }
    }
}
