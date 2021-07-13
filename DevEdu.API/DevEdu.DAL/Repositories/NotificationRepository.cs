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
                    userId = notificationDto.User.Id,
                    notificationDto.Text,
                    notificationDto.Role

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
                .Query<NotificationDto, UserDto, Role, NotificationDto>(
                    _notificationSelectByIdProcedure,
                    (notification, user, role) =>
                    {
                        if (result == null)
                        {
                            result = notification;
                            result.User = user;
                            result.User.Roles = new List<Role> { role };
                        }
                        else
                        {
                            result.User.Roles.Add(role);
                        }
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
            var notificationDictionary = new Dictionary<int, NotificationDto>();

            return _connection
                .Query<NotificationDto, UserDto, City, Role, NotificationDto>(
                    _notificationSelectAllByUserIdProcedure,
                    (notification, user, city, role) =>
                    {
                        NotificationDto result;
                        if (!notificationDictionary.TryGetValue(notification.Id, out result))
                        {
                            result = notification;
                            result.User = user;
                            result.User.City = city;
                            result.User.Roles = new List<Role> { role };
                            notificationDictionary.Add(notification.Id, result);
                        }

                        result.User.Roles.Add(role);
                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
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