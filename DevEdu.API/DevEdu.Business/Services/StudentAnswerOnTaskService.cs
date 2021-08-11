using System;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using DevEdu.Business.IdentityInfo;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Constants;

namespace DevEdu.Business.Services
{
    public class StudentAnswerOnTaskService : IStudentAnswerOnTaskService
    {
        private const string _dateFormat = "dd.MM.yyyy HH:mm:ss";

        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly IStudentAnswerOnTaskValidationHelper _studentAnswerOnTaskValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;

        public StudentAnswerOnTaskService(IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository, 
            IStudentAnswerOnTaskValidationHelper studentAnswerOnTaskValidationHelper,
            IUserValidationHelper userValidationHelper,
            ITaskValidationHelper taskValidationHelper)
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _studentAnswerOnTaskValidationHelper = studentAnswerOnTaskValidationHelper;
            _userValidationHelper = userValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            if (!userInfo.IsAdmin())
                _studentAnswerOnTaskValidationHelper.CheckUserInStudentAnswerAccess(studentId, userInfo.UserId);

            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            var studentAnswerId = _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);

            return studentAnswerId;
        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentAnswerOnTaskValidationHelper.GetStudentAnswerByTaskIdAndStudentIdOrThrowIfNotFound(taskId, studentId);
            _studentAnswerOnTaskValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);
            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(taskId, studentId);
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userInfo.UserId);
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);

            if (!userInfo.Roles.Contains(Role.Admin))
            {
                return _studentAnswerOnTaskValidationHelper.GetStudentAnswerOnTaskAllowedToUser(taskId, userInfo.UserId);
            }

            return _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask(taskId);
        }

        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentAnswerOnTaskValidationHelper.GetStudentAnswerByTaskIdAndStudentIdOrThrowIfNotFound(taskId, studentId);
            _studentAnswerOnTaskValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);
            return checkedStudentAnswerDto;
        }

        public int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId, UserIdentityInfo userInfo)
        {
            if (!userInfo.Roles.Contains(Role.Student) && !userInfo.Roles.Contains(Role.Manager))
            {
                _studentAnswerOnTaskValidationHelper.CheckStudentAnswerOnTaskExistence(taskId, studentId);

                DateTime completedDate = default;

                if (statusId == (int)TaskStatus.Accepted)
                    completedDate = DateTime.Now;

                completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day, completedDate.Hour, completedDate.Minute, completedDate.Second);
                var status = _studentAnswerOnTaskRepository.ChangeStatusOfStudentAnswerOnTask(taskId, studentId, statusId, completedDate);

                return status;
            }
            else
            {
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userInfo.UserId));
            }

        }

        public StudentAnswerOnTaskDto UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentAnswerOnTaskValidationHelper.GetStudentAnswerByTaskIdAndStudentIdOrThrowIfNotFound(taskId, studentId);
            _studentAnswerOnTaskValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);

            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(taskAnswerDto);

            return _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
        }

        public List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);

            var dto = _studentAnswerOnTaskRepository.GetAllAnswersByStudentId(userId);
            return dto;
        }
    }
}