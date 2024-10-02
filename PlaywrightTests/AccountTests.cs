using Xunit;
using Bogus;
using BookstoreAPI.Tests.Helpers;
using BookstoreAPI.Services.Account;

namespace BookstoreAPI.Tests
{
    public class AccountTests 
    {
        private readonly AccountService _accountService = new();
        private readonly UserGenerator _userGenerator = new();
        private readonly Faker _faker = new();

        [Fact]
        public async Task CreateUser()
        {
            //Arrange: Generating test data using Faker
            var (username, password) = _userGenerator.GenerateUserCredentials();
            //Act: Call the service to create a new user
            var userId = await _accountService.CreateUser(username, password);
            System.Console.WriteLine(userId);
            //Assert: Ensure the userId is not null or empty
            Assert.False(string.IsNullOrEmpty(userId), "User creation failed, returned userId is null or empty.");
        }

        [Fact]
        public async Task DeleteUserTest()
        {
            // Arrange: Create a user and generate a token using the generated user credentials
            string username = _faker.Internet.UserName();
            string password = "P@ssw0rd123!"; // Ensure it meets password criteria
            var userId = await _accountService.CreateUser(username, password);
            var token = await _accountService.GenerateToken(username, password);

            System.Console.WriteLine($"Generated Token: {token}");
            System.Console.WriteLine($"User ID: {userId}, Username: {username}");

            // Act: Directly delete the user without checking authorization explicitly
            var isDeleted = await _accountService.DeleteUser(userId, token);

            // Assert: Verify the user was successfully deleted
            Assert.True(isDeleted, $"Failed to delete user with ID {userId}.");
        }
    }
}
