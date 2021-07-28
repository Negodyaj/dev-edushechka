using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DevEdu.API.IntegrationTests
{
    public class DevEduAPITest
    {
        protected readonly HttpClient _client;
        public DevEduAPITest()
        {
            var server = new TestServer
            (
                new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
            );
            _client = server.CreateClient();
        }
        protected class ContentHelper
        {
            public static StringContent GetStringContent(object obj)
                => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }
    }
}