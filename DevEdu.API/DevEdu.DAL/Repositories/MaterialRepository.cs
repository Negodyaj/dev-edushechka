using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class MaterialRepository : BaseRepository, IMaterialRepository
    {
        public MaterialRepository()
        {

        }

        public int AddMaterial(MaterialDto material)
        {
            _query = "dbo.Material_Insert";
            return _connection.QuerySingle<int>(
                _query,
                new { material.Content },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<MaterialDto> GetAllMaterials()
        {
            _query = "dbo.Material_SelectAll";
            return _connection
                .Query<MaterialDto>(
                    _query,
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }

        public MaterialDto GetMaterialById(int id)
        {
            _query = "dbo.Material_SelectById";
            return _connection.QuerySingleOrDefault<MaterialDto>(
                _query,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateMaterial(MaterialDto material)
        {
            _query = "dbo.Material_Update";
            _connection.Execute(
                _query,
                new
                {
                    material.Id,
                    material.Content
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteMaterial(int id)
        {
            _query = "dbo.Material_Delete";
            _connection.Execute(
                _query,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddTagToMaterial(int materialId, int tagId)
        {
            _query = "dbo.Tag_Material_Insert";
            _connection.Execute(
                _query,
                new
                {
                    materialId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _query = "dbo.Tag_Material_Delete";
            _connection.Execute(
                _query,
                new
                {
                    materialId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
