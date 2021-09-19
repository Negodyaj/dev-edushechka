using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        // api/material/with-groups
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpPost("with-groups")]
        [Description("Add material with groups")]
        [ProducesResponseType(typeof(MaterialInfoWithGroupsOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async System.Threading.Tasks.Task<ActionResult<MaterialInfoWithGroupsOutputModel>> AddMaterialWithGroupsAsync([FromBody] MaterialWithGroupsInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            var id = _materialService.AddMaterialWithGroups(dto, materialModel.TagsIds, materialModel.GroupsIds, this.GetUserIdAndRoles());
            dto = await _materialService.GetMaterialByIdWithCoursesAndGroupsAsync(id);
            var output = _mapper.Map<MaterialInfoWithGroupsOutputModel>(dto);
            return Created(new Uri($"api/Material/{output.Id}/full", UriKind.Relative), output);
        }

        // api/material/with-courses
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("with-courses")]
        [Description("Add material with courses")]
        [ProducesResponseType(typeof(MaterialInfoWithCoursesOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async System.Threading.Tasks.Task<ActionResult<MaterialInfoWithCoursesOutputModel>> AddMaterialWithCoursesAsync([FromBody] MaterialWithCoursesInputModel materialModel)
        {
            var dto = _mapper.Map<MaterialDto>(materialModel);
            var id = _materialService.AddMaterialWithCourses(dto, materialModel.TagsIds, materialModel.CoursesIds);
            dto = await _materialService.GetMaterialByIdWithCoursesAndGroupsAsync(id);
            var output = _mapper.Map<MaterialInfoWithCoursesOutputModel>(dto);
            return Created(new Uri($"api/Material/{output.Id}/full", UriKind.Relative), output);
        }

        // api/material
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all materials with tags")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<MaterialInfoOutputModel> GetAllMaterials()
        {
            var user = this.GetUserIdAndRoles();
            var list = _materialService.GetAllMaterials(user);
            return _mapper.Map<List<MaterialInfoOutputModel>>(list);
        }

        // api/material/{id}/full
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{id}/full")]
        [Description("Get material by id with tags, courses and groups")]
        [ProducesResponseType(typeof(MaterialInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public MaterialInfoFullOutputModel GetMaterialByIdWithCoursesAndGroups(int id)
        {
            var dto = _materialService.GetMaterialByIdWithCoursesAndGroupsAsync(id);
            return _mapper.Map<MaterialInfoFullOutputModel>(dto);
        }

        // api/material/5/
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Get material by id with tags")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public MaterialInfoOutputModel GetMaterialByIdWithTags(int id)
        {
            var user = this.GetUserIdAndRoles();
            var dto = _materialService.GetMaterialByIdWithTags(id, user);
            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

        // api/material/5
        [AuthorizeRoles(Role.Teacher, Role.Methodist)]
        [HttpPut("{id}")]
        [Description("Update material by id")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async System.Threading.Tasks.Task<MaterialInfoOutputModel> UpdateMaterialAsync(int id, [FromBody] MaterialInputModel materialModel)
        {
            var user = this.GetUserIdAndRoles();
            var dto = _mapper.Map<MaterialDto>(materialModel);
            dto = await _materialService.UpdateMaterialAsync(id, dto, user);
            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

        // api/material/5/isDeleted/True
        [AuthorizeRoles(Role.Teacher, Role.Methodist)]
        [HttpDelete("{id}/isDeleted/{isDeleted}")]
        [Description("Delete material")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteMaterial(int id, bool isDeleted)
        {
            var user = this.GetUserIdAndRoles();
            _materialService.DeleteMaterialAsync(id, isDeleted, user);
            return NoContent();
        }

        // api/material/{materialId}/tag/{tagId}
        [AuthorizeRoles(Role.Manager, Role.Methodist, Role.Teacher)]
        [HttpPost("{materialId}/tag/{tagId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [Description("Add tag to material")]
        public string AddTagToMaterial(int materialId, int tagId)
        {
            _materialService.AddTagToMaterial(materialId, tagId);
            return $"Tag id:{tagId} was added to material id:{materialId}";
        }

        // api/material/{materialId}/tag/{tagId}
        [AuthorizeRoles(Role.Manager, Role.Methodist, Role.Teacher)]
        [HttpDelete("{materialId}/tag/{tagId}")]
        [Description("Delete tag from material")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteTagFromMaterial(int materialId, int tagId)
        {
            _materialService.DeleteTagFromMaterial(materialId, tagId);
            return NoContent();
        }

        // api/material/by-tag/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist, Role.Student)]
        [HttpGet("by-tag/{tagId}")]
        [Description("Get materials by tag id")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<MaterialInfoOutputModel> GetMaterialsByTagId(int tagId)
        {
            var user = this.GetUserIdAndRoles();
            var list = _materialService.GetMaterialsByTagId(tagId, user);
            return _mapper.Map<List<MaterialInfoOutputModel>>(list);
        }
    }
}