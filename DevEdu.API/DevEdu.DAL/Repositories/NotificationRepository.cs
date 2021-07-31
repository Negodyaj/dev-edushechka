﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        private const string _notificationInsertProcedure = "dbo.Notification_Insert";
        private const string _notificationDeleteProcedure = "dbo.Notification_Delete";
        private const string _notificationSelectByIdProcedure = "dbo.Notification_SelectById";
        private const string _notificationSelectAllByUserIdProcedure = "dbo.Notification_SelectAllByUserId";
        private const string _notificationSelectAllByGroupIdProcedure = "dbo.Notification_SelectAllByGroupId";
        private const string _notificationSelectAllByRoleIdProcedure = "dbo.Notification_SelectAllByRoleId";
        private const string _notificationUpdateProcedure = "dbo.Notification_Update";
        public int AddNotification(NotificationDto notificationDto)
        {
            return _connection.QuerySingle<int>(
                _notificationInsertProcedure,
                new
                {
                    roleId = (notificationDto.Role == 0) ? null : (int?)notificationDto.Role,
                    userId = (notificationDto.User == null) ? null : (int?)notificationDto.User.Id,
                    groupId = (notificationDto.Group == null) ? null : (int?)notificationDto.Group.Id,
                    notificationDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteNotification(int id)
        {
            _connection.Execute(
                _notificationDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public NotificationDto GetNotification(int id)
        {
            NotificationDto result = default;
            return _connection
                .Query<NotificationDto, Role?, UserDto, GroupDto, NotificationDto>(
                    _notificationSelectByIdProcedure,
                    (notification, role, user, group) =>
                    {
                        result = notification;
                        result.Role = (role == null) ? default : role;
                        result.User = user;
                        result.Group = group;

                        return result;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public List<NotificationDto> GetNotificationsByUserId(int userId)
        {

            return _connection
                .Query<NotificationDto, UserDto, NotificationDto>(
                    _notificationSelectAllByUserIdProcedure,
                    (notification, user) =>
                    {
                        NotificationDto result;
                        {
                            result = notification;
                            result.User = user;
                        }
                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<NotificationDto> GetNotificationsByGroupId(int groupId)
        {

            return _connection
                .Query<NotificationDto, GroupDto, NotificationDto>(
                    _notificationSelectAllByGroupIdProcedure,
                    (notification, group) =>
                    {
                        NotificationDto result;
                        {
                            result = notification;
                            result.Group = group;
                        }
                        return result;
                    },
                    new { groupId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<NotificationDto> GetNotificationsByRoleId(int roleId)
        {

            return _connection
                .Query<NotificationDto, Role, NotificationDto>(
                    _notificationSelectAllByRoleIdProcedure,
                    (notification, role) =>
                    {
                        NotificationDto result;
                        {
                            result = notification;
                            result.Role = role;
                        }
                        return result;
                    },
                    new { roleId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateNotification(NotificationDto notificationDto)
        {
            _connection.Execute(
                _notificationUpdateProcedure,
                new
                {
                    notificationDto.Id,
                    notificationDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}