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
    }
}
