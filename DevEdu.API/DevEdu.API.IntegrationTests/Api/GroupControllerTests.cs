using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevEdu.API.IntegrationTests
{
    public class GroupControllerTests : DevEduAPITest
    {
        [Theory]
        [InlineData("GET")]
        public async Task GetGroupTestAsync(string method, int id = 2)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{id}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetGroupsTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/Group/");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("POST")]
        public async Task AddGroupsTestAsync(string method)
        {
            // Arrange
            var model = new GroupInputModel
            {
                Name = "Котейкин дом",
                CourseId = 1,
                PaymentPerMonth = 3500.0M,
                StartDate = "21.03.2000",
                Timetable = "День"
            };
            var request = new HttpRequestMessage(new HttpMethod(method), "api/Group/");
            var content = new StringContent
            (
                $"Name={model.Name}" +
                $"&CourseId={model.CourseId}" +
                $"&PaymentPerMonth={model.PaymentPerMonth}" +
                $"&StartDate={model.StartDate}" +
                $"&Timetable={model.Timetable}",
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await _client.PostAsync(request.RequestUri, content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

    }
}