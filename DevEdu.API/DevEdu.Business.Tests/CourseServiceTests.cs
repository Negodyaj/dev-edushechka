using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class CourseServiceTests
    {
        private  Mock<ICourseRepository> _courseRepositoryMock;
        private  Mock<ITopicRepository> _topicRepositoryMock;
        
        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _topicRepositoryMock = new Mock<ITopicRepository>();
        }
        [Test]
        public void AddTopicToCourse_WithCourseIdAndSimpleDto_InCourseAddedOneTopic() 
        {
            //Given
            var givenCourseId = 12;
            var givenTopicId = 22;
            var courseTopicDto = CourseData.GetCourseTopicDto(1);
            
            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(courseTopicDto));
            var sut = new CourseService(_topicRepositoryMock.Object, _courseRepositoryMock.Object);
            //When
            var courseTopicId = sut.AddTopicToCourse(givenCourseId, givenTopicId, courseTopicDto);
            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(courseTopicDto), Times.Once);
            
        }
        [Test]
        public void AddTopicsToCourse_WithCourseIdAndListSimpleDto_InCourseAddedSomeTopics()
        {
            //Given
            var givenCourseId = 2;
            var topicsDto = CourseData.GetListCourseTopicDto(1);

            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(topicsDto));
            var sut = new CourseService(_topicRepositoryMock.Object, _courseRepositoryMock.Object);
            //When
            sut.AddTopicsToCourse(givenCourseId, topicsDto);
            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(topicsDto), Times.Once);
        }
        [Test]
        public void DeleteTopicFromCourse_ByCourseIdAndTopicId_TopicDeletedFromCourse()
        {
            //Given
            var givenCourseId = 4;
            var givenTopicId = 7;

            _topicRepositoryMock.Setup(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            var sut = new CourseService(_topicRepositoryMock.Object, _courseRepositoryMock.Object);
            //When
            sut.DeleteTopicFromCourse(givenCourseId, givenTopicId);
            //Then
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Once);
        }
        [Test]
        public void SelectAllTopicsByCourseId_ByCourseId_GotListOfCourseTopics()
        {
            //Given
            var givenCourseId = 4;

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId));
            var sut = new CourseService(_topicRepositoryMock.Object, _courseRepositoryMock.Object);
            //When
            sut.SelectAllTopicsByCourseId(givenCourseId);
            //Then
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseId(givenCourseId), Times.Once);

        }

    }
}
