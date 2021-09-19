using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        Task<UserDto> GetUserByIdAndThrowIfNotFound(int userId);
        Task CheckUserBelongToGroup(int groupId, int userId, Role role);
        Task CheckUserBelongToGroup(int groupId, int userId, List<Role> roles);
        Task CheckAuthorizationUserToGroup(int groupId, int userId, Role role);
    }
}