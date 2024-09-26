using Xunit;
using System.Threading.Tasks;
using BookstoreAPI.Services.Account;

namespace BookstoreAPI.Tests
{
    public class AccountTests
    {
      private readonly AccountService _accountServive = new AccountService();

        [Fact]
        public async Task CreateUser()
        {
            //Act: Call the service to create a new user
            var userId = await AccountService.CreateUser("davidtest4", "P@ssw0rd123!");
            System.Console.WriteLine(userId);
            //Assert: Ensure the userId is not null or empty
            Assert.False(string.IsNullOrEmpty(userId), "User creation failed, returned userId is null or empty.");
        }

        [Fact]
        public async Task DeleteUser()
        {
            // Arrange: First create a user to delete
            var userId = await AccountService.CreateUser("testuser", "P@ssw0rd123!");

            // Act: Delete the user
            var isDeleted = await AccountService.DeleteUser(userId);

            // Assert: Ensure the user was successfully deleted
            Assert.True(isDeleted, $"Failed to delete user with ID {userId}.");
        }
    }
}
