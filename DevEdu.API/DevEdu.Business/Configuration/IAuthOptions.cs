using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public interface IAuthOptions
    {
        public SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}