using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

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
            return CheckAccessAndExistence(homeworkId, userId);
        }

        public List<HomeworkDto> GetHomeworkByGroupId(int groupId, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
            return _homeworkRepository.GetHomeworkByGroupId(groupId);
        }

        public List<HomeworkDto> GetHomeworkByTaskId(int taskId, int userId)
        {
            _taskValidationHelper.CheckTaskExistence(taskId);
            return _homeworkRepository.GetHomeworkByTaskId(taskId);
        }

        public HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto, int userId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            _taskValidationHelper.CheckTaskExistence(taskId);
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            var id = _homeworkRepository.AddHomework(dto);
            return _homeworkRepository.GetHomework(id);
        }

        public void DeleteHomework(int homeworkId, int userId)
        {
            CheckAccessAndExistence(homeworkId, userId);

            _homeworkRepository.DeleteHomework(homeworkId);
        }

        public HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto, int userId)
        {
            CheckAccessAndExistence(homeworkId, userId);

            dto.Id = homeworkId;
            _homeworkRepository.UpdateHomework(dto);
            return _homeworkRepository.GetHomework(homeworkId);
        }

        private HomeworkDto CheckAccessAndExistence(int homeworkId, int userId)
        {
            var dtoChecked = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            var groupId = dtoChecked.Group.Id;
            _groupValidationHelper.CheckUserInGroupExistence(groupId, userId);
            return dtoChecked;
        }
    }
}