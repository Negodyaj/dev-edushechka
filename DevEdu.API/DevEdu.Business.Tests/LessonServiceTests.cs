using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;

        [SetUp]
        public void SetUp()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
        }

        [Test]
        public void AddTopicToLesson_LessonIdTopicId_TopicLessonReferenceCreated()
        {
            //Given
            _lessonRepository.Setup(x => x.AddTopicToLesson(LessonData.expectedId, LessonData.topicId));

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object);

            //When
            sut.AddTopicToLesson(LessonData.expectedId, LessonData.topicId);

            //Then
            _lessonRepository.Verify(x => x.AddTopicToLesson(LessonData.expectedId, LessonData.topicId), Times.Once);
        }

        [Test]
        public void DeleteTopicFromLesson_LessonIdTopicId_TopicLessonReferenceDeleted()
        {
            //Given
            _lessonRepository.Setup(x => x.DeleteTopicFromLesson(LessonData.expectedId, LessonData.topicId));

            var sut = new LessonService(_lessonRepository.Object, _commentRepository.Object);

            //When
            sut.DeleteTopicFromLesson(LessonData.expectedId, LessonData.topicId);

            //Then
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(LessonData.expectedId, LessonData.topicId), Times.Once);
        }
    }
}
