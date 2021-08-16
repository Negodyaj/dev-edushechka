using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public interface IAuthOptions
    {
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}