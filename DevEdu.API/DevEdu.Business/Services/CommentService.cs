﻿using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidationHelper _commentValidationHelper;
        private readonly IStudentHomeworkValidationHelper _studentAnswerValidationHelper;

        public CommentService
        (
            ICommentRepository commentRepository,
            ICommentValidationHelper commentValidationHelper,
            ILessonValidationHelper lessonValidationHelper,
            IStudentHomeworkValidationHelper studentAnswerValidationHelper
        )
        {
            _commentRepository = commentRepository;
            _commentValidationHelper = commentValidationHelper;
            _studentAnswerValidationHelper = studentAnswerValidationHelper;
        }

        public async Task<CommentDto> AddCommentToStudentAnswerAsync(int studentHomeworkId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var studentAnswer = await _studentAnswerValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(studentHomeworkId);
            var studentId = studentAnswer.User.Id;
            if (!userInfo.IsAdmin())
                await _studentAnswerValidationHelper.CheckUserInStudentHomeworkAccessAsync(studentId, userInfo.UserId);

            dto.User = new UserDto { Id = userInfo.UserId };
            dto.StudentHomework = new StudentHomeworkDto { Id = studentHomeworkId };
            var id = await _commentRepository.AddCommentAsync(dto);
            return await _commentRepository.GetCommentAsync(id);
        }

        public async Task<CommentDto> GetCommentAsync(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = await _commentValidationHelper.GetCommentByIdAndThrowIfNotFoundAsync(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            return checkedDto;
        }

        public async Task DeleteCommentAsync(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = await _commentValidationHelper.GetCommentByIdAndThrowIfNotFoundAsync(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            await _commentRepository.DeleteCommentAsync(commentId);
        }

        public async Task<CommentDto> UpdateCommentAsync(int commentId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var checkedDto = await _commentValidationHelper.GetCommentByIdAndThrowIfNotFoundAsync(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            dto.Id = commentId;
            await _commentRepository.UpdateCommentAsync(dto);
            return await _commentRepository.GetCommentAsync(commentId);
        }

        private void CheckUserAccessToCommentByUserId(UserIdentityInfo userInfo, CommentDto dto)
        {
            var userId = userInfo.UserId;

            if (userInfo.IsAdmin())
            {
                return;
            }
            _commentValidationHelper.UserComplianceCheck(dto, userId);
        }
    }
}