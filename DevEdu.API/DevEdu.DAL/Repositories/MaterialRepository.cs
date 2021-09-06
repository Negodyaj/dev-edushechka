using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class MaterialRepository : BaseRepository, IMaterialRepository
    {
        private const string _materialInsertProcedure = "dbo.Material_Insert";
        private const string _materialSelectAllProcedure = "dbo.Material_SelectAll";
        private const string _materialSelectByIdProcedure = "dbo.Material_SelectById";
        private const string _materialUpdateProcedure = "dbo.Material_Update";
        private const string _materialDeleteProcedure = "dbo.Material_Delete";
        private const string _materialTagInsertProcedure = "dbo.Material_Tag_Insert";
        private const string _materialTagDeleteProcedure = "dbo.Material_Tag_Delete";
        private const string _materialSelectAllByTagIdProcedure = "dbo.Material_SelectAllByTagId";
        private const string _materialSelectAllByCourseIdProcedure = "dbo.Material_SelectByCourseId";

        public MaterialRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public int AddMaterial(MaterialDto material)
        {
            return _connection.QuerySingle<int>(
                _materialInsertProcedure,
                new { material.Content },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<MaterialDto> GetAllMaterials()
        {
            var materialDictionary = new Dictionary<int, MaterialDto>();
            return _connection
                .Query<MaterialDto, TagDto, MaterialDto>(
                    _materialSelectAllProcedure,
                    (material, tag) =>
                    {
                        if (!materialDictionary.TryGetValue(material.Id, out var materialEntry))
                        {
                            materialEntry = material;
                            materialEntry.Tags = new List<TagDto>();
                            materialDictionary.Add(material.Id, materialEntry);
                        }
                        if (tag != null && tag.Name != null)
                        {
                            materialEntry.Tags.Add(tag);
                        }
                        return materialEntry;
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public MaterialDto GetMaterialById(int id)
        {
            MaterialDto result = default;
            return _connection
                .Query<MaterialDto, TagDto, MaterialDto>(
                    _materialSelectByIdProcedure,
                    (material, tag) =>
                    {
                        if (result == null)
                        {
                            result = material;
                            result.Tags = new List<TagDto> { tag };
                        }
                        else
                        {
                            if (tag != null && tag.Name != null)
                            {
                                result.Tags.Add(tag);
                            }
                        }

                        return material;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
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
                _materialTagInsertProcedure,
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
                _materialTagDeleteProcedure,
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
            var materialDictionary = new Dictionary<int, MaterialDto>();
            return _connection
                .Query<MaterialDto, TagDto, MaterialDto>(
                    _materialSelectAllByTagIdProcedure,
                    (material, tag) =>
                    {
                        if (!materialDictionary.TryGetValue(material.Id, out var materialEntry))
                        {
                            materialEntry = material;
                            materialEntry.Tags = new List<TagDto>();
                            materialDictionary.Add(material.Id, materialEntry);
                        }
                        materialEntry.Tags.Add(tag);

                        return materialEntry;
                    },
                    new { tagId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public List<MaterialDto> GetMaterialsByCourseId(int courseId)
        {
            return _connection
                .Query<MaterialDto>(
                    _materialSelectAllByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure
                ).
                ToList();
        }
    }
}