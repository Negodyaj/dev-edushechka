using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Common
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Role[] roles) : base()
        {
            Roles = string.Join(",", roles);
            Roles += $",{Role.Admin}";
        }
    }
}
