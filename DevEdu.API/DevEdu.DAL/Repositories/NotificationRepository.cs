using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;
namespace DevEdu.DAL.Repositories
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        public int AddNotification(NotificationDto notificationDto)
        {
            return _connection.QuerySingle<int>(
                "dbo.Notification_Insert",
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
                "dbo.Notification_Delete",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public NotificationDto GetNotification(int id)
        {
            return _connection.QuerySingle<NotificationDto>(
                "dbo.Notification_SelectById",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<NotificationDto> GetNotificationsByUser(int userId)
        {
            return _connection
                .Query<NotificationDto>(
                    "dbo.Notification_SelectAllByUserId",
                    new { userId },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateNotification(int id, NotificationDto notificationDto)
        {
            _connection.Execute(
                "dbo.Notification_Update",
                new
                {
                    id,
                    notificationDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
