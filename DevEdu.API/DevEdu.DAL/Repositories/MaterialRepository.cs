using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<int> AddMaterialAsync(MaterialDto material)
        {
            return await _connection
                .QuerySingleAsync<int>(
                _materialInsertProcedure,
                new { material.Content },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<MaterialDto>> GetAllMaterialsAsync()
        {
            var materialDictionary = new Dictionary<int, MaterialDto>();

            return (await _connection
                .QueryAsync<MaterialDto, TagDto, MaterialDto>(
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
                ))
                .Distinct()
                .ToList();
        }

        public async Task<MaterialDto> GetMaterialByIdAsync(int id)
        {
            MaterialDto result = default;

            return (await _connection
                .QueryAsync<MaterialDto, TagDto, MaterialDto>(
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
                ))
                .FirstOrDefault();
        }

        public async Task<int> UpdateMaterialAsync(MaterialDto material)
        {
            return await _connection.ExecuteAsync(
                _materialUpdateProcedure,
                new
                {
                    material.Id,
                    material.Content
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteMaterialAsync(int id, bool isDeleted)
        {
            return await _connection.ExecuteAsync(
                _materialDeleteProcedure,
                new
                {
                    id,
                    isDeleted
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddTagToMaterialAsync(int materialId, int tagId)
        {
            await _connection.ExecuteAsync(
                  _materialTagInsertProcedure,
                  new
                  {
                      materialId,
                      tagId
                  },
                  commandType: CommandType.StoredProcedure
              );
        }

        public async Task<int> DeleteTagFromMaterialAsync(int materialId, int tagId)
        {
            return await _connection.ExecuteAsync(
                _materialTagDeleteProcedure,
                new
                {
                    materialId,
                    tagId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<MaterialDto>> GetMaterialsByTagIdAsync(int tagId)
        {
            var materialDictionary = new Dictionary<int, MaterialDto>();

            return (await _connection
                .QueryAsync<MaterialDto, TagDto, MaterialDto>(
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
                ))
                .Distinct()
                .ToList();
        }

        public async Task<List<MaterialDto>> GetMaterialsByCourseIdAsync(int courseId)
        {
            return (await _connection
                .QueryAsync<MaterialDto>(
                    _materialSelectAllByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure
                )).
                ToList();
        }
    }
}