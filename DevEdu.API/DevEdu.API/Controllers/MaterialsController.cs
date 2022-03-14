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
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMaterialService _materialService;

        public MaterialsController(IMapper mapper, IMaterialService materialService)
        {
            _materialService = materialService;
            _mapper = mapper;
        }

        // api/materials
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all materials")]
        [ProducesResponseType(typeof(List<MaterialInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<MaterialInfoOutputModel>> GetAllMaterialsAsync()
        {
            var user = this.GetUserIdAndRoles();
            var list = await _materialService.GetAllMaterialsAsync(user);

            return _mapper.Map<List<MaterialInfoOutputModel>>(list);
        }

        // api/materials/5/full
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{id}/full")]
        [Description("Get material by id with courses")]
        [ProducesResponseType(typeof(MaterialInfoWithCoursesOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<MaterialInfoWithCoursesOutputModel> GetMaterialByIdWithCoursesAndGroupsAsync(int id)
        {
            var dto = await _materialService.GetMaterialByIdWithCoursesAsync(id);

            return _mapper.Map<MaterialInfoWithCoursesOutputModel>(dto);
        }

        // api/materials/5/
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Get material by id")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<MaterialInfoOutputModel> GetMaterialByIdAsync(int id)
        {
            var user = this.GetUserIdAndRoles();
            var dto = await _materialService.GetMaterialByIdAsync(id, user);

            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

        // api/materials/5
        [AuthorizeRoles(Role.Methodist)]
        [HttpPut("{id}")]
        [Description("Update material by id")]
        [ProducesResponseType(typeof(MaterialInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<MaterialInfoOutputModel> UpdateMaterialAsync(int id, [FromBody] MaterialInputModel materialModel)
        {
            var user = this.GetUserIdAndRoles();
            var dto = _mapper.Map<MaterialDto>(materialModel);
            dto = await _materialService.UpdateMaterialAsync(id, dto, user);

            return _mapper.Map<MaterialInfoOutputModel>(dto);
        }

        // api/materials/5/isDeleted/True
        [AuthorizeRoles(Role.Methodist)]
        [HttpDelete("{id}/isDeleted/{isDeleted}")]
        [Description("Delete/Restore material")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAndRestoreMaterialAsync(int id, bool isDeleted)
        {
            var user = this.GetUserIdAndRoles();
            await _materialService.DeleteMaterialAsync(id, isDeleted, user);

            return NoContent();
        }
    }
}