using Xunit;
using System;
using System.Text.Json;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
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

                var response = await requestContext.PostAsync("https://demoqa.com/Account/v1/User", new APIRequestContextOptions
                {
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json"}
                    },
                    DataObject = userPayload
                });

                if (response.Status == 201)
                {
                    Console.WriteLine($"Response Status: {response.Status}");
                    var responseBody = await response.TextAsync();
                    Console.WriteLine($"Response Body: {responseBody}");
                }
                else
                {
                    throw new Exception($"Response was not OK: Received Status Code: {response.Status}");
                }
            }
        }
    }
}