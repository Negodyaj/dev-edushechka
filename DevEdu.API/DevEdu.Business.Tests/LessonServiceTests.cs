using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using Moq;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IGroupRepository> _groupRepository;
        private UserValidationHelper _userValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private LessonService _sut;

        [SetUp]
        public void Setup()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _userRepository = new Mock<IUserRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepository.Object, _groupRepository.Object);
            _userValidationHelper = new UserValidationHelper(_userRepository.Object);

            _sut = new LessonService(
                    _lessonRepository.Object,
                    _commentRepository.Object,
                    _userRepository.Object,
                    _userValidationHelper,
                    _lessonValidationHelper);
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

            var sut = new LessonService(_lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper,
                _lessonValidationHelper);

            //When
            sut.AddTopicToLesson(lessonId, topicId);

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

            var sut = new LessonService(_lessonRepository.Object,
                            _commentRepository.Object,
                            _userRepository.Object,
                            _userValidationHelper,
                            _lessonValidationHelper);
            //When
            sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_SimpleDto_LessonAdded()
        {
            //Given
            var expectedId = LessonData.LessonId;
            var lessonDto = LessonData.GetAddedLessonDto();
            var topicIds = new List<int>() { 6, 7 };

            _lessonRepository.Setup(x => x.AddLesson(lessonDto)).Returns(expectedId);
            foreach (int topicId in topicIds)
            {
                _lessonRepository.Setup(x => x.AddTopicToLesson(expectedId, topicId));
            }

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actualId = sut.AddLesson(lessonDto, topicIds);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _lessonRepository.Verify(x => x.AddLesson(lessonDto), Times.Once);
            foreach (int topicId in topicIds)
            {
                _lessonRepository.Verify(x => x.AddTopicToLesson(expectedId, topicId), Times.Once);
            }
        }

        [Test]
        public void SelectAllLessonsByGroupId_ExistingGroupIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();

            var groupId = 9;

            _lessonRepository.Setup(x => x.SelectAllLessonsByGroupId(groupId)).Returns(expected);

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.SelectAllLessonsByGroupId(groupId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupId(groupId), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_ExistingTeacherIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();

            var teacherId = 3;

            _lessonRepository.Setup(x => x.SelectAllLessonsByTeacherId(teacherId)).Returns(expected);

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.SelectAllLessonsByTeacherId(teacherId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherId(teacherId), Times.Once);
        }

        [Test]
        public void SelectLessonById_ExistingLessonIdPassed_LessonReturned()
        {
            //Given
            var expected = LessonData.GetSelectedLessonDto();

            var lessonId = LessonData.LessonId;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.SelectLessonById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

        [Test]
        public void SelectLessonWithCommentsById_ExistingLessonIdPassed_LessonWithCommentsReturned()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();

            var expected = lesson;
            expected.Comments = comments;

            var lessonId = LessonData.LessonId;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.SelectLessonWithCommentsById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_ExistingLessonIdPassed_LessonWithCommentsAndAttendancesReturned()
        {
            //Given
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

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.SelectLessonWithCommentsAndStudentsById(lessonId);

            //Then
            Assert.AreEqual(expectedLesson, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void UpdateLesson_SimpleDtoWithoutTeacherPassed_UpdatedLessonReturned()
        {
            //Given
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();

            var expected = LessonData.GetUpdatedLessonDto();

            _lessonRepository.Setup(x => x.UpdateLesson(updatedLesson));
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object, _userRepository.Object,
                _userValidationHelper, _lessonValidationHelper);

            //When
            var actual = sut.UpdateLesson(updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }
    }
}