using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IUserValidationHelper> _userValidationHelper;
        private Mock<ILessonValidationHelper> _lessonValidationHelper;

        [SetUp]
        public void SetUp()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _userRepository = new Mock<IUserRepository>();
            _userValidationHelper = new Mock<IUserValidationHelper>();
            _lessonValidationHelper = new Mock<ILessonValidationHelper>();
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
                _userValidationHelper.Object, 
                _lessonValidationHelper.Object);

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
                            _userValidationHelper.Object,
                            _lessonValidationHelper.Object);
            //When
            sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_SimpleDto_LessonAdded()
        {
            //Given
            var expected = LessonData.LessonId;

            var lessonDto = LessonData.GetAddedLessonDto();

            _lessonRepoMock.Setup(x => x.AddLesson(lessonDto)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actualId = sut.AddLesson(lessonDto);

            //Then
            Assert.AreEqual(expected, actualId);
            _lessonRepoMock.Verify(x => x.AddLesson(lessonDto), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByGroupId_ExistingGroupIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();

            var groupId = 9;

            _lessonRepoMock.Setup(x => x.SelectAllLessonsByGroupId(groupId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actual = sut.SelectAllLessonsByGroupId(groupId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectAllLessonsByGroupId(groupId), Times.Once);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_ExistingTeacherIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();

            var teacherId = 3;

            _lessonRepoMock.Setup(x => x.SelectAllLessonsByTeacherId(teacherId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actual = sut.SelectAllLessonsByTeacherId(teacherId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectAllLessonsByTeacherId(teacherId), Times.Once);
        }

        [Test]
        public void SelectLessonById_ExistingLessonIdPassed_LessonReturned()
        {
            //Given
            var expected = LessonData.GetSelectedLessonDto();

            var lessonId = LessonData.LessonId;

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object); 

            //When
            var actual = sut.SelectLessonById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
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

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepoMock.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actual = sut.SelectLessonWithCommentsById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _commentRepoMock.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
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

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepoMock.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);
            _lessonRepoMock.Setup(x => x.SelectStudentsLessonByLessonId(lessonId)).Returns(students);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actual = sut.SelectLessonWithCommentsAndStudentsById(lessonId);

            //Then
            Assert.AreEqual(expectedLesson, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _commentRepoMock.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void UpdateLesson_SimpleDtoWithoutTeacherPassed_UpdatedLessonReturned()
        {
            //Given
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();

            var expected = LessonData.GetUpdatedLessonDto();

            _lessonRepoMock.Setup(x => x.UpdateLesson(updatedLesson));
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var actual = sut.UpdateLesson(updatedLesson);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

    }
}
