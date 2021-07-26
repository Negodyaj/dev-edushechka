using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

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
    }
}