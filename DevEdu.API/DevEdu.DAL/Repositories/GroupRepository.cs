using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        public void AddGroupMaterialReference(int materialId, int groupId)
        {
            _connection.Execute(
                "dbo.Group_Material_Insert",
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
                "dbo.Group_Material_Delete",
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