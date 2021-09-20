using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskStatus = DevEdu.DAL.Enums.StudentHomeworkStatus;
using System.Threading.Tasks;
using System;

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
            var homeworkDto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserBelongsToHomeworkAsync(homeworkDto.Group.Id, userInfo.UserId);
            taskAnswerDto.Homework = new HomeworkDto { Id = homeworkId };
            taskAnswerDto.User = new UserDto { Id = userInfo.UserId };
            var id = await _studentHomeworkRepository.AddStudentHomeworkAsync(taskAnswerDto);
            return await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);
        }

        public async Task DeleteStudentHomeworkAsync(int id, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(dto.User.Id, userInfo.UserId);
            await _studentHomeworkRepository.DeleteStudentHomeworkAsync(id);
        }

        public async Task<StudentHomeworkDto> UpdateStudentHomeworkAsync(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(dto.User.Id, userInfo.UserId);
            updatedDto.Id = id;
            await _studentHomeworkRepository.UpdateStudentHomeworkAsync(updatedDto);
            return  await _studentHomeworkRepository.GetStudentHomeworkByIdAsync(id);
        }

        public async Task<int> UpdateStatusOfStudentHomeworkAsync(int id, int statusId, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(dto.User.Id, userInfo.UserId);
            DateTime completedDate = default;
            if (statusId == (int)StudentHomeworkStatus.Accepted)
                completedDate = DateTime.Now;
            completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day, completedDate.Hour, completedDate.Minute, completedDate.Second);
            return await _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTaskAsync(id, statusId, completedDate);
        }

        public async Task<StudentHomeworkDto> GetStudentHomeworkByIdAsync(int id, UserIdentityInfo userInfo)
        {
            var dto = await _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                await _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(dto.User.Id, userInfo.UserId);
            return dto;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkOnTaskAsync(int taskId)
        {
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            return await _studentHomeworkRepository.GetAllStudentHomeworkByTaskAsync(taskId);
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkByStudentIdAsync(int userId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            if (userInfo.IsStudent())
                await _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomeworkAsync(userId, userInfo.UserId);
            return await _studentHomeworkRepository.GetAllStudentHomeworkByStudentIdAsync(userId);
        }
    }
}