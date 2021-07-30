using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserValidationHelper> _userValidationHelper;
        private Mock<ILessonValidationHelper> _lessonValidationHelper;
        private Mock<ITopicValidationHelper> _topicValidationHelper;
        private LessonService _sut;

        [SetUp]
        public void SetUp()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _userRepository = new Mock<IUserRepository>();
            _userValidationHelper = new Mock<IUserValidationHelper>();
            _lessonValidationHelper = new Mock<ILessonValidationHelper>();
            _topicValidationHelper = new Mock<ITopicValidationHelper>();
            _sut = new LessonService(_lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);
        }

        [Test]
        public void AddTopicToLesson_WhenLessonIdAndTopicIdAreValid_TopicLessonReferenceCreated()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepository.Setup(x => x.AddTopicToLesson(lessonId, topicId));

            //When
            _sut.AddTopicToLesson(lessonId, topicId);

            //Then
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonValidationHelper.Verify(x => x.CheckTopicLessonReferenceIsUnique(lessonId, topicId), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddTopicToLesson_WhenLessonWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonValidationHelper.Setup(x => x.CheckLessonExistence(lessonId))
                .Throws(new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId)));
            
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
                _sut.AddTopicToLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Never);
            _lessonValidationHelper.Verify(x => x.CheckTopicLessonReferenceIsUnique(lessonId, topicId), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _topicValidationHelper.Setup(x => x.CheckTopicExistence(topicId))
                .Throws(new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId)));

            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
                _sut.AddTopicToLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonValidationHelper.Verify(x => x.CheckTopicLessonReferenceIsUnique(lessonId, topicId), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicLessonReferenceAlreadyExists_ValidationExceptionThrown()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonValidationHelper.Setup(x => x.CheckTopicLessonReferenceIsUnique(lessonId, topicId))
                .Throws(new ValidationException(string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lessonId, topicId)));

            //When
            var exception = Assert.Throws<ValidationException>(() =>
                _sut.AddTopicToLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lessonId, topicId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonValidationHelper.Verify(x => x.CheckTopicLessonReferenceIsUnique(lessonId, topicId), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenLessonIdAndTopicIdAreValid_TopicLessonReferenceDeleted()
        {
            //Given
            var lessonId = 4;
            var topicId = 7;
            var rowsAffected = 1;
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(lessonId, topicId)).Returns(rowsAffected);

            //When
            _sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenLessonWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 4;
            var topicId = 7;

            _lessonValidationHelper.Setup(x => x.CheckLessonExistence(lessonId))
                .Throws(new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId)));

            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
                _sut.DeleteTopicFromLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "lesson", lessonId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Never);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lessonId = 4;
            var topicId = 7;

            _topicValidationHelper.Setup(x => x.CheckTopicExistence(topicId))
                .Throws(new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId)));

            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
                _sut.DeleteTopicFromLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicLessonReferenceDoesNotExist_ValidationExceptionThrown()
        {
            //Given
            var lessonId = 4;
            var topicId = 7;
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(lessonId, topicId)).
                Throws(new ValidationException(string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId)));

            //When
            var exception = Assert.Throws<ValidationException>(() =>
                _sut.DeleteTopicFromLesson(lessonId, topicId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId)));
            _lessonValidationHelper.Verify(x => x.CheckLessonExistence((lessonId)), Times.Once);
            _topicValidationHelper.Verify(x => x.CheckTopicExistence((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_SimpleDto_LessonAdded()
        {
            //Given
            var expectedId = LessonData.LessonId;
            var lessonDto = LessonData.GetAddedLessonDto();
            var topicIds = new List<int>(){ 6, 7};

            _lessonRepository.Setup(x => x.AddLesson(lessonDto)).Returns(expectedId);
            foreach (int topicId in topicIds)
            {
                _lessonRepository.Setup(x => x.AddTopicToLesson(expectedId, topicId));
            }

            var sut = new LessonService(
                _lessonRepository.Object, 
                _commentRepository.Object, 
                _userRepository.Object,
                _userValidationHelper.Object, 
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

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

            var sut = new LessonService(
                _lessonRepository.Object,
                _commentRepository.Object,
                _userRepository.Object,
                _userValidationHelper.Object,
                _lessonValidationHelper.Object,
                _topicValidationHelper.Object);

            //When
            var actual = sut.UpdateLesson(updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

    }
}
