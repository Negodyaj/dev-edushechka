using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;

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
                    _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
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
            var groupDto = Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(groupId)).GetAwaiter().GetResult();
            if (!userIdentity.IsAdmin())
            {
                var currentRole = userIdentity.IsTeacher() ? Role.Teacher : Role.Student;
                _userValidationHelper.CheckAuthorizationUserToGroup(groupId, userIdentity.UserId, currentRole);
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
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            if(_lessonRepository.DeleteTopicFromLesson(lessonId, topicId) == 0)
            {
                throw new ValidationException(nameof(topicId), string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId));
            }
        }

        public void AddTopicToLesson(int lessonId, int topicId)
        {
            var lesson = _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            _lessonValidationHelper.CheckTopicLessonReferenceIsUnique(lesson, topicId);
            _lessonRepository.AddTopicToLesson(lessonId, topicId);
        }

        public StudentLessonDto AddStudentToLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {           
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);

            _lessonRepository.AddStudentToLesson(lessonId, studentId);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public void DeleteStudentFromLesson(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
           
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);
            _lessonRepository.DeleteStudentFromLesson(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {            
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentFeedbackForLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {            
            if (!userIdentityInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId); 
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
            return _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, studentId);
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId, UserIdentityInfo userIdentityInfo)
        {            
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (userIdentityInfo.IsStudent() || userIdentityInfo.IsTeacher())          
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userIdentityInfo.UserId);            
            return _lessonRepository.SelectAllFeedbackByLessonId(lessonId);
        }                


    }
}