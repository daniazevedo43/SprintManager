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
            var user = new User("Test", "test", "test@gmail.com", "Test123test123!");

            Assert.Equal("Test", user.Name);
            Assert.Equal("test", user.UserName);
            Assert.Equal("test@gmail.com", user.Email);
        }

        // Test exception throwing when user is null or empty
        [Theory] 
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyName_ThrowsException_WhenNameIsNullOrEmpty(string name)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new User(name, "test", "test@gmail.com", "Test123test123!")
            );

            Assert.Equal("Name can't be null or empty. (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when name is too long
        [Fact]
        public void VerifyName_ThrowsException_WhenNameIsTooLong()
        {
            string name = new string('T', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() => 
                new User(name, "test", "test@gmail.com", "Test123test123!")
            );

            Assert.Equal($"Name is too long. (Max length '255') (Actual length '{name.Length}') (Parameter 'name')", exception.Message);
        }

        // Test exception throwing when username is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyUsername_ThrowsException_WhenUsernameIsNullOrEmpty(string username)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Test", username, "test@gmail.com", "Test123test123!")
            );

            Assert.Equal("Username can't be null or empty. (Parameter 'username')", exception.Message);
        }

        // Test exception throwing when username has blank spaces
        [Theory]
        [InlineData("test test")]
        [InlineData("test test 12")]
        public void VerifyUsername_ThrowsException_WhenUsernameHasBlankSpaces(string username)
        {
            var exception = Assert.Throws<SprintManagerInvalidUsernameException>(() =>
                new User("Test", username, "test@gmail.com", "Test123test123!")
            );

            Assert.Equal("Username can't have blank spaces.", exception.Message);
        }

        // Test exception throwing when username is too long
        [Fact]
        public void VerifyUsername_ThrowsException_WhenUsernameIsTooLong()
        {
            string username = new string('t', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Test", username, "test@gmail.com", "Test123test123!")
            );

            Assert.Equal($"UserName is too long. (Max length '255') (Actual length '{username.Length}') (Parameter 'username')", exception.Message);
        }

        // Test exception throwing when email is null or empty
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyEmail_ThrowsException_WhenEmailIsNullEmpty(string email)
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Test", "test", email, "Test123test123!")
            );

            Assert.Equal("Email can't be null or empty. (Parameter 'email')", exception.Message);
        }

        // Test exception throwing when email is too long
        [Fact]
        public void VerifyEmail_ThrowsException_WhenEmailIsTooLong()
        {
            string email = new string('t', 256);

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Test", "test", email, "Test123test123!")
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
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal("Password can't be null or empty. (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password needs an uppercase letter
        [Theory]
        [InlineData("test123test123!")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsUppercaseLetter(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one uppercase letter.", exception.Message);
        }

        // Test exception throwing when password needs a number
        [Theory]
        [InlineData("Testtestesttest!")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsNumber(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one number.", exception.Message);
        }

        // Test exception throwing when password needs a special character
        [Theory]
        [InlineData("Test123test123t")]
        public void VerifyPassword_ThrowsException_WhenPasswordNeedsSpecialCharacter(string password)
        {
            var exception = Assert.Throws<SprintManagerPasswordRuleException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal($"Password needs to have at least one special character.", exception.Message);
        }

        // Test exception throwing when password is too short
        [Theory]
        [InlineData("Test123!")]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooShort(string password)
        {
            var exception = Assert.Throws<SprintManagerTooShortException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal($"Password is too short. (Min length '12') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }

        // Test exception throwing when password is too long
        [Fact]
        public void VerifyPassword_ThrowsException_WhenPasswordIsTooLong()
        {
            string password = "";

            for (int i = 0; i < 65; i++)
            {
                password += "Test123!";
            }

            var exception = Assert.Throws<SprintManagerTooLongException>(() =>
                new User("Test", "test", "test@gmail.com", password)
            );

            Assert.Equal($"Password is too long. (Max length '64') (Actual length '{password.Length}') (Parameter 'password')", exception.Message);
        }
    }
}