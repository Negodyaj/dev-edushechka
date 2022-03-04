using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOptions<FilesSettings> _fileSettings;
        private readonly List<string> _acceptableFileExtensions;
        private const string _folderUserPhotoPath = "/media/userPhoto/";

        public UserController(IMapper mapper, IUserService userService,
            IAuthenticationService authenticationService, IOptions<FilesSettings> fileSettings)
        {
            _mapper = mapper;
            _userService = userService;
            _authenticationService = authenticationService;
            _fileSettings = fileSettings;
            _acceptableFileExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        }

        // api/user/userId
        [AuthorizeRoles(Role.Manager)]
        [HttpPut("{userId}")]
        [Description("Update user")]
        [ProducesResponseType(typeof(UserUpdateInfoOutPutModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<UserUpdateInfoOutPutModel>> UpdateUserByIdAsync([FromBody] UserUpdateInputModel model)
        {
            var dtoEntry = _mapper.Map<UserDto>(model);
            var dtoResult = await _userService.UpdateUserAsync(dtoEntry);
            var outPut = _mapper.Map<UserUpdateInfoOutPutModel>(dtoResult);

            return Created(new Uri($"api/User/{outPut.Id}", UriKind.Relative), outPut);
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpGet("{userId}")]
        [Description("Return user by id")]
        [ProducesResponseType(typeof(UserFullInfoOutPutModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<UserFullInfoOutPutModel> GetUserByIdAsync(int userId)
        {
            var dto = await _userService.GetUserByIdAsync(userId);
            var outPut = _mapper.Map<UserFullInfoOutPutModel>(dto);

            return outPut;
        }

        // api/user
        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Description("Return list users")]
        [ProducesResponseType(typeof(List<UserInfoOutPutModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<UserInfoOutPutModel>> GetAllUsersAsync()
        {
            var listDto = await _userService.GetAllUsersAsync();
            var list = _mapper.Map<List<UserInfoOutPutModel>>(listDto);

            return list;
        }

        // api/user/password
        [Authorize]
        [HttpPut("password")]
        [Description("Change user password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangeUserPasswordAsync([FromBody] UserChangePasswordInputModel changePasswordInputModel)
        {
            var userId = this.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            if (!await _authenticationService.VerifyAsync(user.Password, changePasswordInputModel.OldPassword))
                throw new AuthorizationException("Entered old password is wrong");

            user.Password = await _authenticationService.HashPasswordAsync(changePasswordInputModel.NewPassword);
            await _userService.ChangePasswordUserAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpPost("photo")]
        [Description("Upload photo for current user (jpg,png)")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> UpdatePhoto(IFormFile photo)
        {
            if (photo == null)
            {
                throw new ValidationException(nameof(photo), ValidationMessage.PhotoRequired);
            }
            var extension = Path.GetExtension(photo.FileName);
            if (!_acceptableFileExtensions.Contains(extension))
            {
                throw new ValidationException(nameof(photo), ValidationMessage.PhotoWrongExtension);
            }

            var userId = this.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            var staticFolderPath = _fileSettings.Value.PathToStaticFolder;
            if (!string.IsNullOrWhiteSpace(staticFolderPath))
                staticFolderPath = staticFolderPath.TrimEnd('/');
            else
                staticFolderPath = string.Empty;

            var timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

            var pathToSavePhoto = staticFolderPath
                + _folderUserPhotoPath
                + ComputeFileHash(photo)
                + timestamp
                + Path.GetExtension(photo.FileName);

            await CreateFile(pathToSavePhoto, photo);

            TryDeleteFile(user.Photo);

            await _userService.ChangeUserPhotoAsync(userId, pathToSavePhoto);

            var pathToReturn = pathToSavePhoto.TrimStart('.');

            return StatusCode(StatusCodes.Status201Created, pathToReturn);
        }

        // api/user/{userId}
        [AuthorizeRoles(Role.Manager)]
        [HttpDelete("{userId}")]
        [Description("Delete user by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpPost("{userId}/role/{role}")]
        [Description("Add new role to user")]
        [ProducesResponseType(typeof(int), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddRoleToUserAsync(int userId, Role role)
        {
            await _userService.AddUserRoleAsync(userId, (int)role);
            return NoContent();
        }

        // api/user/{userId}/role/{roleId}
        [AuthorizeRoles()]
        [HttpDelete("{userId}/role/{role}")]
        [Description("Delete role from user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRoleFromUserAsync(int userId, Role role)
        {
            await _userService.DeleteUserRoleAsync(userId, (int)role);
            return NoContent();
        }

        private string ComputeFileHash(IFormFile package)
        {
            var hash = "";
            using (var md5 = MD5.Create())
            {
                using (var streamReader = new StreamReader(package.OpenReadStream()))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(streamReader.BaseStream)).Replace("-", "");
                }
            }
            return hash;
        }

        private async Task CreateFile(string path, IFormFile file)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        private void TryDeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch { }
            }
        }
    }
}