using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _groupMaterialInsertReferenceProcedure = "dbo.Group_Material_Insert";
        private const string _groupMaterialDeleteReferenceProcedure = "dbo.Group_Material_Delete";

        public GroupRepository() { }

        public void AddGroupMaterialReference(int materialId, int groupId)
        {
            _connection.Execute(
                _groupMaterialInsertReferenceProcedure,
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteGroupMaterialReference(int materialId, int groupId)
        {
            _connection.Execute(
                _groupMaterialDeleteReferenceProcedure,
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