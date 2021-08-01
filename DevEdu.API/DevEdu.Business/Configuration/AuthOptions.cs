using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public class AuthOptions
    {
        public const string Issuer = "MyAuthServer"; // for example auth.myserver.com
        public const string Audience = "MyAuthClient"; // for example myserver.com
        const string _key = "mysupersecret_secretkey!123";   // key for encoding last part of the token
        public const int Lifetime = 30; // 5 minutes
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
