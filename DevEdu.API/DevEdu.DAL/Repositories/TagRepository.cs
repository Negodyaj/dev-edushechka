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
    public class TagRepository
    {
        private string _coneconnectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _idbConnection;
        public TagRepository()
        {
            _idbConnection = new SqlConnection(_coneconnectionString);
        }

        public int InsertTagToTagMaterial(int materialId, int tagId)
        {
            return _idbConnection.QuerySingle("[dbo].[Tag_Material_Insert] @TagId , @MaterialId", new { tagId , materialId }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTagFromTagMaterial(int materialId, int tagId)
        {
            _idbConnection.Query("[dbo].[Tag_Material_Delete] @TagId, @MaterialId", new { tagId , materialId}, commandType: CommandType.StoredProcedure);
        }
        public int InsertTagToTagTask(int taskId, int tagId)
        {
            return _idbConnection.QuerySingle("[dbo].[Tag_Task_Insert] @TagId , @MaterialId", new { tagId, taskId }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTagFromTagTask(int taskId, int tagId)
        {
            _idbConnection.Query("[Tag_Task_Delete] @TagId, @TaskId", new { tagId, taskId }, commandType: CommandType.StoredProcedure);
        }



    }
}
