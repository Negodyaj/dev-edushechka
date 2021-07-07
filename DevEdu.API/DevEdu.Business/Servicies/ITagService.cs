using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public interface ITagService
    {
        int AddTag(TagDto dto);
        void DeleteTag(int id);
        List<TagDto> GetAllTags();
        TagDto GetTagById(int id);
        void UpdateTag(TagDto dto);
    }
}