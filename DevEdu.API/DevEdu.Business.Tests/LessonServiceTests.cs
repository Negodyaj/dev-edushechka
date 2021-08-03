using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using Moq;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DevEdu.Business.Tests.Data;

namespace DevEdu.Business.Tests
{
    class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepository;
        private Mock<ICommentRepository> _commentRepository;
        private Mock<ITopicRepository> _topicRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;
        private UserValidationHelper _userValidationHelper;
        private ILessonValidationHelper _lessonValidationHelper;
        private ITopicValidationHelper _topicValidationHelper;
        private IGroupValidationHelper _groupValidationHelper;

        private ILessonService _sut;

        [SetUp]
        public void Setup()
        {
            _lessonRepository = new Mock<ILessonRepository>();
            _commentRepository = new Mock<ICommentRepository>();
            _topicRepository = new Mock<ITopicRepository>();
            _groupRepository = new Mock<IGroupRepository>();
            _userRepository = new Mock<IUserRepository>();
            _userValidationHelper = new UserValidationHelper(_userRepository.Object);
            _lessonValidationHelper = new LessonValidationHelper(
                _lessonRepository.Object, 
                _groupRepository.Object, 
                _userRepository.Object
            );
            _topicValidationHelper = new TopicValidationHelper(_topicRepository.Object);
            _groupValidationHelper = new GroupValidationHelper(_groupRepository.Object);

            _sut = new LessonService(_lessonRepository.Object,
                    _commentRepository.Object,
                    _userValidationHelper,
                    _lessonValidationHelper,
                    _topicValidationHelper,
                    _groupValidationHelper);
        }

        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();

            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.AddStudentToLesson(lessonId, userId));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.AddStudentToLesson(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.AddStudentToLesson(lessonId, userId), Times.Once);
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.DeleteStudentFromLesson(lessonId, userId));

            //When
            _sut.DeleteStudentFromLesson(lessonId, userId);

            //Than
            _lessonRepository.Verify(x => x.DeleteStudentFromLesson(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentFeedbackForLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepository.Setup(x => x.SelectUserById(userId)).Returns(LessonData.GetUserDto);

            //When
            var dto = _sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentFeedbackForLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _userRepository.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepository.Setup(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto));
            _lessonRepository.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepository.Verify(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto), Times.Once);
            _lessonRepository.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var lessonId = 30;
            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepository.Setup(x => x.SelectAllFeedbackByLessonId(lessonId)).Returns(listStudentLessonDto);

            //When
            var listOfDto = _sut.SelectAllFeedbackByLessonId(lessonId);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepository.Verify(x => x.SelectAllFeedbackByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void AddTopicToLesson_LessonIdTopicId_TopicLessonReferenceCreated()
        {
            //Given
            var lessonId = 5;
            var topicId = 7;
            _lessonRepository.Setup(x => x.AddTopicToLesson(lessonId, topicId));

            //When
            _sut.AddTopicToLesson(lessonId, topicId);

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

            //When
            _sut.DeleteTopicFromLesson(lessonId, topicId);

            //Then
            _lessonRepository.Verify(x => x.DeleteTopicFromLesson(lessonId, topicId), Times.Once);
        }

        [Test]
        public void AddLesson_UserDtoAndSimpleDtoAndListOfTopicsPassed_LessonAdded()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lessonId = LessonData.LessonId;
            var addedLesson = LessonData.GetAddedLessonDto();
            var topicIds = TopicData.GetListTopicId();
            var topics = TopicData.GetListTopicDto();

            var expectedLesson = addedLesson;
            expectedLesson.Id = lessonId;

            _lessonRepository.Setup(x => x.AddLesson(addedLesson)).Returns(lessonId);
            for(int i = 0; i < topics.Count; i++)
            {
                _topicRepository.Setup(x => x.GetTopic(topicIds[i])).Returns(topics[i]);
                _lessonRepository.Setup(x => x.AddTopicToLesson(lessonId, topicIds[i]));
            }
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expectedLesson);

            //When
            var actualLesson = _sut.AddLesson(userIdentity, addedLesson, topicIds);

            //Then
            Assert.AreEqual(expectedLesson, actualLesson);
            _lessonRepository.Verify(x => x.AddLesson(addedLesson), Times.Once);
            foreach (int topicId in topicIds)
            {
                _topicRepository.Verify(x => x.GetTopic(topicId), Times.Once);
                _lessonRepository.Verify(x => x.AddTopicToLesson(lessonId, topicId), Times.Once);
            }
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expectedLesson);
        }

        [Test]
        public void SelectAllLessonsByGroupId_UserDtoAndExistingGroupIdPassed_LessonsReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var expected = LessonData.GetLessons();
            var groupId = 1;
            var groups = GroupData.GetGroupsDto();
                 
            _lessonRepository.Setup(x => x.SelectAllLessonsByGroupId(groupId)).Returns(expected);
            if (CheckerRole.IsStudent(userIdentity.Roles))
            {
                _groupRepository.Setup(x => x.GetGroupsByStudentId(groupId)).Returns(groups);
            }

            //When
            var actual = _sut.SelectAllLessonsByGroupId(userIdentity, groupId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByGroupId(groupId), Times.Once);
            _groupRepository.Verify(x => x.GetGroupsByStudentId(groupId), Times.AtMostOnce);
        }

        [Test]
        public void SelectAllLessonsByTeacherId_ExistingTeacherIdPassed_LessonsReturned()
        {
            //Given
            var expected = LessonData.GetLessons();
            var teacherId = 3;
            var teacherDto = UserData.GetTeacherIdentity();

            _lessonRepository.Setup(x => x.SelectAllLessonsByTeacherId(teacherId)).Returns(expected);
            _userRepository.Setup(x => x.SelectUserById(teacherId)).Returns(teacherDto);

            //When
            var actual = _sut.SelectAllLessonsByTeacherId(teacherId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectAllLessonsByTeacherId(teacherId), Times.Once);
            _userRepository.Verify(x => x.SelectUserById(teacherId), Times.Once);
        }

        [Test]
        public void SelectLessonWithCommentsById_UserDtoAndExistingLessonIdPassed_LessonWithCommentsReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lessonId = LessonData.LessonId;
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();

            var expected = lesson;
            expected.Comments = comments;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);

            //When
            var actual = _sut.SelectLessonWithCommentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Exactly(2));
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Never);
        }

        [Test]
        public void SelectLessonWithCommentsAndStudentsById_ExistingLessonIdPassed_LessonWithCommentsAndAttendancesReturned()
        {
            //Given
            var userIdentity = UserData.GetAdminIdentity();
            var lesson = LessonData.GetSelectedLessonDto();
            var comments = CommentData.GetListCommentsDto();
            var students = LessonData.GetAttendances();

            var expectedLesson = lesson;
            expectedLesson.Comments = comments;
            expectedLesson.Students = students;

            var lessonId = LessonData.LessonId;

            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(lesson);
            _commentRepository.Setup(x => x.SelectCommentsFromLessonByLessonId(lessonId)).Returns(comments);
            _lessonRepository.Setup(x => x.SelectStudentsLessonByLessonId(lessonId)).Returns(students);

            //When
            var actual = _sut.SelectLessonWithCommentsAndStudentsById(userIdentity, lessonId);

            //Then
            Assert.AreEqual(expectedLesson, actual);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Exactly(2));
            _commentRepository.Verify(x => x.SelectCommentsFromLessonByLessonId(lessonId), Times.Once);
            _lessonRepository.Verify(x => x.SelectStudentsLessonByLessonId(lessonId), Times.Once);
        }

        [Test]
        public void UpdateLesson_SimpleDtoWithoutTeacherPassed_UpdatedLessonReturned()
        {
            //Given
            var userIdentity = UserData.GetTeacherIdentity();
            var lessonId = LessonData.LessonId;
            var updatedLesson = LessonData.GetUpdatedLessonDto();

            var expected = LessonData.GetUpdatedLessonDto();

            _lessonRepository.Setup(x => x.UpdateLesson(updatedLesson));
            _lessonRepository.Setup(x => x.SelectLessonById(lessonId)).Returns(expected);

            //When
            var actual = _sut.UpdateLesson(userIdentity, updatedLesson, lessonId);

            //Then
            Assert.AreEqual(expected, actual);
            _lessonRepository.Verify(x => x.UpdateLesson(updatedLesson), Times.Once);
            _lessonRepository.Verify(x => x.SelectLessonById(lessonId), Times.Once);
        }
    }
}
