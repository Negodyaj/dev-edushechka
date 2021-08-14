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
    public class StudentHomeworkService : IStudentHomeworkService
    {
        private readonly IStudentHomeworkRepository _studentHomeworkRepository;
        private readonly IStudentHomeworkValidationHelper _studentHomeworkValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;

        public StudentHomeworkService(IStudentHomeworkRepository studentHomeworkRepository, 
            IStudentHomeworkValidationHelper studentHomeworkValidationHelper,
            IUserValidationHelper userValidationHelper,
            ITaskValidationHelper taskValidationHelper)
        {
            _studentHomeworkRepository = studentHomeworkRepository;
            _studentHomeworkValidationHelper = studentHomeworkValidationHelper;
            _userValidationHelper = userValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public int AddStudentAnswerOnTask(int homeworkId, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userInfo.UserId);
            //if (!userInfo.IsAdmin())
            //    _studentHomeworkValidationHelper.CheckUserInStudentAnswerAccess(userInfo.UserId, userInfo.UserId);

            taskAnswerDto.Homework = new HomeworkDto { Id = homeworkId };
            taskAnswerDto.User = new UserDto { Id = userInfo.UserId };

            var studentAnswerId = _studentHomeworkRepository.AddStudentAnswerOnHomework(taskAnswerDto);

            return studentAnswerId;
        }

        public void DeleteStudentAnswerOnTask(int id, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentHomeworkValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(id);
            _studentHomeworkValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);
            _studentHomeworkRepository.DeleteStudentHomework(id);
        }

        public StudentHomeworkDto UpdateStudentAnswerOnTask(int id, StudentHomeworkDto taskAnswerDto, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentHomeworkValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(id);
            _studentHomeworkValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);

            taskAnswerDto.Id = id;
            _studentHomeworkRepository.UpdateStudentAnswerOnTask(taskAnswerDto);

            return _studentHomeworkRepository.GetStudentAnswerOnTaskById(id);
        }

        public int ChangeStatusOfStudentAnswerOnTask(int id, int statusId, UserIdentityInfo userInfo)
        {
            if (!userInfo.Roles.Contains(Role.Student) && !userInfo.Roles.Contains(Role.Manager))
            {
                _studentHomeworkValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(id);

                DateTime completedDate = default;

                if (statusId == (int)TaskStatus.Accepted)
                    completedDate = DateTime.Now;

                completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day, completedDate.Hour, completedDate.Minute, completedDate.Second);
                var status = _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTask(id, statusId, completedDate);

                return status;
            }

            if (userInfo.IsAdmin())
            {
                _studentHomeworkValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(id);

                DateTime completedDate = default;

                if (statusId == (int) TaskStatus.Accepted)
                    completedDate = DateTime.Now;

                completedDate = new DateTime(completedDate.Year, completedDate.Month, completedDate.Day,
                    completedDate.Hour, completedDate.Minute, completedDate.Second);
                var status = _studentHomeworkRepository.ChangeStatusOfStudentAnswerOnTask(id, statusId, completedDate);

                return status;
            }

            throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userInfo.UserId));
        }
        public StudentHomeworkDto GetStudentHomeworkId(int id, UserIdentityInfo userInfo)
        {
            var checkedStudentAnswerDto = _studentHomeworkValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(id);
            _studentHomeworkValidationHelper.CheckUserAccessToStudentAnswerByUserId(userInfo, checkedStudentAnswerDto);
            return checkedStudentAnswerDto;
        }

        public List<StudentHomeworkDto> GetAllStudentAnswersOnTask(int taskId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userInfo.UserId);
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);

            if (!userInfo.Roles.Contains(Role.Admin))
            {
                return _studentHomeworkValidationHelper.GetStudentAnswerOnTaskAllowedToUser(taskId, userInfo.UserId);
            }

            return _studentHomeworkRepository.GetAllStudentAnswersOnTask(taskId);
        }

        public List<StudentHomeworkDto> GetAllAnswersByStudentId(int userId, UserIdentityInfo userInfo)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);

            var dto = _studentHomeworkRepository.GetAllAnswersByStudentId(userId);
            return dto;
        }
    }
}