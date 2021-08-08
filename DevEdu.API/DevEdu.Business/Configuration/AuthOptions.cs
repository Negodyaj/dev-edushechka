using System;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Configuration
{
    public class AuthOptions : IAuthOptions
    {
        public const string Issuer = "MyAuthServer"; // for example auth.myserver.com
        public const string Audience = "MyAuthClient"; // for example myserver.com
        public static string _key ;   // key for encoding last part of the token
        public const int Lifetime = 5; // 5 minutes
        public AuthOptions(IOptions<AuthSettings> options)
        {
            _key = options.Value.KeyForToken; //обработать
            _key = CheckString(_key);
            _key = Environment.GetEnvironmentVariable(_key);
        }
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
        private string CheckString(string str)
        {
            string result = str;
            if (str.Contains("{{") && str.Contains("}}"))
            {
                result = str.Replace("{{", "").Replace("}}", "");
            }
            return result;
        }

    }
}
