using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ILessonValidationHelper _lessonValidationHelper;
        private readonly ITopicValidationHelper _topicValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;

        public LessonService(
            ILessonRepository lessonRepository,
            ICommentRepository commentRepository,
            IUserValidationHelper userValidationHelper,
            ILessonValidationHelper lessonValidationHelper,
            ITopicValidationHelper topicValidationHelper,
            IGroupValidationHelper groupValidationHelper
        )
        {
            _lessonRepository = lessonRepository;
            _commentRepository = commentRepository;
            _userValidationHelper = userValidationHelper;
            _lessonValidationHelper = lessonValidationHelper;
            _topicValidationHelper = topicValidationHelper;
            _groupValidationHelper = groupValidationHelper;
        }

        public LessonDto AddLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds)
        {
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserAndTeacherAreSame(userIdentity, lessonDto.Teacher.Id);
            }
            int lessonId = _lessonRepository.AddLesson(lessonDto);

            if (topicIds != null)
            {
                foreach (int topicId in topicIds)
                {
                    _topicValidationHelper.CheckTopicExistence(topicId);
                    _lessonRepository.AddTopicToLesson(lessonId, topicId);
                }
            }
            return _lessonRepository.SelectLessonById(lessonId);
        }

        public void DeleteLesson(UserIdentityInfo userIdentity, int id)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(id);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLesson(userIdentity, lesson);
            }
            _lessonRepository.DeleteLesson(id);
        }

        public List<LessonDto> SelectAllLessonsByGroupId(UserIdentityInfo userIdentity, int groupId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            if (!userIdentity.IsAdmin())
            {
                _userValidationHelper.CheckUserBelongToGroup(groupId, userIdentity.UserId, userIdentity.Roles);
            }
            var result = _lessonRepository.SelectAllLessonsByGroupId(groupId);
            return result;
        }

        public List<LessonDto> SelectAllLessonsByTeacherId(int teacherId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(teacherId);
            return _lessonRepository.SelectAllLessonsByTeacherId(teacherId);
        }
               
        public LessonDto SelectLessonWithCommentsById(UserIdentityInfo userIdentity, int id)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(id);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLesson(userIdentity, lesson);
            }

            LessonDto result = _lessonRepository.SelectLessonById(id);
            result.Comments = _commentRepository.SelectCommentsFromLessonByLessonId(id);
            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(UserIdentityInfo userIdentity, int id)
        {
            LessonDto result = SelectLessonWithCommentsById(userIdentity, id);
            result.Students = _lessonRepository.SelectStudentsLessonByLessonId(id);
            return result;
        }

        public LessonDto UpdateLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, int lessonId)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLesson(userIdentity, lesson);
            }

            lessonDto.Id = lessonId;
            _lessonRepository.UpdateLesson(lessonDto);
            return _lessonRepository.SelectLessonById(lessonDto.Id);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _topicValidationHelper.CheckTopicExistence(topicId);
            if(_lessonRepository.DeleteTopicFromLesson(lessonId, topicId) == 0)
            {
                throw new ValidationException(string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId));
            }
        }

        public void AddTopicToLesson(int lessonId, int topicId)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _topicValidationHelper.CheckTopicExistence(topicId);
            _lessonValidationHelper.CheckTopicLessonReferenceIsUnique(lesson, topicId);
            _lessonRepository.AddTopicToLesson(lessonId, topicId);
        }

        public StudentLessonDto AddStudentToLesson(int lessonId, int userId)
        {
            _lessonRepository.AddStudentToLesson(lessonId, userId);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, userId);
        }

        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            _lessonRepository.DeleteStudentFromLesson(lessonId, userId);
        }

        public StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);

            // check if user relates to lesson
            /*
            I.
                var studentLesson = _lessonRepository.GetStudentLessonByStudentAndLesson(userId, lessonId);
                if (studentLesson == default)
                    throw new AuthorizationException($"user with id = {userId} doesn't relate to lesson with id = {lessonId}");
            II.
                var groupsInLesson = _groupRepository.GetGroupsByLessonId(lessonId);
                var studentGroups = _groupRepository.GetGroupsByStudentId(userId);
                var result = groupsInLesson.Where(gl => studentGroups.Any(gs => gs.Id == gl.Id));
                if (result == default)
                    throw new AuthorizationException($"user with id = {userId} doesn't relate to lesson with id = {lessonId}");
            */

            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentFeedbackForLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, userId);
        }

        public StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, userId);
        }

        public StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, userId);
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId) =>
            _lessonRepository.SelectAllFeedbackByLessonId(lessonId);
    }
}