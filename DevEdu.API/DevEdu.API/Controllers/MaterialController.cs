using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [Authorize]
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
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpPost("with-groups")]
        [Description("Add material with groups")]
        [ProducesResponseType(typeof(MaterialInfoWithGroupsOutputModel), StatusCodes.Status201Created)]
        public MaterialInfoWithGroupsOutputModel AddMaterialWithGroups([FromBody] MaterialWithGroupsInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            int id = _materialService.AddMaterialWithGroups(dto, materialModel.TagsIds, materialModel.GroupsIds);
            dto = _materialService.GetMaterialByIdWithGroups(id);
            return _mapper.Map<MaterialInfoWithGroupsOutputModel>(dto);
        }

        // api/material
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("with-courses")]
        [Description("Add material with courses")]
        [ProducesResponseType(typeof(MaterialInfoWithCoursesOutputModel), StatusCodes.Status201Created)]
        public MaterialInfoWithCoursesOutputModel AddMaterialWithCourses([FromBody] MaterialWithCoursesInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            int id = _materialService.AddMaterialWithCourses(dto, materialModel.TagsIds, materialModel.CoursesIds);
            dto = _materialService.GetMaterialByIdWithCourses(id);
            return _mapper.Map<MaterialInfoWithCoursesOutputModel>(dto);
        }

        // api/material
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet]
        [Description("Get all materials with tags")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        public List<MaterialInfoOutputModel> GetAllMaterials()
        {
            var dto = _materialService.GetAllMaterials();
            return _mapper.Map<List<MaterialInfoOutputModel>>(dto);
        }

        //сделать эндпоинт для получения материалов доступных студенту по его id (со всех его групп и курсов)
        //сделать эндпоинт для материалов доступных тутору и тичеру (по их группам и курсам, в которых их группы)
        
        // api/material/5
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist, Role.Student)]
        //у студента проверять, доступен ли ему материал с этим id
        //у тичера и тутора проверять достыпные им материалы (по группам и курсам)
        [HttpGet("{id}")]
        [Description("Get material by id with tags, courses and groups")]
        [ProducesResponseType(typeof(MaterialInfoFullOutputModel), StatusCodes.Status200OK)]
        public MaterialInfoFullOutputModel GetMaterial(int id)
        {
            var dto = _materialService.GetMaterialByIdWithCoursesAndGroups(id);
            return _mapper.Map<MaterialInfoFullOutputModel>(dto);
        }

        // api/material/5
        [AuthorizeRoles(Role.Teacher, Role.Methodist)]
        //для методиста проверять, что материал имеет связь с каким-нибудь курсом
        //для тичера проверять, что материал имеет связь с доступными им группами
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
        [AuthorizeRoles(Role.Teacher, Role.Methodist)]
        //для методиста проверять, что материал имеет связь с каким-нибудь курсом
        //для тичера проверять, что материал имеет связь с доступными им группами
        [AuthorizeRoles(Role.Methodist)]
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
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist, Role.Student)]
        //у студента проверять, доступен ли ему материал с этим id
        //у тичера и тутора проверять достыпные им материалы (по группам и курсам)
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