using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IHomeworkValidationHelper _homeworkValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;

        public HomeworkService
        (
            IHomeworkRepository homeworkRepository,
            IHomeworkValidationHelper homeworkValidationHelper,
            IGroupValidationHelper groupValidationHelper,
            ITaskValidationHelper taskValidationHelper
        )
        {
            _homeworkRepository = homeworkRepository;
            _homeworkValidationHelper = homeworkValidationHelper;
            _groupValidationHelper = groupValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public HomeworkDto GetHomework(int homeworkId, int userId)
        {
            var dto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userId, dto);
            return dto;
        }

        public List<HomeworkDto> GetHomeworkByGroupId(int groupId, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
            return _homeworkRepository.GetHomeworkByGroupId(groupId);
        }

        public List<HomeworkDto> GetHomeworkByTaskId(int taskId)
        {
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            return _homeworkRepository.GetHomeworkByTaskId(taskId);
        }

        public HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            var id = _homeworkRepository.AddHomework(dto);
            return _homeworkRepository.GetHomework(id);
        }

        public void DeleteHomework(int homeworkId, int userId)
        {
            var dto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userId, dto);

            _homeworkRepository.DeleteHomework(homeworkId);
        }

        public HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto, int userId)
        {
            _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userId, dto);

            dto.Id = homeworkId;
            _homeworkRepository.UpdateHomework(dto);
            return _homeworkRepository.GetHomework(homeworkId);
        }

        private void CheckAccessAndExistenceAndThrowIfNotFound(int userId, HomeworkDto dto)
        {
            var groupId = dto.Group.Id;
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
        }
    }
}