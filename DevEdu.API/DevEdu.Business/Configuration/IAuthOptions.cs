using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public interface IAuthOptions
    {
        int Lifetime { get; set; }
        string Issuer { get; }
        string Audience { get; }

        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}