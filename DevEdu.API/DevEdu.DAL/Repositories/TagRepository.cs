using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace DevEdu.DAL.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        private const string _tagMaterialAddProcedure = "[dbo].[Tag_Material_Insert]";
        private const string _tagMaterialDeleteProcedure = "[dbo].[Tag_Material_Delete]";
        private const string _tagTaskAddProcedure = "[dbo].[Tag_Task_Insert]";
        private const string _tagTaskDeleteProcedure = "[dbo].[Tag_Task_Delete]";
        public int AddTagToMaterial(int materialId, int tagId)
        {
            return _connection
                .QuerySingle(
                _tagMaterialAddProcedure,
                new { tagId, materialId },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _connection
                .Execute(_tagMaterialDeleteProcedure,
                new { tagId, materialId },
                commandType: CommandType.StoredProcedure
                );
        }
        public int AddTagToTagTask(int taskId, int tagId)
        {
            return _connection
                .QuerySingle(_tagTaskAddProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }
        public void DeleteTagFromTagTask(int taskId, int tagId)
        {
            _connection
                .Execute(_tagTaskDeleteProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
