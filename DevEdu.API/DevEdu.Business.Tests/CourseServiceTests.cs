﻿using DevEdu.Business.Constants;
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
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class CourseServiceTests
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private CourseValidationHelper _courseValidationHelper;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IMaterialRepository> _materialRepositoryMock;
        private CourseService _sut;
        private Mock<ITaskRepository> _taskRepositoryMock;
        private ITaskValidationHelper _taskValidationHelper;
        private Mock<ITopicRepository> _topicRepositoryMock;
        private TopicValidationHelper _topicValidationHelper;


        [SetUp]
        public void Setup()
        {
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _groupRepositoryMock = new Mock<IGroupRepository>();
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _courseValidationHelper = new CourseValidationHelper(_courseRepositoryMock.Object, _groupRepositoryMock.Object);
            _topicValidationHelper = new TopicValidationHelper(_topicRepositoryMock.Object);
            _taskValidationHelper = new TaskValidationHelper(_taskRepositoryMock.Object, _groupRepositoryMock.Object, _courseRepositoryMock.Object);
            _topicValidationHelper = new TopicValidationHelper(_topicRepositoryMock.Object);
            _sut = new CourseService(
                _courseRepositoryMock.Object,
                _topicRepositoryMock.Object,
                _taskRepositoryMock.Object,
                _materialRepositoryMock.Object,
                _groupRepositoryMock.Object,
                _courseValidationHelper,
                _topicValidationHelper,
                _taskValidationHelper
            );
        }

        [Test]
        public void AddCourse_CourseInput_CourseCreated()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;

            _courseRepositoryMock.Setup(x => x.AddCourseAsync(courseDto)).ReturnsAsync(courseId);
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);

            //When
            var actualCourse = _sut.AddCourseAsync(courseDto);

            //Than
            Assert.AreEqual(courseId, actualCourse.Id);
            _courseRepositoryMock.Verify(x => x.AddCourseAsync(courseDto), Times.Once());
        }

        [Test]
        public async Task DeleteCourse_IntCourseId_DeleteCourseAsync()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;

            _courseRepositoryMock.Setup(x => x.DeleteCourseAsync(courseId));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);

            //When
            await _sut.DeleteCourseAsync(courseId);

            //Than
            _courseRepositoryMock.Verify(x => x.DeleteCourseAsync(courseId), Times.Once());
            _courseRepositoryMock.Verify(x => x.GetCourseAsync(courseId), Times.Once());
        }

        [Test]
        public void GetCourseById_IntCourseId_ReturnCourseWithTopics()
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);

            //When
            var dto = _sut.GetCourseAsync(courseId).Result;

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourseAsync(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseIdAsync(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseIdAsync(courseId), Times.Never);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseIdAsync(courseId), Times.Never);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        public void GetCourseById_IntCourseIdByTeacherOrTutor_ReturnCourseWithTopics_Groups_Materials_Tasks(Enum role)
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            var userToken = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var groupsByCourse = new List<GroupDto>
            {
                new() { Id = 1 }
            };
            var groupsByUser = new List<GroupDto>
            {
                new() { Id = 1 }
            };
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);
            _groupRepositoryMock.Setup(x => x.GetGroupsByCourseIdAsync(courseId)).ReturnsAsync(groupsByCourse);
            _groupRepositoryMock.Setup(x => x.GetGroupsByUserIdAsync(courseId)).ReturnsAsync(groupsByUser);

            //When
            var dto = _sut.GetFullCourseInfoAsync(courseId, userToken).Result;

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourseAsync(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseIdAsync(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseIdAsync(courseId), Times.Once);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseIdAsync(courseId), Times.Exactly(2));
        }

        [TestCase(Role.Methodist)]
        public void GetCourseById_IntCourseIdByMethodist_ReturnCourseWithTopics_Groups_Materials(Enum role)
        {
            //Given
            var courseDto = CourseData.GetCourseDto();
            var courseId = 1;
            var userToken = UserIdentityInfoData.GetUserIdentityWithRole(role);
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);

            //When
            var dto = _sut.GetFullCourseInfoAsync(courseId, userToken).Result;

            //Than
            Assert.AreEqual(courseDto, dto);
            _courseRepositoryMock.Verify(x => x.GetCourseAsync(courseId), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseIdAsync(courseId), Times.Once);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseIdAsync(courseId), Times.Once);
            _groupRepositoryMock.Verify(x => x.GetGroupsByCourseIdAsync(courseId), Times.Once);
        }

        [Test]
        public async Task GetCourses_WithoutParams_ReturnListCoursesAsync()
        {
            //Given
            var courseList = CourseData.GetListCourses();

            _courseRepositoryMock.Setup(x => x.GetCoursesAsync()).ReturnsAsync(courseList);

            //When
            var actualCourseList = await _sut.GetCoursesAsync();

            //Then
            Assert.AreEqual(courseList, actualCourseList);
            _courseRepositoryMock.Verify(x => x.GetCoursesAsync(), Times.Once);
            foreach (var course in actualCourseList)
            {
                _taskRepositoryMock.Verify(x => x.GetTasksByCourseIdAsync(course.Id), Times.Never);
                _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseIdAsync(course.Id), Times.Never);
                _topicRepositoryMock.Verify(x => x.GetTopicsByCourseIdAsync(course.Id), Times.Never);
            }
        }

        [Test]
        public void UpdateCourseInt_CourseId_Name_Descriptions_ReturnUpdatedCourse()
        {
            //When
            var courseId = 1;
            var courseDto = CourseData.GetCourseDto();
            var updCourseDto = CourseData.GetUpdCourseDto();

            _courseRepositoryMock.Setup(x => x.UpdateCourseAsync(courseDto));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseDto.Id)).ReturnsAsync(updCourseDto);

            //When
            var actualCourseDto = _sut.UpdateCourseAsync(courseId, courseDto).Result;

            //Then
            Assert.AreEqual(updCourseDto, actualCourseDto);
            _courseRepositoryMock.Verify(x => x.UpdateCourseAsync(courseDto), Times.Once);
            _taskRepositoryMock.Verify(x => x.GetTasksByCourseIdAsync(courseId), Times.Never);
            _materialRepositoryMock.Verify(x => x.GetMaterialsByCourseIdAsync(courseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.GetTopicsByCourseIdAsync(courseId), Times.Never);
        }

        [Test]
        public async Task AddTopicToCourse_WithCourseIdAndSimpleDto_TopicWasAddedAsync()
        {
            //Given
            var givenCourseId = 12;
            var givenTopicId = 8;
            var courseTopicDto = new CourseTopicDto { Position = 3 };

            _topicRepositoryMock.Setup(x => x.AddTopicToCourseAsync(courseTopicDto));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopicAsync(givenTopicId)).ReturnsAsync(new TopicDto { Id = givenTopicId });

            //When
            await _sut.AddTopicToCourseAsync(givenCourseId, givenTopicId, courseTopicDto);

            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicToCourseAsync(courseTopicDto), Times.Once);

        }

        [Test]
        public async Task AddTopicsToCourse_WithCourseIdAndListSimpleDto_TopicsWereAddedAsync()
        {
            //Given
            var givenCourseId = 2;
            var courseTopicsDto = CourseData.GetListCourseTopicDto();
            var topicsDto = CourseData.GetTopics();

            _topicRepositoryMock.Setup(x => x.AddTopicsToCourseAsync(courseTopicsDto));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsDto);

            //When
            await _sut.AddTopicsToCourseAsync(givenCourseId, courseTopicsDto);

            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourseAsync(courseTopicsDto), Times.Once);
        }

        [Test]
        public async Task DeleteTopicFromCourse_ByCourseIdAndTopicId_TopicDeletedFromCourseAsync()
        {
            //Given
            var givenCourseId = 4;
            var givenTopicId = 7;

            _topicRepositoryMock.Setup(x => x.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopicAsync(givenTopicId)).ReturnsAsync(new TopicDto { Id = givenTopicId });

            //When
            await _sut.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId);

            //Then
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId), Times.Once);
        }

        [Test]
        public async Task SelectAllTopicsByCourseId_ByCourseId_GotListOfCourseTopicsAsync()
        {
            //Given
            var givenCourseId = 4;

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });

            //When
            await _sut.SelectAllTopicsByCourseIdAsync(givenCourseId);

            //Then
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId), Times.Once);

        }

        [Test]
        public async Task UpdateCourseTopicsByCourseId_WhenCountOfTopicsIsChanged_ThenDeleteAndInsertMethodsCalledAsync()
        {
            //Given
            var givenCourseId = 7;
            var givenCourseTopicsToUpdate = new List<CourseTopicDto>
            {
                new() { Position = 1, Id = 8, Topic = new TopicDto { Id = 8 } },
                new() { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } },
                new() { Position = 6, Id = 9, Topic = new TopicDto { Id = 9 } },
                new() { Position = 8, Id = 2, Topic = new TopicDto { Id = 2 } }
            };

            var topicsInDb = CourseData.GetTopics();
            var courseToicsFromDb = CourseData.GetListCourseTopicDtoFromDataBase();

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId)).ReturnsAsync(courseToicsFromDb);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenCourseTopicsToUpdate));
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsInDb);
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto
                { Id = givenCourseId });

            //When
            await _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenCourseTopicsToUpdate);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Once);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourseAsync(givenCourseTopicsToUpdate), Times.Once);
        }

        [Test]
        public async Task UpdateCourseTopicsByCourseId_TopicsInDatabaseAreAbsentForCourse_AddedTopicsForCourseAsync()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var courseToicsFromDb = new List<CourseTopicDto>();
            var topicsInDb = CourseData.GetTopics();

            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsInDb);
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId)).ReturnsAsync(courseToicsFromDb);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto
                { Id = givenCourseId });

            //When
            await _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenTopicsToUpdate);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourseAsync(givenTopicsToUpdate), Times.Once);
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsToUpdateNotProvided_ThenUpdateTerminates()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            var topicsFromDb = CourseData.GetListCourseTopicDtoFromDataBase();

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId)).ReturnsAsync(topicsFromDb);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenTopicsToUpdate));

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Never);
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
            var topicsFromDb = CourseData.GetListCourseTopicDtoFromDataBase();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 1, Topic = new TopicDto { Id = 1 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 2, Topic = new TopicDto { Id = 2 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 3, Topic = new TopicDto { Id = 3 } });

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId)).ReturnsAsync(topicsFromDb);
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsDto);
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto
                { Id = givenCourseId });
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenTopicsToUpdate));

            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SamePositionsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsAreNotUnique_ValidationExceptionThrown()
        {
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>
            {
                new() { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } },
                new() { Position = 3, Id = 21, Topic = new TopicDto { Id = 21 } },
                new() { Position = 1, Id = 15, Topic = new TopicDto { Id = 15 } }
            };

            List<TopicDto> topicsDto = new()
            {
                new TopicDto { Id = 15 },
                new TopicDto { Id = 21 },
                new TopicDto { Id = 15 }
            };


            var topicsFromDb = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto
                { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsDto);
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId)).ReturnsAsync(topicsFromDb);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));

            //When
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenTopicsToUpdate));

            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SameTopicsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }

        [Test]
        public void SelectAllTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 0;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId));

            //When
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.SelectAllTopicsByCourseIdAsync(givenCourseId));

            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId)));
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseIdAsync(givenCourseId), Times.Never);
        }

        [Test]
        public void AddTaskToCourse_WithTaskIdAndCourseId_Added()
        {
            //Given
            var courseId = 3;
            var taskId = 8;
            var task = new TaskDto { Id = taskId };
            var courseDto = CourseData.GetCourseDto();

            _taskRepositoryMock.Setup(x => x.AddTaskAsync(task));
            _taskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(task);
            _courseRepositoryMock.Setup(x => x.AddTaskToCourseAsync(courseId, taskId));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);

            //When
            _sut.AddTaskToCourseAsync(courseId, taskId).Wait();

            //Then
            _courseRepositoryMock.Verify(x => x.AddTaskToCourseAsync(courseId, taskId), Times.Once);
        }

        [Test]
        public async Task DeleteTaskFromCourse_WithTaskIdAndCourseId_DeletedAsync()
        {
            //Given
            var courseId = 3;
            var taskId = 8;
            var courseDto = CourseData.GetCourseDto();
            var taskDto = TaskData.GetTaskDto();

            _courseRepositoryMock.Setup(x => x.DeleteTaskFromCourseAsync(courseId, taskId));
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(courseId)).ReturnsAsync(courseDto);
            _taskRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId)).ReturnsAsync(taskDto);

            //When
            await _sut.DeleteTaskFromCourseAsync(courseId, taskId);

            //Then
            _courseRepositoryMock.Verify(x => x.DeleteTaskFromCourseAsync(courseId, taskId), Times.Once);
        }

        [Test]
        public void AddTopicToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            _topicRepositoryMock.Setup(x => x.AddTopicToCourseAsync(topic));

            //When
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTopicToCourseAsync(givenCourseId, givenTopicId, topic));

            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourseAsync(topic), Times.Never);
        }

        [Test]
        public void AddTopicToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.AddTopicToCourseAsync(topic));

            //When
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTopicToCourseAsync(givenCourseId, givenTopicId, topic));

            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourseAsync(topic), Times.Never);
        }

        [Test]
        public void AddTopicsToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Give
            var givenCourseId = 2;
            var courseTopic = CourseData.GetListCourseTopicDto();
            List<TopicDto> topicsInDB = CourseData.GetTopicsFromBDUseWhenTopicAbsent();

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsInDB);
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourseAsync(courseTopic));

            //When
            var exp = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTopicsToCourseAsync(givenCourseId, courseTopic));

            //Then
            Assert.That(ServiceMessages.EntityNotFound, Is.EqualTo(exp.Message));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourseAsync(courseTopic), Times.Never);

        }

        [Test]
        public void AddTopicsToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenCourseId = 2;
            var courseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var topicsInDB = CourseData.GetTopics();

            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId));
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourseAsync(courseTopic));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.AddTopicsToCourseAsync(givenCourseId, courseTopic));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourseAsync(courseTopic), Times.Never);
        }

        [Test]
        public void DeleteTopicFromCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId), Times.Never);
        }

        [Test]
        public void DeleteTopicFromCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto
                { Id = givenCourseId });

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourseAsync(givenCourseId, givenTopicId), Times.Never);
        }

        [Test]
        public void UpdateCourseTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenCourseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenCourseTopic));

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

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId)).ReturnsAsync(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopicsAsync()).ReturnsAsync(topicsInBd);

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.UpdateCourseTopicsByCourseIdAsync(givenCourseId, givenCourseTopic));

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

            _courseRepositoryMock.Setup(x => x.GetCourseAsync(givenCourseId));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _sut.DeleteAllTopicsByCourseIdAsync(givenCourseId));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseIdAsync(givenCourseId), Times.Never);

        }

        [Test]
        public async Task GetCourseTopicById_ValidId_CourseTopicWasGottenAsync()
        {
            //Given
            var id = 3;

            _topicRepositoryMock.Setup(x => x.GetCourseTopicByIdAsync(id)).ReturnsAsync(new CourseTopicDto { Id = id });

            //When
            await _sut.GetCourseTopicByIdAsync(id);

            //Then
            _topicRepositoryMock.Verify(x => x.GetCourseTopicByIdAsync(id), Times.Once);
        }

        [Test]
        public void GetCourseTopicById_NotValidId_EntityNotFoundExceptionThrown()
        {
            //Given
            var id = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "courseTopic", id);

            _topicRepositoryMock.Setup(x => x.GetCourseTopicByIdAsync(id));

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCourseTopicByIdAsync(id));

            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.GetCourseTopicByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task GetCourseTopicBySeveralId_ValidCourseTopicIds_CourseTopicsWereGottenAsync()
        {
            //Given
            var ids = new List<int> { 15, 21, 13 };
            var courseTopicsInBd = CourseData.GetListCourseTopicDtoFromDataBase();

            _topicRepositoryMock.Setup(x => x.GetCourseTopicBySeveralIdAsync(ids)).ReturnsAsync(courseTopicsInBd);

            //When
            await _sut.GetCourseTopicBySeveralIdAsync(ids);

            //Then
            _topicRepositoryMock.Verify(x => x.GetCourseTopicBySeveralIdAsync(ids), Times.Once);
        }

        [Test]
        public void GetCourseTopicBySeveralId_NotValidCourseTopicIds_EntityNotFoundExceptionThrown()
        {
            //Given
            var ids = new List<int> { 15, 22, 13 };
            var courseTopicsInBd = CourseData.GetListCourseTopicDtoFromDataBase();

            _topicRepositoryMock.Setup(x => x.GetCourseTopicBySeveralIdAsync(ids)).ReturnsAsync(courseTopicsInBd);

            //When
            var result = Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.GetCourseTopicBySeveralIdAsync(ids));

            //Then
            Assert.That(result.Message, Is.EqualTo(ServiceMessages.EntityNotFound));
            _topicRepositoryMock.Verify(x => x.GetCourseTopicBySeveralIdAsync(ids), Times.Once);
        }
    }
}