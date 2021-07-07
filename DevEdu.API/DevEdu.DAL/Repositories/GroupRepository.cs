using System.Data;
using Dapper;
using DevEdu.DAL.Repositories;

public class GroupRepository : BaseRepository, IGroupRepository
{
    private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
    private const string _userGroupDeleteProcedure = "dbo.Tag_Delete";
    public const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
    public const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
    public const string _insertGroupMaterial = "dbo.Group_Material_Insert";
    public const string _deleteGroupMaterial = "dbo.Group_Material_Delete";

    public void AddGroupLesson(int groupId, int lessonId)
    {
        _connection.Execute(
            _insertGroupLesson,
            new
            {
                groupId,
                lessonId
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public void RemoveGroupLesson(int groupId, int lessonId)
    {
        _connection.Execute(
            _deleteGroupLesson, 
            new
            {
                groupId,
                lessonId
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public void AddGroupMaterialReference(int materialId, int groupId)
    {
        _connection.Execute(
            _insertGroupMaterial,
            new
            {
                materialId,
                groupId
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public void RemoveGroupMaterialReference(int materialId, int groupId)
    {
        _connection.Execute(
            _deleteGroupMaterial,
            new
            {
                materialId,
                groupId
            },
            commandType: CommandType.StoredProcedure
        );
    }
    public int AddUserToGroup(int groupId, int userId, int roleId)
    {
        return _connection.Execute(
            _userGroupInsertProcedure,
            new
            {
                groupId,
                userId,
                roleId
            },
            commandType: CommandType.StoredProcedure
        );
    }
    public int DeleteUserFromGroup(int userId, int groupId)
    {
        return _connection.Execute(
            _userGroupDeleteProcedure,
            new
            {
                userId,
                groupId
            },
            commandType: CommandType.StoredProcedure
        );
    }
}