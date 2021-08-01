using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class CourseServiceTests
    {
        private  Mock<ICourseRepository> _courseRepositoryMock;
        private  Mock<ITopicRepository> _topicRepositoryMock;
        private  Mock<ITaskRepository> _taskRepositoryMock;
        private  Mock<IMaterialRepository> _materialRepositoryMock;
        private CourseService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _sut = new CourseService(_courseRepositoryMock.Object,
                                        _topicRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object);
        }

        [Test]
        public void AddCourse_CourseInput_CourseCreated() 
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;

            _courseRepositoryMock.Setup(x => x.AddCourse(courseDto)).Returns(courseId);

            //When
            var actualCourse = _sut.AddCourse(courseDto);

            //Than
            Assert.AreEqual(courseId, actualCourse);
            _courseRepositoryMock.Verify(x => x.AddCourse(courseDto), Times.Once());
        }

        [Test]
        public void DeleteCourse_IntCourseId_DeleteCourse()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;

            _courseRepositoryMock.Setup(x => x.DeleteCourse(courseId));

            //When
            _sut.DeleteCourse(courseId);

            //Than
            _courseRepositoryMock.Verify(x => x.DeleteCourse(courseId), Times.Once());
        }

        [Test]
        public void GetCourseById_IntCourseId_ReturnCourseWithGroups()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(courseDto);

            //When
            var dto = _sut.GetCourse(courseId);

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTaskByCourseId(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(courseId), Times.Never);
        }
        [Test]
        public void GetCourseById_IntCourseId_ReturnCourseWithGroups_Topics_Materials_Tasks()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(courseDto);

            //When
            var dto = _sut.GetFullCourseInfo(courseId);

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTaskByCourseId(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Once);
            _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(courseId), Times.Once);
        }

        [Test]
        public void GetCourses_WithoutParams_ReturnListCourses()
        {
            //Given
            var courseList = CourseData.GetListCourses();

            _courseRepositoryMock.Setup(x => x.GetCourses()).Returns(courseList);

            //When
            var actualCourseList = _sut.GetCourses();

            //Then
            Assert.AreEqual(courseList, actualCourseList);
            _courseRepositoryMock.Verify(x => x.GetCourses(), Times.Once);
            foreach(var course in actualCourseList)
            {
                _taskRepositoryMock.Verify(x => x.GetTaskByCourseId(course.Id), Times.Never);
                _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(course.Id), Times.Never);
                _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(course.Id), Times.Never);
            }
        }

        [Test]
        public void UpdateCourseInt_CourseId_Name_Descriptions_ReturnApdatedCourse()
        {
            //When
            var courseId = 1;
            var courseDto = CourseData.GetCourseDto();
            var updCourseDto = CourseData.GetUpdCourseDto();

            _courseRepositoryMock.Setup(x => x.UpdateCourse(courseId, courseDto)).Returns(updCourseDto);

            //When
            var actualCourseDto = _sut.UpdateCourse(courseId, courseDto);

            //Then
            Assert.AreEqual(updCourseDto, actualCourseDto);
            _courseRepositoryMock.Verify(x => x.UpdateCourse(courseId, courseDto), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTaskByCourseId(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(courseId), Times.Never);
        }

        [Test]
        public void AddTopicToCourse_WithCourseIdAndSimpleDto_TopicWasAdded() 
        {
            //Given
            var givenCourseId = 12;
            var givenTopicId = 22;
            var courseTopicDto = new CourseTopicDto { Position = 3 };

            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(courseTopicDto));

            //When
            _sut.AddTopicToCourse(givenCourseId, givenTopicId, courseTopicDto);

            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(courseTopicDto), Times.Once);
            
        }
        [Test]
        public void AddTopicsToCourse_WithCourseIdAndListSimpleDto_TopicsWereAdded()
        {
            //Given
            var givenCourseId = 2;
            var topicsDto = CourseData.GetListCourseTopicDto();

            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(topicsDto));
            //When
            _sut.AddTopicsToCourse(givenCourseId, topicsDto);

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

            //When
            _sut.DeleteTopicFromCourse(givenCourseId, givenTopicId);

            //Then
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Once);
        }
        [Test]
        public void SelectAllTopicsByCourseId_ByCourseId_GotListOfCourseTopics()
        {
            //Given
            var givenCourseId = 4;

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId));

            //When
            _sut.SelectAllTopicsByCourseId(givenCourseId);

            //Then
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseId(givenCourseId), Times.Once);

        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenCountOfTopicsNotChanged_ThenUpdateMethodCalled()
        {
            //Given
            var givenCourseId = 7;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase(); 
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
           
            //When
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Once);

        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenCountOfTopicsIsChanged_ThenDeleteAndInsertMethodsCalled()
        {
            //Given
            var givenCourseId = 7;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 8, Topic = new TopicDto { Id = 8 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 6, Id = 9, Topic = new TopicDto { Id = 9 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 8, Id = 2, Topic = new TopicDto { Id = 2 } });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
           
            //When
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Once);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(givenTopicsToUpdate), Times.Once);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_TopicsInDatabaseAreAbsentForCourse_AddedTopicsForCourse()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var toicsFromDB = new List<CourseTopicDto>();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(givenTopicsToUpdate), Times.Once);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsToUpdateNotProvided_ThenUpdateTerminates()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenPositionsAreNotUnique_ValidationExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 21, Topic = new TopicDto { Id = 21 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 13, Topic = new TopicDto { Id = 13 } });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            
            //When
            var exception = Assert.Throws<ValidationException>(() => 
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate));

            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SamePositionsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
            
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsAreNotUnique_ValidationExceptionThrown()
        {
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 3, Id = 21, Topic = new TopicDto { Id = 21 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 15, Topic = new TopicDto { Id = 15 } });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            var exception = Assert.Throws<ValidationException>(() => 
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate));

            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SameTopicsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }

    }
}
