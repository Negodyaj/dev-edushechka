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
            dto = _materialService.GetMaterialById(id);
            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

        // api/material
        [HttpGet]
        [Description("Get all materials with tags")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        public List<MaterialInfoOutputModel> GetAllMaterials()
        {
            var dto = _materialService.GetAllMaterials();
            return _mapper.Map<List<MaterialInfoOutputModel>>(dto);
        }

        // api/material/5
        [HttpGet("{id}")]
        [Description("Get material by id with tags, courses and groups")]
        [ProducesResponseType(typeof(MaterialInfoWithCoursesAndGroupsOutputModel), StatusCodes.Status200OK)]
        public MaterialInfoWithCoursesAndGroupsOutputModel GetMaterial(int id)
        {
            var dto = _materialService.GetMaterialByIdWithCoursesAndGroups(id);
            return _mapper.Map<MaterialInfoWithCoursesAndGroupsOutputModel>(dto);
        }

        // api/material/5
        [HttpPut("{id}")]
        [Description("Update material by id")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        public MaterialInfoOutputModel UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)  
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            dto = _materialService.UpdateMaterial(id, dto);
            return _mapper.Map<MaterialInfoOutputModel>(dto);
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
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [Description("Add tag to material")]
        public string AddTagToMaterial(int materialId, int tagId)
        {
            _materialService.AddTagToMaterial(materialId, tagId);
            return $"Tag id: {tagId} added for material id: {materialId}";
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        [Description("Delete tag from material")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        public string DeleteTagFromMaterial(int materialId, int tagId)
        {
            _materialService.DeleteTagFromMaterial(materialId, tagId);
            return $"Tag id: {tagId} deleted from material id: {materialId}";
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
    }
}