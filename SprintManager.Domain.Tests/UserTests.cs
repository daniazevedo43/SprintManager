using SprintManager.Domain.Entities;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Domain.Tests
{
    public class UserTests
    {
        // Test user creation
        [Fact]
        public void User_Constructor_WithValidData_CreatesUserSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            string passwordHash = user.PasswordHash;

            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("Daniel", user.Name);
            Assert.Equal("d@gmail.com", user.Email);
            Assert.True(user.VerifyPassword("abc123abc123"));
        }

        // Test user's name change
        [Fact]
        public void SetName_UpdatesNameSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetName("Tiago");

            Assert.Equal("Tiago", user.Name);
        }

        // Test email change
        [Fact]
        public void SetEmail_UpdatesEmailSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetEmail("t@gmail.com");

            Assert.Equal("t@gmail.com", user.Email);
        }
        
        // Test password change
        [Fact]
        public void SetPassword_UpdatesPasswordSuccessfully()
        {
            User user = new User("Daniel", "d@gmail.com", "abc123abc123");

            user.SetPassword("abc456abc456");

            string passwordHash = user.PasswordHash;

            Assert.True(user.VerifyPassword("abc456abc456"));
        }

        // Test exception throwing when user is null or empty
        [Theory] 
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() => 
                new User(name, "d@gmail.com", "abc123abc123")
            );

            Assert.Equal("Name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('D', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() => 
                new User(name, "d@gmail.com", "abc123abc123")
            );

            Assert.Equal($"Name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when email is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyEmail_ThrowsException_WhenEmailIsNullEmpty(string email)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new User("Daniel", email, "abc123abc123")
            );

            Assert.Equal("Email can't be null or empty. (Parameter 'email')", exception.Message);
        }

        // Test exception throwing when email is too long
        [Fact]
        public void VerifyEmail_ThrowsException_WhenEmailIsTooLong()
        {
            string email = new string('d', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", email, "abc123abc123")
            );

            Assert.Equal($"Email is too long. (Max length '255') (Actual length '{email.Length}') (Parameter 'email')", exception.Message);
        }

        // Test exception throwing when password is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyPassword_ThrowsException_WhenPasswordIsNullOrEmpty(string password)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new User("Daniel", "d@gmail.com", password)
            );

            Assert.Equal("Password can't be null or empty. (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password is too short
        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooShort()
        {
            string password = new string('a', 11);

            var exception = Assert.Throws<SprintManagerTooShortException>(() =>
                new User("Daniel", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too short. (Min length '12') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password is too long
        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooLong()
        {
            string password = new string('a', 65);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Daniel", "d@gmail.com", password)
            );

            Assert.Equal($"Password is too long. (Max length '64') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }
    }
}