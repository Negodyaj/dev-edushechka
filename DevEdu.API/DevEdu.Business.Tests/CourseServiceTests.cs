using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;

namespace DevEdu.Business.Tests
{
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ITopicRepository> _topicRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<IMaterialRepository> _materialRepositoryMock;
        private CourseValidationHelper _courseValidationHelper;
        private MaterialValidationHelper _materialValidationHelper;
        private CourseService _sut;

        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _courseValidationHelper = new CourseValidationHelper(_courseRepositoryMock.Object);
            _materialValidationHelper = new MaterialValidationHelper(_materialRepositoryMock.Object);
            _sut = new CourseService(_courseRepositoryMock.Object, _topicRepositoryMock.Object,
                _taskRepositoryMock.Object, _materialRepositoryMock.Object, _courseValidationHelper,
                _materialValidationHelper);
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

        [Test]
        public void AddMaterialToCourse_ExistingCourseIdAndMaterialIdIdPassed_AddMaterialToCourse()
        {
            //Given
            const int courseId = 1;
            const int materialId = 1;

            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(CourseData.GetCourseDto());
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);
            _courseRepositoryMock.Setup(x => x.AddCourseMaterialReference(courseId, materialId));

            //When
            _sut.AddCourseMaterialReference(courseId, materialId);

            //Than
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialById(materialId), Times.Once);
            _courseRepositoryMock.Verify(x => x.AddCourseMaterialReference(courseId, materialId), Times.Once);
        }

        [Test]
        public void DeleteMaterialFromCourse_ExistingCourseIdAndMaterialIdIdPassed_DeleteMaterialFromCourse()
        {
            //Given
            const int courseId = 1;
            const int materialId = 1;

            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(CourseData.GetCourseDto());
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(MaterialData.GetMaterialDtoWithoutTags);
            _courseRepositoryMock.Setup(x => x.RemoveCourseMaterialReference(courseId, materialId));

            //When
            _sut.RemoveCourseMaterialReference(courseId, materialId);

            //Than
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialById(materialId), Times.Once);
            _courseRepositoryMock.Verify(x => x.RemoveCourseMaterialReference(courseId, materialId), Times.Once);
        }
    }
}