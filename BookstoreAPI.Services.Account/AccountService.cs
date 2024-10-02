using System.Text.Json;
using Microsoft.Playwright;
using BookstoreAPI.Services.Helpers;

namespace BookstoreAPI.Services.Account
{
    public class AccountService
    {
        public async Task<string> CreateUser(string userName, string password)
        {
            var requestContext = await PlaywrightHelper.CreateRequestContext();
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

            var responseBody = await response.TextAsync();  // Capture the response body

            if (response.Status == 201)
            {
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
                // Log the detailed response for debugging
                throw new Exception($"Failed to create user: Received Status Code: {response.Status}. Response Body: {responseBody}");
            }
        }
        public async Task<bool> DeleteUser(string userId, string token)
        {
            var requestContext = await PlaywrightHelper.CreateRequestContext(token);

            var response = await requestContext.DeleteAsync($"https://demoqa.com/Account/v1/User/{userId}");

            if (response.Status == 204)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete user with ID {userId}: Received Status Code: {response.Status}");
            }
        }

        public async Task<string> GenerateToken(string userName, string password)
        {
            var requestContext = await PlaywrightHelper.CreateRequestContext();

            var tokenPayload = new
            {
                userName,
                password
            };

            var response = await requestContext.PostAsync("https://demoqa.com/Account/v1/GenerateToken", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
                DataObject = tokenPayload
            });

            var responseBody = await response.TextAsync();

            if (response.Status == 200)
            {
                var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                if (responseJson != null && responseJson.TryGetValue("token", out var token))
                {
                    return token.ToString();
                }
                else
                {
                    throw new InvalidOperationException("Token not found in the response.");
                }
            }
            else
            {
                throw new Exception($"Failed to generate token: Received Status Code: {response.Status}");
            }
        }
    }
}