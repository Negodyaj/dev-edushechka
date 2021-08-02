﻿using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private UserValidationHelper _userValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private LessonService _sut;        

        [SetUp]
        public void Setup()
        {
            _lessonRepoMock = new Mock<ILessonRepository>();
            _commentRepoMock = new Mock<ICommentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepoMock.Object);
            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);


            _sut = new LessonService(
                    _lessonRepoMock.Object,
                    _commentRepoMock.Object,
                    _userRepoMock.Object,
                    _userValidationHelper,
                    _lessonValidationHelper);
        }

        [Test]
        public void AddTopicToLesson_LessonIdTopicId_TopicLessonReferenceCreated()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepoMock.Setup(x => x.AddTopicToLesson(lessonId, topicId));

            

            //When
            _sut.AddTopicToLesson(lessonId, topicId);

            //Then
            _lessonRepoMock.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void DeleteTopicFromLesson_LessonIdTopicId_TopicLessonReferenceDeleted()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepoMock.Setup(x => x.DeleteTopicFromLesson(lessonId, topicId));

            //When
            _sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonRepoMock.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_SimpleDto_LessonAdded()
        {
            //Given
            var expectedId = LessonData.LessonId;
            var lessonDto = LessonData.GetAddedLessonDto();
            var topicIds = new List<int>(){ 6, 7};

            _lessonRepoMock.Setup(x => x.AddLesson(lessonDto)).Returns(expectedId);
            foreach (int topicId in topicIds)
            {
                _lessonRepoMock.Setup(x => x.AddTopicToLesson(expectedId, topicId));
            }

            
            //When
            var actualId = _sut.AddLesson(lessonDto, topicIds);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _lessonRepoMock.Verify(x => x.AddLesson(lessonDto), Times.Once);
            foreach (int topicId in topicIds)
            {
                _lessonRepoMock.Verify(x => x.AddTopicToLesson(expectedId, topicId), Times.Once);
            }
        }

        [Test]
        public void SelectAllLessonsByGroupId_ExistingGroupIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();

            var groupId = 9;

            _lessonRepoMock.Setup(x => x.SelectAllLessonsByGroupId(groupId)).Returns(expected);

           

            //When
            var actual = _sut.SelectAllLessonsByGroupId(groupId);

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
                        
            //When
            var actual = _sut.SelectAllLessonsByTeacherId(teacherId);

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


            //When
            var actual = _sut.SelectLessonById(lessonId);

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

            
            //When
            var actual = _sut.SelectLessonWithCommentsById(lessonId);

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
                        
            //When
            var actual = _sut.SelectLessonWithCommentsAndStudentsById(lessonId);

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

            
            //When
            var actual = _sut.UpdateLesson(updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

    
        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.AddStudentToLesson(lessonId, userId));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);            

            //When
            var dto = _sut.AddStudentToLesson(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.AddStudentToLesson(lessonId, userId), Times.Once);           
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.DeleteStudentFromLesson(lessonId, userId));


            //When
            _sut.DeleteStudentFromLesson(lessonId,userId);

            //Than
            _lessonRepoMock.Verify(x => x.DeleteStudentFromLesson(lessonId, userId), Times.Once);
        }


        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentFeedbackForLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(LessonData.GetUserDto);
                       
            //When
            var dto = _sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentFeedbackForLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);


            //When
            var dto = _sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var lessonId = 30;
            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepoMock.Setup(x => x.SelectAllFeedbackByLessonId(lessonId)).Returns(listStudentLessonDto);
             
            //When
            var listOfDto = _sut.SelectAllFeedbackByLessonId(lessonId);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepoMock.Verify(x => x.SelectAllFeedbackByLessonId(lessonId), Times.Once);
        }
    }
}