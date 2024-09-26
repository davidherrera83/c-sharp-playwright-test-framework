using System;
using System.Text.Json;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookstoreAPI.Services.Account
{
    public class AccountService
    {
        public static async Task<string> CreateUser(string userName, string password)
        {
            using var playwright = await Playwright.CreateAsync();
            var requestContext = await playwright.APIRequest.NewContextAsync();
            var userPayload = new
            {
                userName,
                password
            };

            var response = await requestContext.PostAsync("https://demoqa.com/Account/v1/User", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                DataObject = userPayload
            });

            if (response.Status == 201)
            {
                var responseBody = await response.TextAsync();
                var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (responseJson != null)
                {
                    if (responseJson.TryGetValue("userID", out var userId) && userId != null)
                    {
                        return userId.ToString();
                    }
                    else
                    {
                        throw new InvalidOperationException("User ID not found in the response.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Response JSON is null.");
                }
            }
            else
            {
                throw new Exception($"Failed to create user: Received Status Code: {response.Status}");
            }
        }

        public static async Task<bool> DeleteUser(string userId)
        {
            using var playwright = await Playwright.CreateAsync();
            var requestContext = await playwright.APIRequest.NewContextAsync();
            var response = await requestContext.DeleteAsync($"https://demoqa.com/Account/v1/User/{userId}");

            if (response.Status == 200)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete user with ID {userId}: Received Status Code: {response.Status}");
            }
        }
    }
}