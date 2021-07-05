using System.Data;
using Dapper;
using DevEdu.DAL.Repositories;

public class GroupRepository : BaseRepository, IGroupRepository
{
    public  void AddGroupLesson(int groupId, int lessonId)
    {
        _connection.Execute("[dbo].[Group_Lesson_Insert]", new
            {
                groupId,
                lessonId
            },
            commandType: CommandType.StoredProcedure);
    }

    public void RemoveGroupLesson(int groupId, int lessonId)
    {
        _connection.Execute("dbo.Group_Lesson_Delete", new
            {
            groupId,
            lessonId
        },
            commandType: CommandType.StoredProcedure);
    }
}