using Bogus;

namespace BookstoreAPI.Tests.Helpers
    {
        public class UserGenerator
        {
            private readonly Faker _faker = new();
            public (string username, string password) GenerateUserCredentials()
            {
                return (_faker.Internet.UserName(), _faker.Internet.Password(8, false, "", "P@ssw0rd123!"));
            }
        }
    }