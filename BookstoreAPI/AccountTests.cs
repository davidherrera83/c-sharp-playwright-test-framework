using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Playwright;
using Xunit;
using System.Collections.Generic;

namespace BookstoreAPI.Tests
{
    public class AccountTests
    {
        [Fact]
        public async Task CreateUser()
        {
            using (var playwright = await Playwright.CreateAsync())
            {
                var requestContext = await playwright.APIRequest.NewContextAsync();

                var userPayload = new
                {
                    userName = "TestUser" + System.Guid.NewGuid().ToString(),
                    password = "P@ssw0rd123!"
                };

                var jsonPayload = JsonSerializer.Serialize(userPayload);

                var response = await requestContext.PostAsync("https://demoqa.com/Account/v1/User", new APIRequestContextOptions
                {
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json"}
                    },
                    DataObject = userPayload
                });

                Console.WriteLine($"Response Status: {response.Status}");
                var responseBody = await response.TextAsync();
                Console.WriteLine($"Response Body: {responseBody}");

                Assert.Equal(201, response.Status);
            }
        }
    }
}