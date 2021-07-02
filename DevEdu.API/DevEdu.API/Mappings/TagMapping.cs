using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Mappings
{
    public class TagMapping
    {
        public TagDto MapTagInputModelToTagDto(TagInputModel model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TagInputModel, TagDto>());
            Mapper mapper = new Mapper(config);
            TagDto tagDto = mapper.Map<TagDto>(model);
            return tagDto;
        }

        public List<TagDto> MapTagInputModelToTagDto(List<TagInputModel> models)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TagInputModel, TagDto>());
            Mapper mapper = new Mapper(config);
            List<TagDto> tagDtos = mapper.Map<List<TagDto>>(models);
            return tagDtos;
        }
    }
}
