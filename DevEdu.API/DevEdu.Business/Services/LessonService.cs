﻿using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using DevEdu.Business.ValidationHelpers;

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

        public void AddCommentToLesson(int lessonId, CommentDto commentDto)
        {
            int commentId =_commentRepository.AddComment(commentDto);

            _lessonRepository.AddCommentToLesson(lessonId, commentId);
        }

        public LessonDto AddLesson(UserDto userIdentity, LessonDto lessonDto, List<int> topicIds)
        {
            _lessonValidationHelper.CheckUserAndTeacherAreSame(userIdentity, lessonDto.Teacher.Id);

            int lessonId = _lessonRepository.AddLesson(lessonDto);

            if(topicIds != null)
            {
                foreach (int topicId in topicIds)
                {
                    _topicValidationHelper.CheckTopicExistence(topicId);
                    _lessonRepository.AddTopicToLesson(lessonId, topicId);
                }
            }
            return _lessonRepository.SelectLessonById(lessonId);
        }

        public void DeleteCommentFromLesson(int lessonId, int commentId) => _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);

        public void DeleteLesson(UserDto userIdentity, int id)
        {
            _lessonValidationHelper.CheckLessonExistence(id);
            _lessonValidationHelper.CheckUserRelatesToLesson(userIdentity, id);

            _lessonRepository.DeleteLesson(id);
        }

        public List<LessonDto> SelectAllLessonsByGroupId(UserDto userIdentity, int groupId)
        {
            //_groupValidationHelper.CheckGroupExistence(groupId);
            var result = _lessonRepository.SelectAllLessonsByGroupId(groupId);
            _lessonValidationHelper.CheckUserRelatesToGroup(userIdentity, result, groupId);
            return result;
        }

        public List<LessonDto> SelectAllLessonsByTeacherId(int teacherId)
        {
            _lessonValidationHelper.CheckTeacherExistence(teacherId);
            return _lessonRepository.SelectAllLessonsByTeacherId(teacherId);
        }
               
        public LessonDto SelectLessonWithCommentsById(UserDto userIdentity, int id)
        {
            _lessonValidationHelper.CheckLessonExistence(id);
            _lessonValidationHelper.CheckUserRelatesToLesson(userIdentity, id);

            LessonDto result = _lessonRepository.SelectLessonById(id);
            result.Comments = _commentRepository.SelectCommentsFromLessonByLessonId(id);
            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(UserDto userIdentity, int id)
        {
            LessonDto result = SelectLessonWithCommentsById(userIdentity, id);
            result.Students = _lessonRepository.SelectStudentsLessonByLessonId(id);
            return result;
        }

        public LessonDto UpdateLesson(UserDto userIdentity, LessonDto lessonDto, int lessonId)
        {
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            _lessonValidationHelper.CheckUserRelatesToLesson(userIdentity, lessonId);

            lessonDto.Id = lessonId;
            _lessonRepository.UpdateLesson(lessonDto);
            return _lessonRepository.SelectLessonById(lessonDto.Id);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId) => 
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);

        public void AddTopicToLesson(int lessonId, int topicId) =>
            _lessonRepository.AddTopicToLesson(lessonId, topicId);

        public void AddStudentToLesson(int lessonId, int userId)
        {
            var studentLessonDto =
               new StudentLessonDto
               {
                   User = new UserDto { Id = userId },
                   Lesson = new LessonDto { Id = lessonId }
               };
            _lessonRepository.AddStudentToLesson(studentLessonDto);
        }

        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            var studentLessonDto =
                new StudentLessonDto
                {
                    User = new UserDto { Id = userId },
                    Lesson = new LessonDto { Id = lessonId }
                };
            _lessonRepository.DeleteStudentFromLesson(studentLessonDto);
        }

        public void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            _userValidationHelper.CheckUserExistence(userId);
            _lessonValidationHelper.CheckLessonExistence(lessonId);

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
        }

        public void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
        }

        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId)=>
            _lessonRepository.SelectAllFeedbackByLessonId(lessonId);

        public StudentLessonDto GetStudenLessonByLessonAndUserId(int lessonId, int userId) =>
            _lessonRepository.SelectByLessonAndUserId(lessonId, userId);
    }
}