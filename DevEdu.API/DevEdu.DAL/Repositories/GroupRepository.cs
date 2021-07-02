using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

public class GroupRepository
{
    private static string _connectionString =
        "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;" +
        " Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
    private IDbConnection _connection = new SqlConnection(_connectionString);
    public  void AddGroupLesson(int groupId, int lessonId)
    {
        _connection.Query("[dbo].[Group_Lesson_Insert]", new
            {
                groupId,
                lessonId
            },
            commandType: CommandType.StoredProcedure);
    }

    public void RemoveGroupLesson(int groupId, int lessonId)
    {
        _connection.Query("dbo.Group_Lesson_Delete", new
            {
            groupId,
            lessonId
        },
            commandType: CommandType.StoredProcedure);
    }

    public void AddGroupMaterial(int groupId, int materialId)
    {
        _connection.Query("[dbo].[Group_Material_Insert]", new
            {
                groupId,
            materialId
        },
            commandType: CommandType.StoredProcedure);
    }

    public void RemoveGroupMaterial(int groupId, int materialId)
    {
        _connection.Query("dbo.Group_Material_Delete", new
            {
                groupId,
                materialId
        },
            commandType: CommandType.StoredProcedure);
    }
}