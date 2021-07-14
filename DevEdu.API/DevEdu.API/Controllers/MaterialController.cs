using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        private readonly IMapper _mapper;

        public MaterialController(IMapper mapper, IMaterialService materialService) 
        {
            _materialService = materialService;
            _mapper = mapper;
        }

        // api/material
        [HttpPost]
        [Description("Add material")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status201Created)]
        public MaterialInfoOutputModel AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            int id = _materialService.AddMaterial(dto);
            return GetMaterialInfoOutputModelById(id);
        }

        // api/material
        [HttpGet]
        [Description("Get all materials with tags")]
        [ProducesResponseType(typeof(List<MaterialInfoWithTagsOutputModel>), StatusCodes.Status200OK)]
        public List<MaterialInfoWithTagsOutputModel> GetAllMaterials()
        {
            var dto = _materialService.GetAllMaterials();
            return _mapper.Map<List<MaterialInfoWithTagsOutputModel>>(dto);
        }

        // api/material/5
        [HttpGet("{id}")]
        [Description("Get material by id with tags and courses")]
        [ProducesResponseType(typeof(MaterialInfoWithTagsAndCoursesOutputModel), StatusCodes.Status200OK)]
        public MaterialInfoWithTagsAndCoursesOutputModel GetMaterial(int id)
        {
            var dto = _materialService.GetMaterialById(id);
            return _mapper.Map<MaterialInfoWithTagsAndCoursesOutputModel>(dto);
        }

        // api/material/5
        [HttpPut("{id}")]
        [Description("Update material by id")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        public MaterialInfoOutputModel UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)  
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            _materialService.UpdateMaterial(id, dto);
            return GetMaterialInfoOutputModelById(id);
        }

        // api/material/5/isDeleted/True
        [HttpDelete("{id}/isDeleted/{isDeleted}")]
        [Description("Delete material")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteMaterial(int id, bool isDeleted)
        {
            _materialService.DeleteMaterial(id, isDeleted);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpPost("{materialId}/tag/{tagId}")]
        public void AddTagToMaterial(int materialId, int tagId)
        {
            _materialService.AddTagToMaterial(materialId, tagId);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _materialService.DeleteTagFromMaterial(materialId, tagId);
        }

        // api/material/by-tag/1
        [HttpGet("by-tag/{tagId}")]
        [Description("Get materials by tag id")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        public List<MaterialInfoOutputModel> GetMaterialsByTagId(int tagId)
        {
            var dto = _materialService.GetMaterialsByTagId(tagId);
            return _mapper.Map<List<MaterialInfoOutputModel>>(dto);
        }

        private MaterialInfoOutputModel GetMaterialInfoOutputModelById(int id)
        {
            var dto = _materialService.GetMaterialById(id);
            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

    }
}