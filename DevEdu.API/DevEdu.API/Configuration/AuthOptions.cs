using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.API.Configuration
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // for example auth.myserver.com
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена myserver.com
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации подписи
        public const int LIFETIME = 5; // время жизни токена - 5 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}