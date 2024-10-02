using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookstoreAPI.Services.Helpers
{
    public class PlaywrightHelper
    {
        public static async Task<IAPIRequestContext> CreateRequestContext(string token = null)
        {
            var playwright = await Playwright.CreateAsync();

            // Change to Dictionary<string, string> to allow the use of Add
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };

            // Add the Authorization header if a token is provided
            if (!string.IsNullOrEmpty(token))
            {
                headers.Add("Authorization", $"Bearer {token}");
                System.Console.WriteLine($"Token added: {token}");
            }

            // Use the headers in APIRequestNewContextOptions
            var requestContextOptions = new APIRequestNewContextOptions
            {
                ExtraHTTPHeaders = headers
            };

            return await playwright.APIRequest.NewContextAsync(requestContextOptions);
        }
    }
}