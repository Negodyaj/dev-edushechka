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
        private const string _materialSelectByIdProcedure = "dbo.Material_SelectById";
        private const string _materialUpdateProcedure = "dbo.Material_Update";
        private const string _materialDeleteOrRestoreProcedure = "dbo.Material_DeleteOrRestore";
        private const string _materialSelectAllByCourseIdProcedure = "dbo.Material_SelectByCourseId";

        public MaterialRepository(IOptions<DatabaseSettings> options) : base(options) {}

        public async Task<int> AddMaterialAsync(MaterialDto material, int courseId)
        {
            return await _connection
                .QuerySingleAsync<int>(
                    _materialInsertProcedure,
                    new
                    {
                        courseId,
                        material.Content,
                        material.Link
                    },
                    commandType: CommandType.StoredProcedure
                );
        }

        public async Task<MaterialDtoWithCourseId> GetMaterialByIdAsync(int id)
        {
            return await _connection
                .QuerySingleOrDefaultAsync<MaterialDtoWithCourseId>(
                    _materialSelectByIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> UpdateMaterialAsync(MaterialDto material)
        {
            return await _connection.ExecuteAsync(
                _materialUpdateProcedure,
                new
                {
                    material.Id,
                    material.Content,
                    material.Link
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteOrRestoreMaterialAsync(int id, bool isDeleted)
        {
            return await _connection.ExecuteAsync(
                _materialDeleteOrRestoreProcedure,
                new
                {
                    id,
                    isDeleted
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<MaterialDto>> GetMaterialsByCourseIdAsync(int courseId)
        {
            return (await _connection
                .QueryAsync<MaterialDto>(
                    _materialSelectAllByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure
                )).ToList();
        }
    }
}