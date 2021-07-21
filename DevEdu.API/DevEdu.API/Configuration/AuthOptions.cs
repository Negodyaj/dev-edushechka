using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.API.Configuration
{
    public class AuthOptions
    {
        public const string _issuer = "MyAuthServer"; // for example auth.myserver.com
        public const string _audience = "MyAuthClient"; // for example myserver.com
        const string _key = "mysupersecret_secretkey!123";   // key for encoding last part of the token
        public const int _lifetime = 5; // 5 minutes
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}