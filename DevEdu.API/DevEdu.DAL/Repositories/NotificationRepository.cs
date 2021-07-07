using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;
namespace DevEdu.DAL.Repositories
{


    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        private const string _notificationInsertProcedure =             "dbo.Notification_Insert";
        private const string _notificationDeleteProcedure =             "dbo.Notification_Delete";
        private const string _notificationSelectByIdProcedure =         "dbo.Notification_SelectById";
        private const string _notificationSelectAllByUserProcedure =    "dbo.Notification_SelectAllByUserId";
        private const string _notificationUpdateProcedure =             "dbo.Notification_Update";
        public int AddNotification(NotificationDto notificationDto)
        {
            return _connection.QuerySingle<int>(
                _notificationInsertProcedure,
                new
                {
                    notificationDto.UserId,
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
            return _connection.QuerySingle<NotificationDto>(
                _notificationSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<NotificationDto> GetNotificationsByUser(int userId)
        {
            return _connection
                .Query<NotificationDto>(
                    _notificationSelectAllByUserProcedure,
                    new { userId },
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
