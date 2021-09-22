using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationValidationHelper _notificationValidationHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;


        public NotificationService(INotificationRepository notificationRepository,
            INotificationValidationHelper notificationValidationHelper,
            IGroupRepository groupRepository,
            IUserValidationHelper userValidationHelper,
            IGroupValidationHelper groupValidationHelper)
        {
            _notificationRepository = notificationRepository;
            _notificationValidationHelper = notificationValidationHelper;
            _groupRepository = groupRepository;
            _userValidationHelper = userValidationHelper;
            _groupValidationHelper = groupValidationHelper;
        }

        public async Task<List<NotificationDto>> GetAllNotificationByUserAsync(UserIdentityInfo userInfo)
        {
            var rolesList = userInfo.Roles;
            var notifications = await GetNotificationsByUserIdAsync(userInfo.UserId);
            foreach (var role in rolesList)
            {
                var listByRole = await GetNotificationsByRoleIdAsync((int)role);
                notifications.AddRange(listByRole);
            }

            if (userInfo.IsStudent())
            {
                var groupList = await _groupRepository.GetGroupsByUserIdAsync(userInfo.UserId);
                foreach (var group in groupList)
                {
                    var listByGroup = await GetNotificationsByGroupIdAsync(group.Id);
                    notifications.AddRange(listByGroup);
                }

            }
            notifications = notifications.OrderByDescending(o => o.Date).ToList();

            return notifications;
        }

        public async Task<NotificationDto> GetNotificationAsync(int id)
        {
            var dto = await _notificationValidationHelper.GetNotificationByIdAndThrowIfNotFoundAsync(id);
            return dto;
        }

        public async Task<List<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            var list = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            return list;
        }

        public async Task<List<NotificationDto>> GetNotificationsByGroupIdAsync(int groupId)
        {
            await _groupValidationHelper.CheckGroupExistenceAsync(groupId);
            var list = await _notificationRepository.GetNotificationsByGroupIdAsync(groupId);
            return list;
        }

        public async Task<List<NotificationDto>> GetNotificationsByRoleIdAsync(int RoleId)
        {
            var list = await _notificationRepository.GetNotificationsByRoleIdAsync(RoleId);
            return list;
        }

        public async Task<NotificationDto> AddNotificationAsync(NotificationDto dto, UserIdentityInfo userInfo)
        {
            if (userInfo.IsTeacher())
            {
                _notificationValidationHelper.CheckNotificationIsForGroup(dto, userInfo.UserId);
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(dto.Group.Id, userInfo.UserId, Role.Teacher);
            }
            _notificationValidationHelper.CheckRoleIdUserIdGroupIdIsNotNull(dto);
            var output = await _notificationRepository.AddNotificationAsync(dto);
            return await GetNotificationAsync(output);
        }

        public async Task DeleteNotificationAsync(int id, UserIdentityInfo userInfo)
        {
            var checkedDto = await _notificationValidationHelper.GetNotificationByIdAndThrowIfNotFoundAsync(id);
            if (userInfo.IsTeacher())
            {
                _notificationValidationHelper.CheckNotificationIsForGroup(checkedDto, userInfo.UserId);
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(checkedDto.Group.Id, userInfo.UserId, Role.Teacher);
            }
            await _notificationRepository.DeleteNotificationAsync(id);
        }

        public async Task<NotificationDto> UpdateNotificationAsync(int id, NotificationDto dto, UserIdentityInfo userInfo)
        {
            var checkedDto = await _notificationValidationHelper.GetNotificationByIdAndThrowIfNotFoundAsync(id);
            dto.Id = id;
            if (userInfo.IsTeacher())
            {
                _notificationValidationHelper.CheckNotificationIsForGroup(checkedDto, userInfo.UserId);
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(checkedDto.Group.Id, userInfo.UserId, Role.Teacher);
            }
            await _notificationRepository.UpdateNotificationAsync(dto);
            return await _notificationRepository.GetNotificationAsync(id);
        }
    }
}