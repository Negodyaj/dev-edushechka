using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ITagRepository
    {
        Task<int> AddTagAsync(TagDto tagDto);
        Task<int> DeleteTagAsync(int id);
        Task<List<TagDto>> SelectAllTagsAsync();
        Task<TagDto> SelectTagByIdAsync(int id);
        Task<int> UpdateTagAsync(TagDto tagDto);
    }
}