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
            var expectedCourseTopicId = 42;
            var courseTopicDto = new CourseTopicDto { Position = 3 };

            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(courseTopicDto)).Returns(expectedCourseTopicId);

            var sut = new CourseService(_topicRepositoryMock.Object, _courseRepositoryMock.Object);
            //When
            var courseTopicId = sut.AddTopicToCourse(givenCourseId, givenTopicId, courseTopicDto);

            //Then
            Assert.AreEqual(expectedCourseTopicId, courseTopicId);
        }
    }
}
