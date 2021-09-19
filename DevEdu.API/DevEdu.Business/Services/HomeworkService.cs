using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public HomeworkDto GetHomework(int homeworkId, UserIdentityInfo userInfo)
        {
            var dto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userInfo, dto);
            return dto;
        }

        public async Task<List<HomeworkDto>> GetHomeworkByGroupIdAsync(int groupId, UserIdentityInfo userInfo)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);
            if (!userInfo.IsAdmin())
                _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);
            return _homeworkRepository.GetHomeworkByGroupIdAsync(groupId);
        }

        public List<HomeworkDto> GetHomeworkByTaskId(int taskId)
        {
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            return _homeworkRepository.GetHomeworkByTaskIdAsync(taskId);
        }

        public HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto, UserIdentityInfo userInfo)
        {
            Task.Run(() => _groupValidationHelper.CheckGroupExistenceAsync(groupId)).GetAwaiter().GetResult();
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            if (!userInfo.IsAdmin())
                _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);
            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            var id = _homeworkRepository.AddHomeworkAsync(dto);
            return _homeworkRepository.GetHomeworkAsync(id);
        }

        public void DeleteHomework(int homeworkId, UserIdentityInfo userInfo)
        {
            var dto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userInfo, dto);

            _homeworkRepository.DeleteHomeworkAsync(homeworkId);
        }

        public HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto, UserIdentityInfo userInfo)
        {
            var homeworkDto = _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFound(homeworkId);
            CheckAccessAndExistenceAndThrowIfNotFound(userInfo, homeworkDto);

            homeworkDto.Id = homeworkId;
            homeworkDto.StartDate = dto.StartDate;
            homeworkDto.EndDate = dto.EndDate;
            _homeworkRepository.UpdateHomeworkAsync(homeworkDto);
            return _homeworkRepository.GetHomeworkAsync(homeworkId);
        }

        private void CheckAccessAndExistenceAndThrowIfNotFound(UserIdentityInfo userInfo, HomeworkDto dto)
        {
            if (userInfo.IsAdmin()) { return; }
            var groupId = dto.Group.Id;
            _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);
        }
    }
}