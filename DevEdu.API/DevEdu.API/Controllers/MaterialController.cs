using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private readonly IMaterialRepository _repository;
        private readonly IMapper _mapper;

        public MaterialController(IMapper mapper, IMaterialRepository repository) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        // api/material
        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            return _repository.AddMaterial(dto);
        }

        // api/material
        [HttpGet]
        public List<MaterialDto> GetAllMaterials()
        {
            return _repository.GetAllMaterials();
        }

        // api/material/5
        [HttpGet("{id}")]
        public MaterialDto GetMaterial(int id)
        {
            return _repository.GetMaterialById(id);
        }

        // api/material/5
        [HttpPut("{id}")] 
        public void UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)  
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            dto.Id = id;
            _repository.UpdateMaterial(dto);
        }

        // api/material/5/isDeleted/True
        [HttpDelete("{id}/isDeleted/{isDeleted}")]
        public void DeleteMaterial(int id, bool isDeleted)
        {
            _repository.DeleteMaterial(id, isDeleted);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpPost("{materialId}/tag/{tagId}")]
        public void AddTagToMaterial(int materialId, int tagId)
        {
            _repository.AddTagToMaterial(materialId, tagId);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _repository.DeleteTagFromMaterial(materialId, tagId);
        }

        // api/material/by-tag/1
        [HttpGet("by-tag/{tagId}")]
        public List<MaterialDto> GetMaterialsByTagId(int tagId)
        {
            return _repository.GetMaterialsByTagId(tagId);
        }

    }
}
