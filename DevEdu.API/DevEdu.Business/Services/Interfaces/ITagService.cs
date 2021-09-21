using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ITagService
    {
        Task<TagDto> AddTagAsync(TagDto dto);
        Task DeleteTagAsync(int id);
        Task<List<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task<TagDto> UpdateTagAsync(TagDto dto, int id);
    }
}