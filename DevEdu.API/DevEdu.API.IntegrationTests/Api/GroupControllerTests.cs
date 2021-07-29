using DevEdu.API.Models.InputModels;
using System.Net;
using System.Net.Http;
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

        [Fact]
        public async Task AddGroupsTestAsync()
        {
            // Arrange
            var model = new GroupInputModel
            {
                Name = "Котейкин дом",
                CourseId = 1,
                GroupStatusId = DAL.Enums.GroupStatus.Forming,
                PaymentPerMonth = 3500.0M,
                StartDate = "21.03.2000",
                Timetable = "День"
            };
            var request = new
            {
                Url = $"api/Group/",
                Body = new
                {
                    Name = model.Name,
                    CourseId = model.CourseId,
                    GroupStatusId = model.GroupStatusId,
                    StartDate = model.StartDate,
                    Timetable = model.Timetable,
                    PaymentPerMonth = model.PaymentPerMonth
                }
            };

            // Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE")]
        public async Task DeleteGroup(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "api/Group/");

            // Act
            var response = await _client.DeleteAsync(request.RequestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData(2)]
        public async Task UpdateGroup(int id)
        {
            // Arrange
            var model = new GroupInputModel
            {
                Name = "Котейкин дом",
                CourseId = 1,
                GroupStatusId = DAL.Enums.GroupStatus.Forming,
                PaymentPerMonth = 3500.0M,
                StartDate = "21.03.2000",
                Timetable = "День"
            };

            var request = new
            {
                Url = $"api/Group/{id}",
                Body = new
                {
                    Name = model.Name,
                    CourseId = model.CourseId,
                    GroupStatusId = model.GroupStatusId,
                    StartDate = model.StartDate,
                    Timetable = model.Timetable,
                    PaymentPerMonth = model.PaymentPerMonth
                }
            };

            // Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task ChangeGroupStatus(int groupId, int statusId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/change-status/{statusId}" };

            // Act
            var response = await _client.PutAsync(request.Url, null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task AddGroupToLesson(int groupId, int lessonId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/lesson/{lessonId}" };

            // Act
            var response = await _client.PostAsync(request.Url, null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", 1, 1)]
        public async Task RemoveGroupFromLesson(string method, int groupId, int lessonId)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{groupId}/lesson/{lessonId}");

            // Act
            var response = await _client.DeleteAsync(request.RequestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task AddGroupMaterialReference(int groupId, int materialId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/material/{materialId}" };

            // Act
            var response = await _client.PostAsync(request.Url, null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", 1, 2)]
        public async Task RemoveGroupMaterialReference(string method, int groupId, int materialId)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{groupId}/material/{materialId}");

            // Act
            var response = await _client.DeleteAsync(request.RequestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2, 1)]
        public async Task AddUserToGroup(int groupId, int userId, int roleId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/user/{userId}/role/{roleId}" };

            // Act
            var response = await _client.PostAsync(request.Url, null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData(3, 2)]
        public async Task DeleteUserFromGroup(int groupId, int userId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/user/{userId}" };

            // Act
            var response = await _client.DeleteAsync(request.Url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 1, 2)]
        public async Task GetGroupTask(string method, int groupId, int taskId)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{groupId}/task/{taskId}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 1)]
        public async Task GetTasksByGroupId(string method, int groupId)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{groupId}/tasks");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task AddTaskToGroup(int groupId, int taskId)
        {
            // Arrange
            var request = new { Url = $"api/Group/{groupId}/task/{taskId}" };

            // Act
            var response = await _client.PostAsync(request.Url, null);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", 1, 2)]
        public async Task DeleteTaskFromGroup(string method, int groupId, int taskId)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"api/Group/{groupId}/task/{taskId}");

            // Act
            var response = await _client.DeleteAsync(request.RequestUri);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task UpdateGroupTask(int groupId, int taskId)
        {
            // Arrange
            var request = new
            { 
                Url = $"api/Group/{groupId}/task/{taskId}",
                Body = new
                {

                }
            };

            // Act
            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}