using System;
using System.Collections.Generic;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class LessonServiceTests
    {
        private readonly Mock<ILessonRepository> _lessonRepoMock;
        private readonly Mock<ICommentRepository> _commentRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;

        public LessonServiceTests()
        {
            _lessonRepoMock = new Mock<ILessonRepository>();
            _commentRepoMock = new Mock<ICommentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
        }

        [Test]
        public void AddLesson_SimpleDto_CreatedLesson()
        {
            //Given
            var expected = LessonData.LessonId;

            var lessonDto = LessonData.GetAddedLessonDto();

            _lessonRepoMock.Setup(x => x.AddLesson(lessonDto)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);
            //When
            var actualId = sut.AddLesson(lessonDto);

            //Then
            Assert.AreEqual(expected, actualId);
            _lessonRepoMock.Verify(x => x.AddLesson(lessonDto), Times.Once);
        }

        [Test]
        public void SelectAllLessons_IntGroupId_ListOfLessonDto()
        {
            //Given
            var expected = LessonData.GetLessons();

            var groupId = 9;

            _lessonRepoMock.Setup(x => x.SelectAllLessonsByGroupId(groupId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);

            //When
            var actual = sut.SelectAllLessonsByGroupId(groupId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectAllLessonsByGroupId(groupId), Times.Once);
        }

        [Test]
        public void SelectAllLessons_IntTeacherId_ListOfLessonDto()
        {
            //Given
            var expected = LessonData.GetLessons();

            var teacherId = 3;

            _lessonRepoMock.Setup(x => x.SelectAllLessonsByTeacherId(teacherId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);

            //When
            var actual = sut.SelectAllLessonsByTeacherId(teacherId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectAllLessonsByTeacherId(teacherId), Times.Once);
        }

        [Test]
        public void SelectLesson_IntLessonId_LessonDto()
        {
            //Given
            var expected = LessonData.GetSelectedLessonDto();

            var lessonId = LessonData.LessonId;

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);

            //When
            var actual = sut.SelectLessonById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }

        [Test]
        public void SelectLessonWithComments_IntLessonId_LessonDto()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetComments();

            var expected = lesson;
            expected.Comments = comments;

            var lessonId = LessonData.LessonId;

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepoMock.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);

            //When
            var actual = sut.SelectLessonWithCommentsById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.AtLeastOnce);
            _commentRepoMock.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudents_IntLessonId_LessonDto()
        {
            //Given
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetComments();
            var students = LessonData.GetStudents();

            var expected = lesson;
            expected.Comments = comments;
            expected.Students = students;

            var lessonId = LessonData.LessonId;

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepoMock.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);
            _lessonRepoMock.Setup(x => x.SelectStudentsLessonByLessonId(lessonId)).Returns(students);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);

            //When
            var actual = sut.SelectLessonWithCommentsAndStudentsById(lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.AtLeastOnce);
            _commentRepoMock.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.AtLeastOnce);
            _lessonRepoMock.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void UpdateLesson_SimlpeDtoWithoutTeacher_UpdatedLessonDto()
        {
            //Given
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();

            var expected = LessonData.GetUpdatedLessonDto();

            _lessonRepoMock.Setup(x => x.UpdateLesson(updatedLesson));
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object);
            //When
            var actual = sut.UpdateLesson(updatedLesson);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepoMock.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.AtLeastOnce);
        }

    }
}
