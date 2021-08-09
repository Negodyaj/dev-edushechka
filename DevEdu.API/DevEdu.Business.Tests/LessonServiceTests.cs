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
                _userRepository.Object,
                new UserValidationHelper(_userRepository.Object),
                new LessonValidationHelper(
                    _lessonRepository.Object,
                    _groupRepository.Object),
                new TopicValidationHelper(_topicRepository.Object));
        }

        [Test]
        public void AddTopicToLesson_WhenLessonIdAndTopicIdExist_TopicLessonReferenceCreated()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetTopicDtoWithoutTags();

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopic(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.AddTopicToLesson(lesson.Id, topic.Id));

            //When
            _sut.AddTopicToLesson(lesson.Id, topic.Id);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lesson.Id, topic.Id), Times.Once);
        }

        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetTopicDtoWithoutTags();

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopic(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.AddTopicToLesson(lesson.Id, topic.Id));

            //When
            _sut.AddTopicToLesson(lesson.Id, topic.Id);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lesson.Id, topic.Id), Times.Once);
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
            _lessonRepository.Verify(x => x.SelectLessonById((lessonId)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topicId)), Times.Never);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topicId = 7;
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                            _sut.AddTopicToLesson(lesson.Id, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lesson.Id, topicId), Times.Never);
        }

        [Test]
        public void AddTopicToLesson_WhenTopicLessonReferenceAlreadyExists_ValidationExceptionThrown()
        {
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();
            var expectedMessage = string.Format(string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lesson.Id, topic.Id));

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopic(topic.Id)).Returns(topic);

            //When
            var actual = Assert.Throws<ValidationException>(() =>
                            _sut.AddTopicToLesson(lesson.Id, topic.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.AddTopicToLesson(lesson.Id, topic.Id), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenLessonIdAndTopicIdAreValid_TopicLessonReferenceDeleted()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopic(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(lesson.Id, topic.Id)).Returns(1);

            //When
            _sut.DeleteTopicFromLesson(lesson.Id, topic.Id);

            //Then
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lesson.Id, topic.Id), Times.Once);
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
            _lessonRepository.Verify(x => x.SelectLessonById((lessonId)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topicId)), Times.Never);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicWithGivenIdDoesNotExist_EntityNotFoundExceptionThrown()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var topicId = 7;
            var expectedMessage = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", topicId);

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);

            //When
            var actual = Assert.Throws<EntityNotFoundException>(() =>
                            _sut.DeleteTopicFromLesson(lesson.Id, topicId));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topicId)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lesson.Id, topicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromLesson_WhenTopicLessonReferenceDoesNotExist_ValidationExceptionThrown()
        {
            var lesson = LessonData.GetSelectedLessonDto();
            var topic = TopicData.GetAnotherTopicDtoWithoutTags();
            var expectedMessage = string.Format(string.Format(ServiceMessages.LessonTopicReferenceNotFound, lesson.Id, topic.Id));

            _lessonRepository.Setup(x => x.SelectLessonById(lesson.Id)).Returns(lesson);
            _topicRepository.Setup(x => x.GetTopic(topic.Id)).Returns(topic);
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(lesson.Id, topic.Id)).Returns(0);

            //When
            var actual = Assert.Throws<ValidationException>(() =>
                            _sut.DeleteTopicFromLesson(lesson.Id, topic.Id));

            //Then
            Assert.AreEqual(expectedMessage, actual.Message);
            _lessonRepository.Verify(x => x.SelectLessonById((lesson.Id)), Times.Once);
            _topicRepository.Verify(x => x.GetTopic((topic.Id)), Times.Once);
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lesson.Id, topic.Id), Times.Once);
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

            //When
            var actualId = _sut.AddLesson(lessonDto, topicIds);

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

            //When
            var actual = _sut.SelectAllLessonsByGroupId(groupId);

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

            //When
            var actual = _sut.SelectAllLessonsByTeacherId(teacherId);

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

            //When
            var actual = _sut.SelectLessonById(lessonId);

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

            //When
            var actual = _sut.SelectLessonWithCommentsById(lessonId);

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

            //When
            var actual = _sut.SelectLessonWithCommentsAndStudentsById(lessonId);

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

            //When
            var actual = _sut.UpdateLesson(updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }
    }
}