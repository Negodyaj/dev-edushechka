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
        [Description("Adds material")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public int AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            return _materialService.AddMaterial(dto);
        }

        // api/material
        [HttpGet]
        [Description("Returns all materials with tags")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public List<MaterialInfoWithTagsOutputModel> GetAllMaterials()
        {
            var dto = _materialService.GetAllMaterials();
            return _mapper.Map<List<MaterialInfoWithTagsOutputModel>>(dto);
        }

        // api/material/5
        [HttpGet("{id}")]
        [Description("Returns material by id with tags and courses")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public MaterialInfoWithTagsAndCoursesOutputModel GetMaterial(int id)
        {
            var dto = _materialService.GetMaterialById(id);
            return _mapper.Map<MaterialInfoWithTagsAndCoursesOutputModel>(dto);
        }

        // api/material/5
        [HttpPut("{id}")]
        [Description("Updates material by id")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public void UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)  
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            _materialService.UpdateMaterial(id, dto);
        }

        // api/material/5/isDeleted/True
        [HttpDelete("{id}/isDeleted/{isDeleted}")]
        [Description("Deletes material")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public void DeleteMaterial(int id, bool isDeleted)
        {
            _materialService.DeleteMaterial(id, isDeleted);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpPost("{materialId}/tag/{tagId}")]
        [Description("Adds tag to material")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public void AddTagToMaterial(int materialId, int tagId)
        {
            _materialService.AddTagToMaterial(materialId, tagId);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        [Description("Deletes tag from material")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _materialService.DeleteTagFromMaterial(materialId, tagId);
        }

        // api/material/by-tag/1
        [HttpGet("by-tag/{tagId}")]
        [Description("Returns materials by tag id")]
        [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
        public List <MaterialInfoOutputModel> GetMaterialsByTagId(int tagId)
        {
            var dto = _materialService.GetMaterialsByTagId(tagId);
            return _mapper.Map<List<MaterialInfoOutputModel>>(dto);
        }

    }
}