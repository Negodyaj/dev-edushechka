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

        public StudentHomeworkDto AddStudentHomework(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            var homeworkDto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            if (!userInfo.IsAdmin())
                _studentHomeworkValidationHelper.CheckUserBelongsToHomeworkAsync(homeworkDto.Group.Id, userInfo.UserId);
            taskAnswerDto.Homework = new HomeworkDto { Id = homeworkId };
            taskAnswerDto.User = new UserDto { Id = userInfo.UserId };
            var id = _studentHomeworkRepository.AddStudentHomework(taskAnswerDto);
            return _studentHomeworkRepository.GetStudentHomeworkById(id);
        }

        public void DeleteStudentHomework(int id, UserIdentityInfo userInfo)
        {
            var dto = _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomework(dto.User.Id, userInfo.UserId);
            _studentHomeworkRepository.DeleteStudentHomework(id);
        }

        public StudentHomeworkDto UpdateStudentHomework(int id, StudentHomeworkDto updatedDto, UserIdentityInfo userInfo)
        {
            var dto = _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomework(dto.User.Id, userInfo.UserId);
            updatedDto.Id = id;
            _studentHomeworkRepository.UpdateStudentHomework(updatedDto);
            return _studentHomeworkRepository.GetStudentHomeworkById(id);
        }

        public int UpdateStatusOfStudentHomework(int id, int statusId, UserIdentityInfo userInfo)
        {
            var dto = _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(dto.User.Id, userInfo.UserId);
            DateTime completedDate = default;
            if (statusId == (int)StudentHomeworkStatus.Accepted)
                completedDate = DateTime.Now;
            completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day, completedDate.Hour, completedDate.Minute, completedDate.Second);
            return _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTask(id, statusId, completedDate);
        }

        public StudentHomeworkDto GetStudentHomeworkById(int id, UserIdentityInfo userInfo)
        {
            var dto = _studentHomeworkValidationHelper.GetStudentHomeworkByIdAndThrowIfNotFound(id);
            if (!userInfo.IsAdmin())
                _studentHomeworkValidationHelper.CheckUserInStudentHomeworkAccessAsync(dto.User.Id, userInfo.UserId);
            return dto;
        }

        public async Task<List<StudentHomeworkDto>> GetAllStudentHomeworkOnTaskAsync(int taskId)
        {
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            return _studentHomeworkRepository.GetAllStudentHomeworkByTask(taskId);
        }

        public List<StudentHomeworkDto> GetAllStudentHomeworkByStudentId(int userId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            if (userInfo.IsStudent())
                _studentHomeworkValidationHelper.CheckUserComplianceToStudentHomework(userId, userInfo.UserId);
            return _studentHomeworkRepository.GetAllStudentHomeworkByStudentId(userId);
        }
    }
}