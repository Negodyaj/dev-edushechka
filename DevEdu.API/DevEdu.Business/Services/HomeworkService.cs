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

        public async Task<HomeworkDto> GetHomeworkAsync(int homeworkId, UserIdentityInfo userInfo)
        {
            var dto = await _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFoundAsync(homeworkId);
            await CheckAccessAndExistenceAndThrowIfNotFoundAsync(userInfo, dto);
            return dto;
        }

        public async Task<List<HomeworkDto>> GetHomeworkByGroupIdAsync(int groupId, UserIdentityInfo userInfo)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);
            if (!userInfo.IsAdmin())
                await _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);

            return await _homeworkRepository.GetHomeworkByGroupIdAsync(groupId);
        }

        public async Task<List<HomeworkDto>> GetHomeworkByTaskIdAsync(int taskId)
        {
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            return await _homeworkRepository.GetHomeworkByTaskIdAsync(taskId);
        }

        public async Task<HomeworkDto> AddHomeworkAsync(int groupId, int taskId, HomeworkDto dto, UserIdentityInfo userInfo)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);
            await _taskValidationHelper.GetTaskByIdAndThrowIfNotFoundAsync(taskId);
            if (!userInfo.IsAdmin())
                await _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);

            dto.Group = new GroupDto { Id = groupId };
            dto.Task = new TaskDto { Id = taskId };
            var id = await _homeworkRepository.AddHomeworkAsync(dto);
            return await _homeworkRepository.GetHomeworkAsync(id);
        }

        public async Task DeleteHomeworkAsync(int homeworkId, UserIdentityInfo userInfo)
        {
            var dto = await _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFoundAsync(homeworkId);
            await CheckAccessAndExistenceAndThrowIfNotFoundAsync(userInfo, dto);

            await _homeworkRepository.DeleteHomeworkAsync(homeworkId);
        }

        public async Task<HomeworkDto> UpdateHomeworkAsync(int homeworkId, HomeworkDto dto, UserIdentityInfo userInfo)
        {
            var homeworkDto = await _homeworkValidationHelper.GetHomeworkByIdAndThrowIfNotFoundAsync(homeworkId);
            await CheckAccessAndExistenceAndThrowIfNotFoundAsync(userInfo, homeworkDto);

            homeworkDto.Id = homeworkId;
            homeworkDto.StartDate = dto.StartDate;
            homeworkDto.EndDate = dto.EndDate;
            await _homeworkRepository.UpdateHomeworkAsync(homeworkDto);
            return await _homeworkRepository.GetHomeworkAsync(homeworkId);
        }

        private async Task CheckAccessAndExistenceAndThrowIfNotFoundAsync(UserIdentityInfo userInfo, HomeworkDto dto)
        {
            if (userInfo.IsAdmin()) { return; }
            var groupId = dto.Group.Id;
            await _groupValidationHelper.CheckUserInGroupExistenceAsync(groupId, userInfo.UserId);
        }
    }
}