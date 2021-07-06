using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        public const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
        public const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
        public const string _groupMaterialInsertProcedure = "dbo.Group_Material_Insert";
        public const string _groupMaterialDeleteProcedure = "dbo.Group_Material_Delete";

        public  void AddGroupLesson(int groupId, int lessonId)
        {
            _connection.Execute(_insertGroupLesson, new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure);
        }

        public void RemoveGroupLesson(int groupId, int lessonId)
        {
            _connection.Execute(_deleteGroupLesson, new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure);
        }

        public int AddGroupMaterialReference(int materialId, int groupId)
        {
            return _connection.Execute(
                _groupMaterialInsertProcedure,
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int RemoveGroupMaterialReference(int materialId, int groupId)
        {
            return _connection.Execute(
                _groupMaterialDeleteProcedure,
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}