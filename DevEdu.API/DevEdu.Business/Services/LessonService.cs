using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ILessonValidationHelper _lessonValidationHelper;

        public LessonService(
            ILessonRepository lessonRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IUserValidationHelper userValidationHelper,
            ILessonValidationHelper lessonValidationHelper
        )
        {
            _lessonRepository = lessonRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userValidationHelper = userValidationHelper;
            _lessonValidationHelper = lessonValidationHelper;
        }

        public void AddCommentToLesson(int lessonId, CommentDto commentDto)
        {
            int commentId = _commentRepository.AddComment(commentDto);

            _lessonRepository.AddCommentToLesson(lessonId, commentId);
        }

        public int AddLesson(LessonDto lessonDto, List<int> topicIds)
        {
            int lessonId = _lessonRepository.AddLesson(lessonDto);

            if (topicIds != null)
            {
                topicIds.ForEach(topicId => _lessonRepository.AddTopicToLesson(lessonId, topicId));
            }

            return lessonId;
        }

        public void DeleteCommentFromLesson(int lessonId, int commentId) => _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);

        public void DeleteLesson(int id) => _lessonRepository.DeleteLesson(id);

        public List<LessonDto> SelectAllLessonsByGroupId(int id) => _lessonRepository.SelectAllLessonsByGroupId(id);
        
        public List<LessonDto> SelectAllLessonsByTeacherId(int id) => _lessonRepository.SelectAllLessonsByTeacherId(id);
        
        public LessonDto SelectLessonById(int id) => _lessonRepository.SelectLessonById(id);
        
        public LessonDto SelectLessonWithCommentsById(int id)
        {
            LessonDto result = _lessonRepository.SelectLessonById(id);

            result.Comments = _commentRepository.SelectCommentsFromLessonByLessonId(id);

            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(int id)
        {
            LessonDto result = SelectLessonWithCommentsById(id);

            result.Students = _lessonRepository.SelectStudentsLessonByLessonId(id);

            return result;
        }

        public LessonDto UpdateLesson(LessonDto lessonDto, int id)
        {
            lessonDto.Id = id;
            _lessonRepository.UpdateLesson(lessonDto);
            return _lessonRepository.SelectLessonById(lessonDto.Id);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId) =>
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);

        public void AddTopicToLesson(int lessonId, int topicId) =>
            _lessonRepository.AddTopicToLesson(lessonId, topicId);

        public StudentLessonDto AddStudentToLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            if (!CheckerRole.IsAdmin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);

            _lessonRepository.AddStudentToLesson(lessonId, studentId);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public void DeleteStudentFromLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!CheckerRole.IsAdmin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            _lessonRepository.DeleteStudentFromLesson(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!CheckerRole.IsAdmin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentFeedbackForLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!CheckerRole.IsAdmin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            if (!CheckerRole.IsAdmin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId); 
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId, UserIdentityInfo userIdentityInfo)
        {
            var userId = userIdentityInfo.UserId;
            var roles = userIdentityInfo.Roles;
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            if (CheckerRole.IsStudent(roles) || CheckerRole.IsTeacher(roles))
            {
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            }
            return _lessonRepository.SelectAllFeedbackByLessonId(lessonId);
        }                


    }
}