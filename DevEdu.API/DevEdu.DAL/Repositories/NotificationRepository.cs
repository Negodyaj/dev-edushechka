using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        private const string _notificationInsertProcedure =             "dbo.Notification_Insert";
        private const string _notificationDeleteProcedure =             "dbo.Notification_Delete";
        private const string _notificationSelectByIdProcedure =         "dbo.Notification_SelectById";
        private const string _notificationSelectAllByUserIdProcedure =  "dbo.Notification_SelectAllByUserId";
        private const string _notificationUpdateProcedure =             "dbo.Notification_Update";
        public int AddNotification(NotificationDto notificationDto)
        {
            return _connection.QuerySingle<int>(
                _notificationInsertProcedure,
                new
                {

                    userId = (notificationDto.User == null) ? null : (int?)notificationDto.User.Id,
                    roleId = (notificationDto.Role == null) ? null : (int?)notificationDto.Role,
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
                .Query<NotificationDto, Role ,UserDto, NotificationDto >(
                    _notificationSelectByIdProcedure,
                    (notification, role, user) =>
                    {
                        result = notification;
                        result.Role = role;
                        result.User = user;
                        
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
                .Query<NotificationDto, Role, UserDto, NotificationDto>(
                    _notificationSelectAllByUserIdProcedure,
                    (notification, role, user) =>
                    {
                        NotificationDto result;
                        {
                            result = notification;
                            result.Role = role;
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

        public void UpdateNotification( NotificationDto notificationDto)
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