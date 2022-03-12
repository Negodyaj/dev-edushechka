using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Helpers;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly IOptions<FilesSettings> _fileSettings;
        private readonly IFileHelper _workWithFiles;
        private const string _folderUserPhotoPath = "/media/userPhoto/";

        public UserService(IUserRepository userRepository, IUserValidationHelper helper,
            IOptions<FilesSettings> fileSettings,
            IFileHelper workWithFiles)
        {
            _userRepository = userRepository;
            _userValidationHelper = helper;
            _fileSettings = fileSettings;
            _workWithFiles = workWithFiles;
        }

        public async Task<UserDto> AddUserAsync(UserDto dto)
        {
            var userInDb = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (userInDb != null)
            {
                throw new NotUniqueException(nameof(UserDto.Email));
            }

            var addedUserId = await _userRepository.AddUserAsync(dto);

            await _userRepository.AddUserRoleAsync(addedUserId, (int)Role.Student);

            var response = await _userRepository.GetUserByIdAsync(addedUserId);

            return response;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(id);

            return user;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email));

            return user;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var list = await _userRepository.GetAllUsersAsync();
            return list;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto dto)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(dto.Id);
            await _userRepository.UpdateUserAsync(dto);

            var user = await _userRepository.GetUserByIdAsync(dto.Id);

            return user;
        }

        public async Task ChangePasswordUserAsync(UserDto dto)
        {
            await _userRepository.UpdateUserPasswordAsync(dto);
        }

        public async Task<string> ChangeUserPhotoAsync(int userId, IFormFile photo)
        {
            var user = await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);

            var staticFolderPath = _fileSettings.Value.PathToStaticFolder;
            if (!string.IsNullOrWhiteSpace(staticFolderPath))
                staticFolderPath = staticFolderPath.TrimEnd('/');
            else
                staticFolderPath = string.Empty;

            var sbPathToSavePhoto = new StringBuilder();
            sbPathToSavePhoto.Append(staticFolderPath);
            sbPathToSavePhoto.Append(_folderUserPhotoPath);
            sbPathToSavePhoto.Append(_workWithFiles.ComputeFileHash(photo));
            sbPathToSavePhoto.Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            sbPathToSavePhoto.Append(Path.GetExtension(photo.FileName));
            var pathToSavePhoto = sbPathToSavePhoto.ToString();

            await _workWithFiles.CreateFile(pathToSavePhoto, photo);

            await _userRepository.UpdateUserPhotoAsync(userId, pathToSavePhoto);

            _workWithFiles.TryDeleteFile(user.Photo);

            var pathToReturn = pathToSavePhoto.TrimStart('.');
            return pathToReturn;
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(id);
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task AddUserRoleAsync(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.AddUserRoleAsync(userId, roleId);
        }

        public async Task DeleteUserRoleAsync(int userId, int roleId)
        {
            await _userValidationHelper.GetUserByIdAndThrowIfNotFoundAsync(userId);
            await _userRepository.DeleteUserRoleAsync(userId, roleId);
        }
    }
}