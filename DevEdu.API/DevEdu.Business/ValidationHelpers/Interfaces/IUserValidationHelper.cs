using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        Task CheckAccessChangeDataForUserAsync(int getInfoUserId, int leadId, List<Role> roles);
        Task CheckAuthorizationUserToGroupAsync(int groupId, int userId, Role role);
        Task CheckUserBelongToGroupAsync(int groupId, int userId, List<Role> roles);
        Task CheckUserBelongToGroupAsync(int groupId, int userId, Role role);
        Task<UserDto> GetUserByIdAndThrowIfNotFoundAsync(int userId);
    }
}