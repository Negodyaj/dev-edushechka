using Dapper;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class MaterialRepository : BaseRepository, IMaterialRepository
    {
        private const string _addMaterial = "dbo.Material_Insert";
        private const string _getAllMaterials = "dbo.Material_SelectAll";
        private const string _getMaterialById = "dbo.Material_SelectById";
        private const string _updateMaterial = "dbo.Material_Update";
        private const string _deleteMaterial = "dbo.Material_Delete";
        private const string _addTagToMaterial = "dbo.Material_Tag_Insert";
        private const string _deleteTagFromMaterial = "dbo.Material_Tag_Delete";
        private const string _getMaterialsByTagId = "dbo.Material_SelectAllByTagId";

        public MaterialRepository()
        {

        }

        public int AddMaterial(MaterialDto material)
        {
            return _connection.QuerySingle<int>(
                _addMaterial,
                new { material.Content },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<MaterialDto> GetAllMaterials()
        {
            return _connection
                .Query<MaterialDto>(
                    _getAllMaterials,
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }

        public MaterialDto GetMaterialById(int id)
        {
            return _connection.QuerySingleOrDefault<MaterialDto>(
                _getMaterialById,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int UpdateMaterial(MaterialDto material)
        {
            return _connection.Execute(
                _updateMaterial,
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
                _deleteMaterial,
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
                _addTagToMaterial,
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
                _deleteTagFromMaterial,
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
                    _getMaterialsByTagId,
                    new { tagId },
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }
    }
}
