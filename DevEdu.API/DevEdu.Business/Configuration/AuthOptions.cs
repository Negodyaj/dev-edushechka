using System.Text;
using DevEdu.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public class AuthOptions 
    {
        public const string Issuer = "MyAuthServer"; // for example auth.myserver.com
        public const string Audience = "MyAuthClient"; // for example myserver.com
        private static string _key ;   // key for encoding last part of the token
        public static int Lifetime ; // 5 minutes
        public AuthOptions(IOptions<AuthSettings> options)
        {
            _key = options.Value.KeyForToken;
            Lifetime = options.Value.TokenLifeTime;
        } 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
