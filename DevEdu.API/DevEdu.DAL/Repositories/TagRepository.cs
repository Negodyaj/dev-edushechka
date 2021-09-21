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
    public class TagRepository : BaseRepository, ITagRepository
    {
        private const string _tagInsertProcedure = "dbo.Tag_Insert";
        private const string _tagDeleteProcedure = "dbo.Tag_Delete";
        private const string _tagSelectAllProcedure = "dbo.Tag_SelectAll";
        private const string _tagSelectByIdProcedure = "dbo.Tag_SelectByID";
        private const string _tagUpdateProcedure = "dbo.Tag_Update";

        public TagRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddTagAsync(TagDto tagDto)
        {
            return await _connection.QuerySingleOrDefaultAsync<int>(
                _tagInsertProcedure,
                new { tagDto.Name },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteTagAsync(int id)
        {
            return await _connection.ExecuteAsync(
                _tagDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<TagDto>> SelectAllTagsAsync()
        {
            return (await _connection.QueryAsync<TagDto>(
                _tagSelectAllProcedure,
                commandType: CommandType.StoredProcedure
            ))
            .ToList();
        }

        public async Task<TagDto> SelectTagByIdAsync(int id)
        {
            return await _connection.QuerySingleOrDefaultAsync<TagDto>(
                _tagSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> UpdateTagAsync(TagDto tagDto)
        {
            return await _connection.ExecuteAsync(
                _tagUpdateProcedure,
                new
                {
                    tagDto.Id,
                    tagDto.Name
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}