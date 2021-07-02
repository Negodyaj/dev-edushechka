using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Mappers
{
    public class ModelToDtoMapper
    {
        public MaterialDto MapMaterial(MaterialInputModel material)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.CreateMap<MaterialInputModel, MaterialDto>()));
            return mapper.Map<MaterialDto>(material);
        }
    }
}
