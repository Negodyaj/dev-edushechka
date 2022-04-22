using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class StudentHomeworkService : IStudentHomeworkService
    {
        private readonly IStudentHomeworkRepository _studentHomeworkRepository;
        private readonly IStudentHomeworkValidationHelper _studentHomeworkValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IHomeworkValidationHelper _homeworkValidationHelper;

        public StudentHomeworkService
        (
            IStudentHomeworkRepository studentHomeworkRepository,
            IStudentHomeworkValidationHelper studentHomeworkValidationHelper,
            IUserValidationHelper userValidationHelper,
            ITaskValidationHelper taskValidationHelper,
            IHomeworkValidationHelper homeworkValidationHelper
        )
        {
            _studentHomeworkRepository = studentHomeworkRepository;
            _studentHomeworkValidationHelper = studentHomeworkValidationHelper;
            _userValidationHelper = userValidationHelper;
            _taskValidationHelper = taskValidationHelper;
            _homeworkValidationHelper = homeworkValidationHelper;
        }

        public async Task<StudentHomeworkDto> AddStudentHomeworkAsync(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            var homeworkDto = await _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFoundAsync(homeworkId);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserBelongsToHomeworkAsync(homeworkDto.Group.Id, userInfo.UserId);

            taskAnswerDto.Homework = new HomeworkDto { Id = homeworkId };
            taskAnswerDto.User = new UserDto { Id = userInfo.UserId };
            var id = await _studentHomeworkRepository.AddStudentHomeworkAsync(taskAnswerDto);
            var studentHomeworkDto = await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);

            return studentHomeworkDto;
        }

        public async Task DeleteStudentHomeworkAsync(int id, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(dto.User.Id, userInfo.UserId);

            await _studentHomeworkRepository.DeleteStudentHomeworkAsync(id);
        }

        public async Task<StudentHomeworkDto> UpdateStudentHomeworkAsync(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(dto.User.Id, userInfo.UserId);

            updatedDto.Id = id;
            updatedDto.Status = dto.Status;

            if (dto.Status == StudentHomeworkStatus.ToFix)
                updatedDto.Status = StudentHomeworkStatus.ToVerifyFixes;

            _studentHomeworkValidationHelper.CheckUserCanChangeStatus(userInfo, dto, updatedDto.Status);

            await _studentHomeworkRepository.UpdateStudentHomeworkAsync(updatedDto);
            var studentHomeworkDto = await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);

            return studentHomeworkDto;
        }

        public async Task<StudentHomeworkStatus> UpdateStatusOfStudentHomeworkAsync(int id, StudentHomeworkStatus status, UserIdentityInfo userInfo)
        {
            var studentHomeworkDto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(studentHomeworkDto.User.Id, userInfo.UserId);

            _studentHomeworkValidationHelper.CheckUserCanChangeStatus(userInfo, studentHomeworkDto, status);

            DateTime completedDate = default;
            DateTime answerDate = default;

            if (status == StudentHomeworkStatus.Done)
            {
                completedDate = DateTime.Now;
                if (studentHomeworkDto.Homework.EndDate < studentHomeworkDto.AnswerDate)
                    status = StudentHomeworkStatus.DoneAfterDeadline;
            }

            if (status == StudentHomeworkStatus.ToCheck || status == StudentHomeworkStatus.ToVerifyFixes)
                answerDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


            completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day,
                completedDate.Hour, completedDate.Minute, completedDate.Second);

            var result = await _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTaskAsync(id, (int)status, completedDate);

            return (StudentHomeworkStatus)result;
        }

        public async Task<StudentHomeworkStatus> ApproveOrDeclineStudentHomework(int id, bool isApproved, UserIdentityInfo userInfo)
        {
            var studentHomeworkDto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(studentHomeworkDto.User.Id, userInfo.UserId);

            StudentHomeworkStatus newStatus;
            DateTime completedDate = default;
            if (isApproved)
            {
                newStatus = studentHomeworkDto.AnswerDate < studentHomeworkDto.Homework.EndDate ? 
                    StudentHomeworkStatus.Done : StudentHomeworkStatus.DoneAfterDeadline;
                var now = DateTime.Now;
                completedDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            }
            else
            {
                newStatus = StudentHomeworkStatus.ToFix;
            }

            _studentHomeworkValidationHelper.CheckUserCanChangeStatus(userInfo, studentHomeworkDto, newStatus);

            var result = await _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTaskAsync(id, (int)newStatus, completedDate);
            return (StudentHomeworkStatus)result;
        }

        public async Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFoundAsync(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(dto.User.Id, userInfo.UserId);

            return dto;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkOnTaskAsync(int taskId)
        {
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            var studentHomeworkDto = await _studentHomeworkRepository.GetAllStudentHomeworkByTaskAsync(taskId);

            return studentHomeworkDto;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId, UserIdentityInfo userInfo)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            if (userInfo.IsStudent())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(userId, userInfo.UserId);

            var listStudentHomeworkDto = await _studentHomeworkRepository.GetAllStudentHomeworkByStudentIdAsync(userId);

            return listStudentHomeworkDto;
        }
    }
}