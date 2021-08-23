using Microsoft.Extensions.Configuration;
using System;

namespace DevEdu.Core
{
    public static class ConfigurationExtensions
    {
         public static void SetEnvironmentVariableForConfiguration(this IConfiguration configuration)
        {
            foreach (var item in configuration.AsEnumerable())
            {
                if (item.Value != null && item.Value.Contains("{{"))
                {
                    var envName = RemoveCurlyBrackets(item.Value);
                    var envValue = Environment.GetEnvironmentVariable(envName);
                    configuration.GetSection(item.Key).Value = envValue;
                }
            }
        }
        private static string RemoveCurlyBrackets(string str)
        {
            return str.Replace("{{", "").Replace("}}", "");
        }
    }
}
