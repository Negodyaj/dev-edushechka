using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
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
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<ITopicRepository> _topicRepositoryMock;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IMaterialRepository> _materialRepositoryMock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private CourseValidationHelper _courseValidationHelper;
        private TopicValidationHelper _topicValidationHelper;
        private TaskValidationHelper _taskValidationHelper;
        private MaterialValidationHelper _materialValidationHelper;
        private GroupValidationHelper _groupValidationHelper;
        private CourseService _sut;


        [SetUp]
        public void Setup()
        {
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _courseValidationHelper = new CourseValidationHelper(_courseRepositoryMock.Object);
            _topicValidationHelper = new TopicValidationHelper(_topicRepositoryMock.Object);
            _taskValidationHelper = new TaskValidationHelper(_taskRepositoryMock.Object, _groupRepositoryMock.Object);
            _groupValidationHelper = new GroupValidationHelper(_groupRepositoryMock.Object);
            _materialValidationHelper = new MaterialValidationHelper(
                _materialRepositoryMock.Object,
                _groupRepositoryMock.Object,
                _courseRepositoryMock.Object);
            _topicValidationHelper = new TopicValidationHelper(_topicRepositoryMock.Object);
            _sut = new CourseService
            (
                _courseRepositoryMock.Object,
                _topicRepositoryMock.Object,
                _taskRepositoryMock.Object,
                _materialRepositoryMock.Object,
                _groupRepositoryMock.Object,
                _courseValidationHelper,
                _topicValidationHelper,
                _taskValidationHelper,
                _materialValidationHelper,
                _groupValidationHelper
            );
        }

        [Test]
        public void AddCourse_CourseInput_CourseCreated()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;

            _courseRepositoryMock.Setup(x => x.AddCourse(courseDto)).Returns(courseId);
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(courseDto);

            //When
            var actualCourse = _sut.AddCourse(courseDto);

            //Than
            Assert.AreEqual(courseId, actualCourse.Id);
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
        public void GetCourseById_IntCourseId_ReturnCourseWithTopics()
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
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseId(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Never);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseId(courseId), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetCourseById_IntCourseIdByTeacherOrTutor_ReturnCourseWithTopics_Groups_Materials_Tasks(Enum role)
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            var userToken = UserIdentityInfoData.GetUserIdentityWithRole(role);
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(courseDto);

            //When
            var dto = _sut.GetFullCourseInfo(courseId, userToken);

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseId(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Once);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseId(courseId), Times.Once);
        }

        [TestCase(Role.Methodist)]
        public void GetCourseById_IntCourseIdByMethodist_ReturnCourseWithTopics_Groups_Materials(Enum role)
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            var userToken = UserIdentityInfoData.GetUserIdentityWithRole(role);
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(courseDto);

            //When
            var dto = _sut.GetFullCourseInfo(courseId, userToken);

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseId(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Once);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseId(courseId), Times.Once);
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
            foreach (var course in actualCourseList)
            {
                _taskRepositoryMock.Verify(x => x.GetTasksByCourseId(course.Id), Times.Never);
                _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(course.Id), Times.Never);
                _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(course.Id), Times.Never);
            }
        }

        [Test]
        public void UpdateCourseInt_CourseId_Name_Descriptions_ReturnUpdatedCourse()
        {
            //When
            var courseId = 1;
            var courseDto = CourseData.GetCourseDto();
            var updCourseDto = CourseData.GetUpdCourseDto();

            _courseRepositoryMock.Setup(x => x.UpdateCourse(courseDto));
            _courseRepositoryMock.Setup(x => x.GetCourse(courseDto.Id)).Returns(updCourseDto);

            //When
            var actualCourseDto = _sut.UpdateCourse(courseId, courseDto);

            //Then
            Assert.AreEqual(updCourseDto, actualCourseDto);
            _courseRepositoryMock.Verify(x => x.UpdateCourse(courseDto), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseId(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseId(courseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.GetTopicsByCourseId(courseId), Times.Never);
        }

        [Test]
        public void AddTopicToCourse_WithCourseIdAndSimpleDto_TopicWasAdded()
        {
            //Given
            var givenCourseId = 12;
            var givenTopicId = 8;
            var courseTopicDto = new CourseTopicDto { Position = 3 };

            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(courseTopicDto));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopic(givenTopicId)).Returns(new TopicDto() { Id = givenTopicId });

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
            var courseTopicsDto = CourseData.GetListCourseTopicDto();
            var topicsDto = CourseData.GetTopics();

            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courseTopicsDto));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);

            //When
            _sut.AddTopicsToCourse(givenCourseId, courseTopicsDto);
            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courseTopicsDto), Times.Once);
        }

        [Test]
        public void DeleteTopicFromCourse_ByCourseIdAndTopicId_TopicDeletedFromCourse()
        {
            //Given
            var givenCourseId = 4;
            var givenTopicId = 7;

            _topicRepositoryMock.Setup(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopic(givenTopicId)).Returns(new TopicDto() { Id = givenTopicId });

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
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });

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
            var courseTopicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            var topicsInDB = CourseData.GetTopics();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseTopicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });

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
            var givenCourseTopicsToUpdate = new List<CourseTopicDto>();
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 8, Topic = new TopicDto { Id = 8 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 6, Id = 9, Topic = new TopicDto { Id = 9 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 8, Id = 2, Topic = new TopicDto { Id = 2 } });

            var topicsInDB = CourseData.GetTopics();
            var courseToicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseToicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenCourseTopicsToUpdate));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });

            //When
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Once);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(givenCourseTopicsToUpdate), Times.Once);
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_TopicsInDatabaseAreAbsentForCourse_AddedTopicsForCourse()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var courseToicsFromDB = new List<CourseTopicDto>();
            List<TopicDto> topicsInDB = CourseData.GetTopics();
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseToicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });

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
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate));
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
            Assert.That(result.Message, Is.EqualTo(ServiceMessages.EntityNotFound));
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_WhenPositionsAreNotUnique_ValidationExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            var topicsDto = CourseData.GetTopics();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 1, Topic = new TopicDto { Id = 1 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 2, Topic = new TopicDto { Id = 2 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 3, Topic = new TopicDto { Id = 3 } });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
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
            List<TopicDto> topicsDto = new List<TopicDto>();

            topicsDto.Add(new TopicDto { Id = 15 });
            topicsDto.Add(new TopicDto { Id = 21 });
            topicsDto.Add(new TopicDto { Id = 15 });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);
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
        public void AddCourseMaterialReference_ValidCourseIdAndMaterialId_MaterialWasAddedToCourse()
        {
            var courseId = 2;
            var materialId = 4;
            var materialDto = MaterialData.GetMaterialDtoWithoutTags();

            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(new CourseDto() { Id = courseId });
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            //When
            _sut.AddCourseMaterialReference(courseId, materialId);
            //Then
            _courseRepositoryMock.Verify(x => x.AddCourseMaterialReference(courseId, materialId), Times.Once);
        }
        [Test]
        public void AddCourseMaterialReference_NotValidCourseId_EntityNotFoundExceptionThrown()
        {
            var courseId = 2;
            var materialId = 4;
            var materialDto = MaterialData.GetMaterialDtoWithoutTags();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", courseId);
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId));
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.AddCourseMaterialReference(courseId, materialId));
            //Then
            _courseRepositoryMock.Verify(x => x.AddCourseMaterialReference(courseId, materialId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void AddCourseMaterialReference_NotValidMaterialId_EntityNotFoundExceptionThrown()
        {
            var courseId = 2;
            var materialId = 4;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialId);
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(new CourseDto() { Id = courseId }); ;
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.AddCourseMaterialReference(courseId, materialId));
            //Then
            _courseRepositoryMock.Verify(x => x.AddCourseMaterialReference(courseId, materialId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void RemoveCourseMaterialReference_ValidCourseIdAndMaterialId_MaterialWasDeletedFromCourse()
        {
            //Given
            var courseId = 2;
            var materialId = 4;
            var materialDto = MaterialData.GetMaterialDtoWithoutTags();
            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(new CourseDto() { Id = courseId });
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            //When
            _sut.RemoveCourseMaterialReference(courseId, materialId);
            //Then
            _courseRepositoryMock.Verify(x => x.RemoveCourseMaterialReference(courseId, materialId), Times.Once);
        }
        [Test]
        public void RemoveCourseMaterialReference_NotValidCourseId_EntityNotFoundExceptionThrown()
        {
            //Given
            var courseId = 2;
            var materialId = 4;
            var materialDto = MaterialData.GetMaterialDtoWithoutTags();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", courseId);

            _courseRepositoryMock.Setup(x => x.GetCourse(courseId));
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId)).Returns(materialDto);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.RemoveCourseMaterialReference(courseId, materialId));
            //Then
            _courseRepositoryMock.Verify(x => x.RemoveCourseMaterialReference(courseId, materialId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void RemoveCourseMaterialReference_NotValidMaterialId_EntityNotFoundExceptionThrown()
        {
            //Given
            var courseId = 2;
            var materialId = 4;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "material", materialId);

            _courseRepositoryMock.Setup(x => x.GetCourse(courseId)).Returns(new CourseDto() { Id = courseId }); ;
            _materialRepositoryMock.Setup(x => x.GetMaterialById(materialId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() => _sut.RemoveCourseMaterialReference(courseId, materialId));
            //Then
            _courseRepositoryMock.Verify(x => x.RemoveCourseMaterialReference(courseId, materialId), Times.Never);
            Assert.That(result.Message, Is.EqualTo(exp));
        }
        [Test]
        public void SelectAllTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 0;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            _sut.SelectAllTopicsByCourseId(givenCourseId));
            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId)));
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseId(givenCourseId), Times.Never);
        }

        [Test]
        public void AddTaskToCourse_WithTaskIdAndCourseId_Added()
        {
            //Given
            var givenCourseId = 3;
            var givenTaskId = 8;
            _courseRepositoryMock.Setup(x => x.AddTaskToCourse(givenCourseId, givenTaskId));

            //When
            _sut.AddTaskToCourse(givenCourseId, givenTaskId);
            //Then
            _courseRepositoryMock.Verify(x => x.AddTaskToCourse(givenCourseId, givenTaskId), Times.Once);
        }

        [Test]
        public void DeleteTaskFromCourse_WithTaskIdAndCourseId_Deleted()
        {
            //Given
            var givenCourseId = 3;
            var givenTaskId = 8;
            _courseRepositoryMock.Setup(x => x.DeleteTaskFromCourse(givenCourseId, givenTaskId));
            //When
            _sut.DeleteTaskFromCourse(givenCourseId, givenTaskId);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteTaskFromCourse(givenCourseId, givenTaskId), Times.Once);
        }

        [Test]
        public void AddTopicToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(topic));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTopicToCourse(givenCourseId, givenTopicId, topic));
            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(topic), Times.Never);
        }

        [Test]
        public void AddTopicToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(topic));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTopicToCourse(givenCourseId, givenTopicId, topic));
            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(topic), Times.Never);
        }

        [Test]
        public void AddTopicsToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Give
            var givenCourseId = 2;
            var courseTopic = CourseData.GetListCourseTopicDto();
            List<TopicDto> topicsInDB = CourseData.GetTopicsFromBDUseWhenTopicAbsent();

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courseTopic));
            //When
            var exp = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTopicsToCourse(givenCourseId, courseTopic));
            //Then
            Assert.That(ServiceMessages.EntityNotFound, Is.EqualTo(exp.Message));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courseTopic), Times.Never);

        }

        [Test]
        public void AddTopicsToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenCourseId = 2;
            var courseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            List<TopicDto> topicsInDB = CourseData.GetTopics();

            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courseTopic));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.AddTopicsToCourse(givenCourseId, courseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courseTopic), Times.Never);
        }

        [Test]
        public void DeleteTopicFromCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Never);
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenCourseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenCourseTopic), Times.Never);

        }

        [Test]
        public void UpdateCourseTopicsByCourseId_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenCourseTopic = CourseData.GetListCourseTopicDto();
            var topicsInBd = CourseData.GetTopicsFromBDUseWhenTopicAbsent();
            var exp = ServiceMessages.EntityNotFound;

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInBd);
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenCourseTopic), Times.Never);

        }

        [Test]
        public void DeleteAllTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            _sut.DeleteAllTopicsByCourseId(givenCourseId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);

        }

        [Test]
        public void AddMaterialToCourse_ExistingCourseIdAndMaterialIdIdPassed_MaterialAddedToCourse()
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
        public void DeleteMaterialFromCourse_ExistingCourseIdAndMaterialIdIdPassed_MaterialRemovedFromCourse()
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

        [Test]
        public void AddMaterialToCourse_WhenCourseIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var course = CourseData.GetCourseDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), course.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddCourseMaterialReference(course.Id, material.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void AddMaterialToCourse_WhenMaterialIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var course = CourseData.GetCourseDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);
            _courseRepositoryMock.Setup(x => x.GetCourse(course.Id)).Returns(CourseData.GetCourseDto());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddCourseMaterialReference(course.Id, material.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _courseRepositoryMock.Verify(x => x.GetCourse(course.Id), Times.Once);
        }

        [Test]
        public void DeleteMaterialToCourse_WhenCourseIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var course = CourseData.GetCourseDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), course.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.RemoveCourseMaterialReference(course.Id, material.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [Test]
        public void DeleteMaterialToCourse_WhenMaterialIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown()
        {
            //Given
            var course = CourseData.GetCourseDto();
            var material = MaterialData.GetMaterialDtoWithoutTags();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), material.Id);
            _courseRepositoryMock.Setup(x => x.GetCourse(course.Id)).Returns(CourseData.GetCourseDto());

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.RemoveCourseMaterialReference(course.Id, material.Id));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _courseRepositoryMock.Verify(x => x.GetCourse(course.Id), Times.Once);
        }
        [Test]
        public void GetCourseTopicById_ValidId_CourseTopicWasGotten()

        {

            //Given

            var id = 3;

            _topicRepositoryMock.Setup(x => x.GetCourseTopicById(id)).Returns(new CourseTopicDto() { Id = id });

            //When

            _sut.GetCourseTopicById(id);

            //Then

            _topicRepositoryMock.Verify(x => x.GetCourseTopicById(id), Times.Once);

        }
        [Test]
        public void GetCourseTopicById_NotValidId_EntityNotFoundExceptionThrown()

        {

            //Given

            var id = 3;

            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "courseTopic", id);

            _topicRepositoryMock.Setup(x => x.GetCourseTopicById(id));

            //When

            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetCourseTopicById(id));

            //Then

            Assert.That(result.Message, Is.EqualTo(exp));

            _topicRepositoryMock.Verify(x => x.GetCourseTopicById(id), Times.Once);

        }
        [Test]
        public void GetCourseTopicBySeveralId_ValidCourseTopicIds_CourseTopicsWereGotten()

        {

            //Given

            var ids = new List<int>() { 15, 21, 13 };

            var courseTopicsInBd = CourseData.GetListCourseTopicDtoFromDataBase();

            _topicRepositoryMock.Setup(x => x.GetCourseTopicBySeveralId(ids)).Returns(courseTopicsInBd);

            //When

            _sut.GetCourseTopicBySeveralId(ids);

            //Then

            _topicRepositoryMock.Verify(x => x.GetCourseTopicBySeveralId(ids), Times.Once);

        }
        [Test]
        public void GetCourseTopicBySeveralId_NotValidCourseTopicIds_EntityNotFoundExceptionThrown()

        {

            //Given

            var ids = new List<int>() { 15, 22, 13 };

            var courseTopicsInBd = CourseData.GetListCourseTopicDtoFromDataBase();

            _topicRepositoryMock.Setup(x => x.GetCourseTopicBySeveralId(ids)).Returns(courseTopicsInBd);

            //When

            var result = Assert.Throws<EntityNotFoundException>(() => _sut.GetCourseTopicBySeveralId(ids));

            //Then

            Assert.That(result.Message, Is.EqualTo(ServiceMessages.EntityNotFound));

            _topicRepositoryMock.Verify(x => x.GetCourseTopicBySeveralId(ids), Times.Once);

        }
    }
}