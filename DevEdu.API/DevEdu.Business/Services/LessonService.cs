using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ILessonValidationHelper _lessonValidationHelper;
        private readonly ITopicValidationHelper _topicValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;

        public LessonService(
            IUserRepository userRepository,
            ILessonRepository lessonRepository,
            ICommentRepository commentRepository,
            IUserValidationHelper userValidationHelper,
            ILessonValidationHelper lessonValidationHelper,
            ITopicValidationHelper topicValidationHelper,
            IGroupValidationHelper groupValidationHelper
        )
        {
            _userRepository = userRepository;
            _lessonRepository = lessonRepository;
            _commentRepository = commentRepository;
            _userValidationHelper = userValidationHelper;
            _lessonValidationHelper = lessonValidationHelper;
            _topicValidationHelper = topicValidationHelper;
            _groupValidationHelper = groupValidationHelper;
        }

        public async Task<LessonDto> AddLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds)
        {
            if (!userIdentity.IsAdmin())
            {
                _lessonValidationHelper.CheckUserAndTeacherAreSame(userIdentity, lessonDto.Teacher.Id);
            }

            int lessonId = await _lessonRepository.AddLessonAsync(lessonDto);

            foreach (GroupDto group in lessonDto.Groups)
            {
                await _groupValidationHelper.CheckGroupExistenceAsync(group.Id);
                foreach (UserDto user in group.Students)
                {
                    await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(user.Id);
                    await _lessonRepository.AddStudentToLessonAsync(lessonId, user.Id);
                }
            }

            if (topicIds != null)
            {
                foreach (int topicId in topicIds)
                {
                    await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topicId);
                    await _lessonRepository.AddTopicToLessonAsync(lessonId, topicId);
                }
            }
            return await _lessonRepository.SelectLessonByIdAsync(lessonId);
        }

        public async Task DeleteLessonAsync(UserIdentityInfo userIdentity, int id)
        {
            var lesson = await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(id);
            if (!userIdentity.IsAdmin())
            {
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, lesson);
            }
            await _lessonRepository.DeleteLessonAsync(id);
        }

        public async Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(UserIdentityInfo userIdentity, int groupId)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);
            if (!userIdentity.IsAdmin())
            {
                var currentRole = userIdentity.IsTeacher() ? Role.Teacher : Role.Student;
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(groupId, userIdentity.UserId, currentRole);
            }

            var result = await _lessonRepository.SelectAllLessonsByGroupIdAsync(groupId);

            return result;
        }

        public async Task<List<LessonDto>> SelectAllLessonsByTeacherIdAsync(int teacherId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(teacherId);
            return await _lessonRepository.SelectAllLessonsByTeacherIdAsync(teacherId);
        }

        public async Task<LessonDto> SelectLessonWithCommentsByIdAsync(UserIdentityInfo userIdentity, int id)
        {
            var lesson = await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(id);
            if (!userIdentity.IsAdmin())
            {
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, lesson);
            }

            LessonDto result = await _lessonRepository.SelectLessonByIdAsync(id);
            result.Comments = await _commentRepository.SelectCommentsFromLessonByLessonIdAsync(id);
            return result;
        }

        public async Task<LessonDto> SelectLessonWithCommentsAndStudentsByIdAsync(UserIdentityInfo userIdentity, int id)
        {
            LessonDto result = await SelectLessonWithCommentsByIdAsync(userIdentity, id);
            result.Students = await _lessonRepository.SelectStudentsLessonByLessonIdAsync(id);//.Result;
            return result;
        }

        public async Task<LessonDto> UpdateLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, int lessonId)
        {
            var existingLesson = await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            if (!userIdentity.IsAdmin())
            {
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(userIdentity, existingLesson);
            }

            //проверка существования Topics введённых в lessonDto
            foreach (var topic in lessonDto.Topics)
                await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topic.Id);

            //удаление неиспользуемых
            foreach (var idTopicToDelete in existingLesson.Topics.Select(t => t.Id).Except(lessonDto.Topics.Select(t => t.Id)))
                if (await _lessonRepository.DeleteTopicFromLessonAsync(lessonId, idTopicToDelete) == 0)
                    throw new ValidationException(nameof(idTopicToDelete), string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, idTopicToDelete));

            //добавление недостающих
            foreach (var idTopicToAdd in lessonDto.Topics.Select(t => t.Id).Except(existingLesson.Topics.Select(t => t.Id)))
                await _lessonRepository.AddTopicToLessonAsync(lessonId, idTopicToAdd);

            lessonDto.Id = lessonId;
            await _lessonRepository.UpdateLessonAsync(lessonDto);
            return await _lessonRepository.SelectLessonByIdAsync(lessonDto.Id);
        }

        public async Task DeleteTopicFromLessonAsync(int lessonId, int topicId)
        {
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topicId);
            if (await _lessonRepository.DeleteTopicFromLessonAsync(lessonId, topicId) == 0)
            {
                throw new ValidationException(nameof(topicId), string.Format(ServiceMessages.LessonTopicReferenceNotFound, lessonId, topicId));
            }
        }

        public async Task AddTopicToLessonAsync(int lessonId, int topicId)
        {
            var lesson = await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topicId);
            _lessonValidationHelper.CheckTopicLessonReferenceIsUnique(lesson, topicId);
            await _lessonRepository.AddTopicToLessonAsync(lessonId, topicId);
        }

        public async Task<StudentLessonDto> AddStudentToLessonAsync(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentId);
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            if (!userIdentityInfo.IsAdmin())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            await _lessonRepository.AddStudentToLessonAsync(lessonId, studentId);
            return await _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public async Task DeleteStudentFromLessonAsync(int lessonId, int studentId, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentId);
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            await _lessonRepository.DeleteStudentFromLessonAsync(lessonId, studentId);
        }

        public async Task<StudentLessonDto> UpdateStudentFeedbackForLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentId);
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            await _lessonRepository.UpdateStudentFeedbackForLessonAsync(studentLessonDto);
            return await _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public async Task<StudentLessonDto> UpdateStudentAbsenceReasonOnLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            if (!userIdentityInfo.IsAdmin())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            await _lessonRepository.UpdateStudentAbsenceReasonOnLessonAsync(studentLessonDto);
            return await _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public async Task<StudentLessonDto> UpdateStudentAttendanceOnLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo)
        {
            if (!userIdentityInfo.IsAdmin())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(studentId);
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            _lessonValidationHelper.CheckAttendanceExistence(lessonId, studentId);
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.Student = new UserDto { Id = studentId };
            await _lessonRepository.UpdateStudentAttendanceOnLessonAsync(studentLessonDto);
            return await _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, studentId);
        }

        public async Task<List<StudentLessonDto>> SelectAllFeedbackByLessonIdAsync(int lessonId, UserIdentityInfo userIdentityInfo)
        {
            await _lessonValidationHelper.GetLessonByIdAndThrowIfNotFoundAsync(lessonId);
            if (userIdentityInfo.IsStudent() || userIdentityInfo.IsTeacher())
                await _lessonValidationHelper.CheckUserBelongsToLessonAsync(lessonId, userIdentityInfo.UserId);

            return await _lessonRepository.SelectAllFeedbackByLessonIdAsync(lessonId);
        }
    }
}