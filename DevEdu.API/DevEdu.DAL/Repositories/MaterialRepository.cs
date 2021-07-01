using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DevEdu.DAL.Repositories
{
    public class MaterialRepository
    {
        public string ConnectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _connection;

        public MaterialRepository()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        public int AddMaterial(MaterialDto material)
        {
            return _connection.QuerySingle<int>("dbo.Material_Insert", new
            {
                material.Content
            }, 
            commandType: CommandType.StoredProcedure);
        }

        public List<MaterialDto> GetAllMaterials()
        {
            return _connection.Query<MaterialDto>("dbo.Material_SelectAll", commandType: CommandType.StoredProcedure).AsList(); ;
        }

        public MaterialDto GetMaterialById(int id)
        {
            return _connection.QuerySingle<MaterialDto>("dbo.Material_SelectById @Id", new { id }, commandType: CommandType.StoredProcedure);
        }

        public void UpdateMaterial(MaterialDto material)
        {
            _connection.Query("dbo.Material_Update @Id, @Content", new { material.Id, material.Content}, commandType: CommandType.StoredProcedure);
        }

        public void DeleteMaterial(int id)
        {
            _connection.Query("dbo.Material_Delete @Id", new { id }, commandType: CommandType.StoredProcedure);
        }
    }
}
