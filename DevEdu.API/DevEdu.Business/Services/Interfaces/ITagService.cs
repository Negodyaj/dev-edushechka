using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ITagService
    {
        TagDto AddTag(TagDto dto);
        void DeleteTag(int id);
        List<TagDto> GetAllTags();
        TagDto GetTagById(int id);
        TagDto UpdateTag(TagDto dto, int id);
    }
}