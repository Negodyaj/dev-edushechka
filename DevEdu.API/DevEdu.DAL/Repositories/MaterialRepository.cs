using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class MaterialRepository : BaseRepository, IMaterialRepository
    {
        private const string _materialAddProcedure = "dbo.Material_Insert";
        private const string _materialSelectAllProcedure = "dbo.Material_SelectAll";
        private const string _materialSelectByIdProcedure = "dbo.Material_SelectById";
        private const string _materialUpdateProcedure = "dbo.Material_Update";
        private const string _materialDeleteProcedure = "dbo.Material_Delete";
        private const string _addTagToMaterialProcedure = "dbo.Material_Tag_Insert";
        private const string _deleteTagFromMaterialProcedure = "dbo.Material_Tag_Delete";
        private const string _materialSelectAllByTagIdProcedure = "dbo.Material_SelectAllByTagId";

        public MaterialRepository()
        {

        }

        public int AddMaterial(MaterialDto material)
        {
            return _connection.QuerySingle<int>(
                _materialAddProcedure,
                new { material.Content },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<MaterialDto> GetAllMaterials()
        {
            return _connection
                .Query<MaterialDto>(
                    _materialSelectAllProcedure,
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }

        public MaterialDto GetMaterialById(int id)
        {
            return _connection.QuerySingleOrDefault<MaterialDto>(
                _materialSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int UpdateMaterial(MaterialDto material)
        {
            return _connection.Execute(
                _materialUpdateProcedure,
                new
                {
                    material.Id,
                    material.Content
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int DeleteMaterial(int id, bool isDeleted)
        {
            return _connection.Execute(
                _materialDeleteProcedure,
                new 
                { 
                    id,
                    isDeleted
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddTagToMaterial(int materialId, int tagId)
        {
            _connection.Execute(
                _addTagToMaterialProcedure,
                new
                {
                    materialId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int DeleteTagFromMaterial(int materialId, int tagId)
        {
            return _connection.Execute(
                _deleteTagFromMaterialProcedure,
                new
                {
                    materialId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<MaterialDto> GetMaterialsByTagId(int tagId)
        {
            return _connection
                .Query<MaterialDto>(
                    _materialSelectAllByTagIdProcedure,
                    new { tagId },
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }
    }
}
