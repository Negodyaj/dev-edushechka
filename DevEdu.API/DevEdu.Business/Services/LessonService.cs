using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            int lessonId = _lessonRepository.AddLessonAsync(lessonDto);

            if (topicIds != null)
            {
                foreach (int topicId in topicIds)
                {
                    _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
                    _lessonRepository.AddTopicToLessonAsync(lessonId, topicId);
                }
            }
            return _lessonRepository.SelectLessonByIdAsync(lessonId);
        }

        public void DeleteLesson(UserIdentityInfo userIdentity, int id)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(id);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, lesson);
            }
            _lessonRepository.DeleteLessonAsync(id);
        }

        public async Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(UserIdentityInfo userIdentity, int groupId)
        {
            var groupDto = Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(groupId)).GetAwaiter().GetResult();
            if (!userIdentity.IsAdmin())
            {
                var currentRole = userIdentity.IsTeacher() ? Role.Teacher : Role.Student;
                await _userValidationHelper.CheckAuthorizationUserToGroup(groupId, userIdentity.UserId, currentRole);
            }
            var result = await _lessonRepository.SelectAllLessonsByGroupIdAsync(groupId);
            return result;
        }

        public List<LessonDto> SelectAllLessonsByTeacherId(int teacherId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(teacherId);
            return _lessonRepository.SelectAllLessonsByTeacherIdAsync(teacherId);
        }

        public LessonDto SelectLessonWithCommentsById(UserIdentityInfo userIdentity, int id)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(id);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, lesson);
            }

            LessonDto result = _lessonRepository.SelectLessonByIdAsync(id);
            result.Comments = _commentRepository.SelectCommentsFromLessonByLessonIdAsync(id);
            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(UserIdentityInfo userIdentity, int id)
        {
            LessonDto result = SelectLessonWithCommentsById(userIdentity, id);
            result.Students = _lessonRepository.SelectStudentsLessonByLessonIdAsync(id).Result;
            return result;
        }

        public LessonDto UpdateLesson(UserIdentityInfo userIdentity, LessonDto lessonDto, int lessonId)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, lesson);
            }

            lessonDto.Id = lessonId;
            _lessonRepository.UpdateLessonAsync(lessonDto);
            return _lessonRepository.SelectLessonByIdAsync(lessonDto.Id);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            if (_lessonRepository.DeleteTopicFromLessonAsync(lessonId, topicId).Result == 0)
            {
                throw new ValidationException(nameof(topicId), string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId));
            }
        }

        public void AddTopicToLesson(int lessonId, int topicId)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            _lessonValidationHelper.CheckTopicLessonReferenceIsUnique(lesson, topicId);
            _lessonRepository.AddTopicToLessonAsync(lessonId, topicId);
        }

        public StudentLessonDto AddStudentToLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            _lessonRepository.AddStudentToLessonAsync(lessonId, studentId);
            return _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public void DeleteStudentFromLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {

            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);
            _lessonRepository.DeleteStudentFromLessonAsync(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentFeedbackForLessonAsync(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLessonAsync(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAttendanceOnLessonAsync(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId, UserIdentityInfo userIdentityInfo)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (userIdentityInfo.IsStudent() || userIdentityInfo.IsTeacher())
                _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);
            return _lessonRepository.SelectAllFeedbackByLessonIdAsync(lessonId);
        }


    }
}