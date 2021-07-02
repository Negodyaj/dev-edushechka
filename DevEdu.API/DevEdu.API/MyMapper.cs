using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;

namespace DevEdu.API
{
    public class MyMapper
    {
        public MyMapper()
        {
        }
        public U SingleMapping<T, U>(T model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, U>());
            var mapper = new Mapper(config);
            return mapper.Map<U>(model);
        }
    }
}
